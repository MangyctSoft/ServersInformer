using ServersInfo.Models;
using System.Collections.Generic;

namespace ServersInfo.Interface
{
    /// <summary>
    /// Интерфейс для работы с базами данных.
    /// </summary>
    public interface IDatabaseHelper
    {
        /// <summary>
        /// Получить информацию по БД.
        /// </summary>
        /// <param name="connectionString">Строка подключения к серверу.</param>
        /// <returns></returns>
        IEnumerable<ServerDbInfo> GetDatabaseInfo();
    }
}
