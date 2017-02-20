
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer
{
    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs(string request)
        {
            mRequest = request;
        }

        public string Request
        {
            get { return mRequest; }
        }
        private string mRequest;
    }
}
