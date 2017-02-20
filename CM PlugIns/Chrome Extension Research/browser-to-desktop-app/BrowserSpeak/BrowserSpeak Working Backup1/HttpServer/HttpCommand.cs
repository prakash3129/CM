
using System.Collections.Specialized;
using System.Net;


namespace GCC
{
    abstract public class HttpCommand
    {
        public HttpCommand(string name)
        {
            mName = name;
        }

        public abstract void Execute(NameValueCollection parameters, HttpListenerContext context);

        public string Name
        {
            get { return mName; }
        }
        private string mName;

        public void OutputString(HttpListenerResponse response, string responseString)
        {
            if (!string.IsNullOrEmpty(responseString))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
   }
}
