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
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace HttpServer
{
    class BufferTextCommand : HttpCommand
    {
        public BufferTextCommand() :
            base("buffertext")
        {
        }

        public override void Execute(NameValueCollection parameters, HttpListenerContext context)
        {
            string clearParam = parameters["clear"];
            bool clearExisting = !string.IsNullOrEmpty(clearParam) && clearParam.ToLower() == "true";
            if (clearExisting)
            {
                TextBuffer.Instance.Clear();
            }
            TextBuffer.Instance.Add(parameters["text"], int.Parse(parameters["req"]), int.Parse(parameters["totalreqs"]));
        }
    }
}
