using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ServersInfo.Interface;
using ServersInfo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServersInfo.BL
{
    /// <summary>
    /// Класс для работы с Google Sheets
    /// </summary>
    public class GoogleSheetsHelper : IGoogleSheetsHelper
    {
        readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string ApplicationName = "Server Info 2020";
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        private readonly string _spreadsheetId;
        SheetsService service;
        /// <summary>
        /// Местоположение строки подсчета свободного места
        /// </summary>
        private int cellFooter;
        /// <summary>
        /// Свободное место
        /// </summary>
        private double freeSpace;
        /// <summary>
        /// Общий размер всех БД на сервере
        /// </summary>
        private double dbTotalSize;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="clientJson">Имя JSON-файла для связи с документов Google Sheets</param>
        /// <param name="spreadsheetId">Идентификатор документа</param>
        public GoogleSheetsHelper(string clientJson, string spreadsheetId)
        {
            _spreadsheetId = spreadsheetId;
            GoogleCredential credential;
            using (var stream = new FileStream(clientJson, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            // Создать Google Sheets API сервис
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        /// <summary>
        /// Добавление записей в таблицу
        /// </summary>
        /// <param name="tableEntry">Список сведений о всех БД</param>
        /// <param name="size">Размер дискового пространства на сервере</param>
        public void CreateEntryTable(List<TableEntry> tableEntry, double size)
        {
            cellFooter      = 0;
            freeSpace       = 0;
            dbTotalSize     = 0;
            foreach (var item in tableEntry)
            {
                var sheet = item.ServerHost;
                // Проверка листа в документе
                var defSheet = service.Spreadsheets
                                        .Get(_spreadsheetId)
                                        .Execute()
                                        .Sheets
                                        .FirstOrDefault(x => x.Properties.Title.Equals(sheet));
                // Если лист отсутствует, создать его и добавить записи,
                // в противном случаи обновить данные на листе
                if (defSheet == null)
                {                    
                    CreateNewSheet(sheet);
                    CreateHeadSheet(sheet);
                    UpdateEntry(sheet, item.ServerDbInfos);
                }
                else
                {
                    DeleteEntry(sheet);
                    UpdateEntry(sheet, item.ServerDbInfos);
                }
                // Подсчет оставшегося места
                freeSpace = size - dbTotalSize;

                CreateCellFooter(sheet, freeSpace, DateTime.Now.ToString());
            };
        }
        /// <summary>
        /// Проверяет корректность идентификатора документа Google Sheets
        /// </summary>
        /// <returns></returns>
        public bool CheckExistSpreadsheetId()
        {
            try
            {
                var def = service.Spreadsheets.Get(_spreadsheetId).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        /// <summary>
        /// Обновить записи в таблице
        /// </summary>
        /// <param name="sheet">Страница</param>
        /// <param name="rows">Данные для занесения в таблицу</param>
        private void UpdateEntry(string sheet, List<ServerDbInfo> rows)
        {
            var range       = $"{sheet}!A2";
            var valueRange  = new ValueRange();          
            var data        = new List<IList<object>>();
            int i           = 2;
            // Формируем данные для отправки в документ
            foreach (var item in rows)
            {
                var row = new List<object>
                {
                    sheet,                  // Имя сервера (так же название страницы)
                    item.DatabaseName,      // Имя БД
                    item.DatabaseSize,      // Размер БД
                    item.DateUpdated        // Дата обновления
                };
                data.Add(row);
                // Складываем размеры БД
                dbTotalSize += double.Parse(item.DatabaseSize);
                i++;
            }            
            // Задаем отступ последней строки от данных
            cellFooter      = i + 2;

            // Отправляем запрос на изменение данных
            valueRange.Values = data;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, _spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }
        /// <summary>
        /// Создание новой страницы
        /// </summary>
        /// <param name="title">Заголовок страницы</param>
        private void CreateNewSheet(string title)
        {
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = title;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });
            var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, _spreadsheetId);
            batchUpdateRequest.Execute();
        }           
        /// <summary>
        /// Создание заголовка
        /// </summary>
        /// <param name="sheet">Имя страницы</param>
        private void CreateHeadSheet(string sheet)
        {
            var range       = $"{sheet}!A1:D1";
            var valueRange  = new ValueRange();
            var oblist      = new List<object>()
            {
                "Сервер",
                "База данных",
                "Размер в ГБ",
                "Дата обновления"
            };
            valueRange.Values = new List<IList<object>> { oblist };
            // Отправляем запрос на изменение данных
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }
        /// <summary>
        /// Очистка страницы
        /// </summary>
        /// <param name="sheet">Страница</param>
        private void DeleteEntry(string sheet)
        {
            var range           = $"{sheet}!A2:F50";
            var requestBody     = new ClearValuesRequest();
            var deleteRequest   = service.Spreadsheets.Values.Clear(requestBody, _spreadsheetId, range);
            var deleteReponse   = deleteRequest.Execute();
        }
        /// <summary>
        /// Создание последней строки с информацией о оставшемся свободном месте на сервере
        /// </summary>
        /// <param name="sheet">Страница</param>
        /// <param name="freeSpace">Свободное место</param>
        /// <param name="update">Дата обновления</param>
        private void CreateCellFooter(string sheet, double freeSpace, string update)
        {
            var range       = $"{sheet}!A{cellFooter}";
            var valueRange  = new ValueRange();
            var oblist      = new List<object>()
            {
                sheet,
                "Свободно",
                freeSpace.ToString(),
                update
            };
            valueRange.Values = new List<IList<object>> { oblist };
            // Отправляем запрос на изменение данных
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }
    }
}
