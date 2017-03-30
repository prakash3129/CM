// ChromeConnect
//
// by Thanga Prakash - [2016-03-21]

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;


namespace GCC
{
    public class ChromeConnect
    {
        public ChromeConnect()
        {
            
        }
        
        private StringBuilder sbText = new StringBuilder();        
        private HttpListener httpListener;                                
        private string[] sarrStringParts;
        public delegate void MessageRecivedHandler(object sender, MessageRecivedEventArgs e);
        public event MessageRecivedHandler MessageRecived;

        public delegate void ExceptionRaisedHandler(object sender, ExceptionRaisedEventArgs e);
        public event ExceptionRaisedHandler ExceptionRaised;

        public void Start(string Host)
        {
            if (!HttpListener.IsSupported)
            {
                //MessageBox.Show("Windows XP SP2 or Server 2003 is required.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnException(new Exception("HttpListener not supported. Windows XP SP2 or Server 2003 is required."));
                return;
            }
            try
            {
                httpListener = new HttpListener();                
                httpListener.Prefixes.Add(Host);
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(this.ReciveRequest), null);
            }
            catch (Exception e)
            {
                OnException(e);
                //MessageBox.Show(string.Format("Unable to start the web server.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void End()
        {
            if (httpListener != null)
            {
                HttpListener listener = httpListener;
                httpListener = null;
                listener.Close();
            }
        }
        

        public void ReciveRequest(IAsyncResult IAresult)
        {
            if (httpListener == null)
            {
                return; 
            }            
            HttpListenerContext httpContext = httpListener.EndGetContext(IAresult);
            try
            {
                DispatchCommand(httpContext);
            }
            catch (Exception e)
            {
                OnException(e);
                //MessageBox.Show(string.Format("Command failed.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                httpListener.BeginGetContext(new AsyncCallback(this.ReciveRequest), null);
            }
        }

        private void DispatchCommand(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;

            string queryString = string.Empty;
            int urlEndPos = request.RawUrl.IndexOf('?');

            try
            {
                if (request.HttpMethod == "GET")
                {
                    if (urlEndPos != -1 && urlEndPos < request.RawUrl.Length - 1)
                    {
                        queryString = request.RawUrl.Substring(urlEndPos + 1);
                    }
                }
                else
                {
                    queryString = GetQueryStringFromPostData(request);
                }
                
                NameValueCollection queryCollection = SplitNameValuePairs(queryString);
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html";
                response.ContentLength64 = 0;
                Stream output = response.OutputStream;
                
                string url = urlEndPos != -1 ? request.RawUrl.Substring(0, urlEndPos) : request.RawUrl;
                url = url.Trim(new char[] {'/'});
                url = url.ToLower();

                
                if (url.EndsWith("/dummy.gif"))
                {                    
                    url = url.Substring(0, url.Length - 10);
                    //response.ContentType = "img/gif";
                    //response.ContentLength64 = mDummyGif.Length;
                    //output.Write(mDummyGif, 0, mDummyGif.Length);

                    byte[] mDummyGif = Encoding.ASCII.GetBytes("Recived");
                    response.ContentType = "text/plain";
                    response.ContentLength64 = mDummyGif.Length;
                    output.Write(mDummyGif, 0, mDummyGif.Length);

                    //response.ContentType = "text/plain";
                    //response.ContentLength64 = Encoding.ASCII.GetBytes("Satheesh Kumar").Length;
                    //output.Write(Encoding.ASCII.GetBytes("Satheesh Kumar"), 0, Encoding.ASCII.GetBytes("Satheesh Kumar").Length);
                }

                Add(queryCollection["text"], int.Parse(queryCollection["req"]), int.Parse(queryCollection["totalreqs"]),Convert.ToBoolean(queryCollection["clear"]));
                output.Close();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        private static NameValueCollection SplitNameValuePairs(string queryString)
        {
            NameValueCollection queryCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(queryString))
            {
                string[] args = queryString.Split(new char[] { '&' });
                for (int i = 0; i < args.Length; i++)
                {
                    int separatorPos = args[i].IndexOf('=');
                    if (separatorPos > 0 && separatorPos < args[i].Length)
                    {
                        queryCollection[args[i].Substring(0, separatorPos)] = System.Web.HttpUtility.UrlDecode(args[i].Substring(separatorPos + 1), System.Text.Encoding.UTF8);
                    }
                }
            }
            return queryCollection;
        }

        private static string GetQueryStringFromPostData(HttpListenerRequest request)
        {
            string queryString = string.Empty;
            if (request.HasEntityBody)
            {
                Stream body = request.InputStream;
                Encoding encoding = request.ContentEncoding;
                StreamReader reader = new StreamReader(body, encoding);
                queryString = reader.ReadToEnd();
                body.Close();
                reader.Close();
            }
            return queryString;
        }        

        public void Add(string text, int part, int maxParts, bool clear)
        {
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    if (clear)
                        Clear();

                    if (sarrStringParts == null)
                    {
                        sarrStringParts = new string[maxParts];
                    }
                    Debug.Assert(maxParts == sarrStringParts.Length);
                    Debug.Assert(part > 0 && part <= sarrStringParts.Length);

                    sarrStringParts[part - 1] = text;
                    bool stringMissing = false;
                    for (int i = 0; i < sarrStringParts.Length; i++)
                    {
                        if (sarrStringParts[i] == null)
                        {
                            stringMissing = true;
                            break;
                        }
                    }
                    if (!stringMissing)
                    {
                        for (int i = 0; i < sarrStringParts.Length; i++)
                        {
                            sbText.Append(sarrStringParts[i]);
                        }
                        sarrStringParts = null;
                        OnMessageRecived();
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                }
            }
        }

        public void Clear()
        {
            sbText = new StringBuilder();
            sarrStringParts = null;           
        }        

        

        private void OnMessageRecived()
        {
            if (MessageRecived != null)
            {
                MessageRecived(this, new MessageRecivedEventArgs(sbText.ToString()));
            }
        }
        
        private void OnException(Exception ex)
        {
            if (ExceptionRaised != null)
            {
                ExceptionRaised(this, new ExceptionRaisedEventArgs(ex));
            }
        }
    }

    public class MessageRecivedEventArgs : EventArgs
    {
        public string Text { get; internal set; }
        
        public MessageRecivedEventArgs(string sRecivedText)
        {
            this.Text = sRecivedText;
        }
    }

    public class ExceptionRaisedEventArgs : EventArgs
    {
        public Exception Ex { get; internal set; }

        public ExceptionRaisedEventArgs(Exception ex)
        {
            this.Ex = ex;
        }
    }
}
