using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Utils
{
    public class Browser
    {
        /// <summary>
        /// Адрес хоста
        /// </summary>
        private string _host;
        /// <summary>
        /// Порт
        /// </summary>
        private int? _port;

        /// <summary>
        /// Получение json по url-адресу
        /// </summary>
        /// <param name="url">Адрес получения json</param>
        /// <returns>Строка в формате json</returns>
        public string GetJson(string url)
        {
            var separatorPosition = url.IndexOf('?');
            var methodUrl = separatorPosition < 0 ? url : url.Substring(0, separatorPosition);
            var parameters = separatorPosition < 0 ? string.Empty : url.Substring(separatorPosition + 1);

            return WebCall.MakeCall(methodUrl, _host, _port).Response;
        }
    }

}
