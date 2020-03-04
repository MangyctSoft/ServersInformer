using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServersInfo.Models
{
    /// <summary>
    /// Информация о сервере
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Расположение сервера
        /// </summary>
        [XmlAttribute("Host")]
        public string Host { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [XmlAttribute("Username")]
        public string User { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [XmlAttribute("Password")]
        public string Password { get; set; }
        /// <summary>
        /// Порт
        /// </summary>
        [XmlAttribute("Port")]
        public string Port { get; set; }
        /// <summary>
        /// Размер дискового пространства в гигабайтах (Gb)
        /// </summary>
        [XmlAttribute("Size")]
        public double Size { get; set; }       

        public override string ToString()
        {
            return $"Host={Host};Username={User};Password={Password};Port={Port}";
        }
    }
}
