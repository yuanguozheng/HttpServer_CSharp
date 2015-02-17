using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Utils
{
    public class UILog
    {
        public delegate void MessageHandler(string msg);

        static MessageHandler handler;

        public event MessageHandler Recevied
        {
            add
            {
                handler += new MessageHandler(value);
            }
            remove
            {
                handler -= new MessageHandler(value);
            }
        }

        public static void HandleMsg(string msg)
        {
            handler(msg);
        }
    }
}
