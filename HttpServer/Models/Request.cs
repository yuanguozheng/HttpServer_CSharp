using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Models
{
    class Request
    {
        private String _QueryString;
        public String Method { get; set; }
        public String QueryString
        {
            get
            {
                if (_QueryString == "/")
                {
                    return _QueryString + HttpServer.DEFAULT;
                }
                return _QueryString;
            }
            set
            {
                if (value != _QueryString)
                {
                    _QueryString = value;
                }
            }
        }
        public String Version { get; set; }
        public Dictionary<String, String> Headers { get; set; }
        public String Body { get; set; }
    }
}
