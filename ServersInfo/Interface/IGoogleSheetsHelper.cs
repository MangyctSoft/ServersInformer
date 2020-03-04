using ServersInfo.Models;
using System.Collections.Generic;

namespace ServersInfo.Interface
{
    public interface IGoogleSheetsHelper
    {
        /// <summary>
        /// Добавление записей в таблицу
        /// </summary>
        /// <param name="tableEntry">Список сведений о всех БД</param>
        /// <param name="size">Размер дискового пространства на сервере</param>
        void CreateEntryTable(List<TableEntry> tableEntry, double size);
        /// <summary>
        /// Проверяет корректность идентификатора документа Google Sheets
        /// </summary>
        /// <returns></returns>
        bool CheckExistSpreadsheetId();
    }
}
