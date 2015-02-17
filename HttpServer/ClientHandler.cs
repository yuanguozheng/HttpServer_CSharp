using HttpServer.Models;
using HttpServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HttpServer
{
    class ClientHandler
    {
        public static String MIME_PLAINTEXT = "text/plain";

        public static String MIME_HTML = "text/html";

        public static String MIME_DEFAULT_BINARY = "application/octet-stream";

        private TcpClient client;
        private ResponseHeader responseHeader;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
        }

        public void Run()
        {
            NetworkStream ns = client.GetStream();
            string requestMsg = GetRequestString(ns);
            Request request = ToObject(requestMsg);            
            WriteResponseStream(ns, request.QueryString);
            client.Close();
        }

        private String GetRequestString(NetworkStream ns)
        {
            byte[] buffer;
            StringBuilder builder = new StringBuilder();
            int len = 0;
            while (true)
            {
                buffer = new byte[1024];
                len = ns.Read(buffer, 0, buffer.Length);
                builder.Append(Encoding.UTF8.GetString(buffer), 0, len);
                if (len < buffer.Length)
                {
                    break;
                }
            }
            string request = builder.ToString();
            return request;
        }

        private Request ToObject(String requestMsg)
        {
            Request request = new Request();
            request.Headers = new Dictionary<string, string>();
            String[] info = Regex.Split(requestMsg, "\r\n");
            processDict(request, info);
            int bodyIndex = requestMsg.IndexOf("\r\n\r\n") + 4;
            String body = requestMsg.Substring(bodyIndex).Trim();
            request.Body = body;
            return request;
        }

        private void processDict(Request request, String[] info)
        {
            for (int i = 0; i < info.Length; i++)
            {
                String item = info[i];
                if (item == "")
                {
                    break;
                }
                if (i == 0)
                {
                    String[] baseInfo = Regex.Split(item, " ");
                    request.Method = baseInfo[0];
                    request.QueryString = baseInfo[1];
                    request.Version = baseInfo[2];
                    UILog.HandleMsg(item);
                }
                else
                {
                    String[] kvInfo = Regex.Split(item, ": ");
                    request.Headers.Add(kvInfo[0], kvInfo[1]);
                }
            }
        }

        private void WriteResponseStream(NetworkStream ns, String uri)
        {
            responseHeader = new ResponseHeader();
            String realPath = String.Concat(HttpServer.HOME_PATH, uri);
            byte[] headerBin;
            if (!File.Exists(realPath))
            {
                responseHeader.Status = 404;
                responseHeader.ContentType = MIME_HTML;
                String body = "<h1>404 Not Found</h1>";
                byte[] bodyBin = Encoding.UTF8.GetBytes(body);
                responseHeader.ContentLength = bodyBin.Length;
                String header = responseHeader.ToString();
                headerBin = Encoding.UTF8.GetBytes(header);
                ns.Write(headerBin, 0, headerBin.Length);
                ns.Write(bodyBin, 0, bodyBin.Length);
                ns.Flush();
                return;
            }
            responseHeader.Status = 200;
            FileStream fs = null;
            lock(this)
            {
                fs = new FileStream(realPath, FileMode.Open);
            }
            
            responseHeader.ContentType = MimeUtil.GetMIMEType(fs.Name);
            responseHeader.ContentLength = fs.Length;
            headerBin = Encoding.UTF8.GetBytes(responseHeader.ToString());
            ns.Write(headerBin, 0, headerBin.Length);
            
            byte[] buffer;
            while (true)
            {
                buffer = new byte[1024];
                int n = fs.Read(buffer, 0, buffer.Length);
                ns.Write(buffer, 0, n);
                if (n < buffer.Length)
                {
                    break;
                }
            }
            ns.Flush();
        }
    }
}
