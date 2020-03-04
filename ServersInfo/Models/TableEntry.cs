using System;
using System.Collections.Generic;

namespace ServersInfo.Models
{
    /// <summary>
    /// Информация, заносимая в таблицу
    /// </summary>
    public class TableEntry
    {
        /// <summary>
        /// Хост сервера
        /// </summary>
        public string ServerHost { get; set; }
        /// <summary>
        /// Информация о сервере
        /// </summary>
        public List<ServerDbInfo> ServerDbInfos { get; set; }
    }

    public class ServerDbInfo
    {
        /// <summary>
        /// Имя БД
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Размер БД
        /// </summary>
        public string DatabaseSize { get; set; }
        /// <summary>
        /// Дата обновления
        /// </summary>
        public string DateUpdated { get; set; }
    }
}
