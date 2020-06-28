using System.Collections.Generic;

namespace ServersInfo.Models
{
    /// <summary>
    /// Информация, заносимая в таблицу.
    /// </summary>
    public class TableEntry
    {
        /// <summary>
        /// Хост сервера.
        /// </summary>
        public string ServerHost { get; set; }
        /// <summary>
        /// Список свединей серверов.
        /// </summary>
        public IEnumerable<ServerDbInfo> ServerDbInfos { get; set; }
    }
    /// <summary>
    /// Информация базы данных на сервере.
    /// </summary>
    public class ServerDbInfo
    {
        /// <summary>
        /// Имя БД.
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Размер БД.
        /// </summary>
        public string DatabaseSize { get; set; }
        /// <summary>
        /// Дата обновления.
        /// </summary>
        public string DateUpdated { get; set; }
    }
}
