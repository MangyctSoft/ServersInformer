using ServersInfo.Models;
using System.Collections.Generic;

namespace ServersInfo.Interface
{
    public interface IDatabaseHelper
    {
        /// <summary>
        /// Получает информацию по каждой БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к серверу</param>
        /// <returns></returns>
        List<ServerDbInfo> GetDatabaseInfo(string connectionString);
    }
}
