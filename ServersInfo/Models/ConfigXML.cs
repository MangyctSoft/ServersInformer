using System.Collections.Generic;

namespace ServersInfo.Models
{
    /// <summary>
    /// Структура конфигурационного файла
    /// </summary>
    public class ConfigXML
    {
        /// <summary>
        /// Время ожидания
        /// </summary>
        public int TimeOut { get; set; }
        /// <summary>
        /// Имя JSON-файла с учетными данными
        /// </summary>
        public string ClientJson { get; set; }
        /// <summary>
        /// Идентификатор документа с таблицами
        /// </summary>
        public string SpreadsheetId { get; set; }
        /// <summary>
        /// Список серверов
        /// </summary>
        public List<Server> Servers { get; set; }
    }
}
