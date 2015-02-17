using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Models
{
    class ResponseHeader
    {
        public ResponseHeader()
        {
            Headers = new Dictionary<string, string>();
        }
        public int Status { get; set; }
        public String ContentType { get; set; }
        public long ContentLength { get; set; }
        public Dictionary<String, String> Headers { get; set; }
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("HTTP/1.1 {0}\r\n", Models.Status.GetDescription(Status));
            builder.AppendFormat("Date: {0}\r\n", DateTime.Now.ToString("r"));
            builder.AppendFormat("Content-Type: {0}\r\n", ContentType);
            builder.AppendFormat("Content-Length: {0}\r\n", ContentLength);
            if (Headers.Count != 0)
            {
                foreach (KeyValuePair<String, String> item in Headers)
                {
                    builder.AppendFormat("{0}: {1}\r\n", item.Key, item.Value);
                }
            }
            builder.Append("\r\n");
            return builder.ToString();
        }
    }
}
