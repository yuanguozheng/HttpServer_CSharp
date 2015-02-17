using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Models
{
    public static class Status
    {
        public static Dictionary<int, String> sStatusData;
        static Status()
        {
            sStatusData = new Dictionary<int, String>()
            {
              {101,"Switching Protocols"},
              {200, "OK"}, 
              {201, "Created"}, 
              {202, "Accepted"}, 
              {204, "No Content"}, 
              {206, "Partial Content"}, 
              {301, "Moved Permanently"}, 
              {304, "Not Modified"}, 
              {400, "Bad Request"}, 
              {401, "Unauthorized"},
              {403,"Forbidden"}, 
              {404, "Not Found"}, 
              {405, "Method Not Allowed"}, 
              {416, "Requested Range Not Satisfiable"}, 
              {500, "Internal Server Error"}
            };
        }

        public static String GetDescription(int status)
        {
            if (!sStatusData.ContainsKey(status))
            {
                return null;
            }
            return "" + status.ToString() + " " + sStatusData[status];
        }
    }
}
