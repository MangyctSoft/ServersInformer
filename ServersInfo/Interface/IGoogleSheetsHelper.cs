using ServersInfo.Models;
using System.Collections.Generic;

namespace ServersInfo.Interface
{
    /// <summary>
    /// Интерфейс для работы с Google Sheets.
    /// </summary>
    public interface IGoogleSheetsHelper
    {
        /// <summary>
        /// Добавление записей в таблицу.
        /// </summary>
        /// <param name="tableEntry">Список сведений о всех БД.</param>
        /// <param name="size">Размер дискового пространства на сервере.</param>
        void CreateEntryTable(IEnumerable<TableEntry> tableEntry, double size);
        /// <summary>
        /// Проверяет корректность идентификатора документа Google Sheets.
        /// </summary>
        /// <returns></returns>
        bool CheckExistSpreadsheetId();
    }
}
