

using System.Collections.Specialized;
using System.Net;


namespace GCC
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
