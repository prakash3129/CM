
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
