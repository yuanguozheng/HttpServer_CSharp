using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace HttpServer
{
    public class HttpServer
    {
        public static String HOME_PATH;
        public static String DEFAULT = "index.html";

        private static int sPort;
        private TcpListener listener;
        private Thread thread;

        public HttpServer(int port, string homepath, string defaultDoc = "index.html")
        {
            sPort = port;
            HOME_PATH = homepath;
            DEFAULT = defaultDoc;
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientHandler clientHandler = new ClientHandler(client);
                    thread = new Thread(clientHandler.Run);
                    thread.Start();
                }
                catch
                {

                }
            }
        }

        public void Stop()
        {
            try
            {
                listener.Stop();
                thread.Abort();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
