// BrowserSpeak
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//
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
