// BrowserSpeak
//
// by Mark Gladding

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace GCC
{
    public class HttpCommandDispatcher
    {
        public HttpCommandDispatcher()
        {
            //MemoryStream stream = new MemoryStream();
            //dummyImage.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
            //mDummyGif = stream.GetBuffer();
        }

        public event RequestReceivedHandler RequestReceived;

        public void Start(string url)
        {
            if (!HttpListener.IsSupported)
            {
                MessageBox.Show("Windows XP SP2 or Server 2003 is required.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                mListener = new HttpListener();
                // Add the prefix.
                mListener.Prefixes.Add(url);
                mListener.Start();
                mListener.BeginGetContext(new AsyncCallback(this.ProcessRequest), null);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Unable to start the web server.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void End()
        {
            if (mListener != null)
            {
                HttpListener listener = mListener;
                mListener = null;
                listener.Close();
            }
        }
        public void AddCommand(HttpCommand command)
        {
            Debug.Assert(!mCommands.ContainsKey(command.Name));
            mCommands[command.Name] = command;
        }

        public void AddResourceLocator(ResourceLocator locator)
        {
            foreach (string extension in locator.Extensions)
            {
                Debug.Assert(!mResourceLocators.ContainsKey(extension));
                mResourceLocators[extension] = locator;
            }
        }

        public void ProcessRequest(IAsyncResult result)
        {
            if (mListener == null)
            {
                return; // Listener is has  been closed
            }
            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context = mListener.EndGetContext(result);
            try
            {
                DispatchCommand(context);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Command failed.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mListener.BeginGetContext(new AsyncCallback(this.ProcessRequest), null);
            }
        }

        private void DispatchCommand(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;

            // Inform listeners that a request has been received.
            // Only send up to 240 characters of the URL
            const int MaxNotifyRequestLength = 240;
            string unescapedUrl = request.RawUrl;
            string notifyRequest = unescapedUrl.Substring(0, Math.Min(MaxNotifyRequestLength, unescapedUrl.Length));
            if (request.RawUrl.Length > notifyRequest.Length)
            {
                notifyRequest += "..."; // Indicate the URL has been truncated.
            }
          //  OnRequestReceived(notifyRequest);

            string queryString = string.Empty;
            int urlEndPos = request.RawUrl.IndexOf('?');
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

            // Split query string args into name/value pairs
            NameValueCollection queryCollection = SplitNameValuePairs(queryString);

            HttpListenerResponse response = context.Response;
            response.ContentType = "text/html";
            response.ContentLength64 = 0;
            Stream output = response.OutputStream;

            // Extract a lowercase URL that doesn't include the query string.
            string url = urlEndPos != -1 ? request.RawUrl.Substring(0, urlEndPos) : request.RawUrl;
            url = url.Trim(new char[] { '/' });
            url = url.ToLower();

            // If it's a request for our dummy gif, send the dummy
            // gif image as a response and then process the actual command
            if (url.EndsWith("/dummy.gif"))
            {
                // Remove /dummy.gif from the url
                url = url.Substring(0, url.Length - 10);

                // Send the gif image

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

            // If the url has an extension (e.g. .png) then the
            // request is for a file (e.g. an image), so see if
            // a ResourceLocator is registered to handle this extension.
            string extension = Path.GetExtension(url);
            bool foundResource = false;
            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.Substring(1);
                ResourceLocator locator;
                if (mResourceLocators.TryGetValue(extension, out locator))
                {
                    locator.OutputResource(Path.GetFileNameWithoutExtension(url), extension, context);
                    foundResource = true;
                }
            }
            // Not a request for a resource, must be a command
            if (!foundResource)
            {
                HttpCommand command;
                if (mCommands.TryGetValue(url, out command))
                {
                    command.Execute(queryCollection, context);
                }
                else
                {
                    // Unknown command.
                    string responseString = string.Format("<html><head><title>Browser Speak Error</title></head><body><p>The command '{0}' is not supported by Browser Speak.</p></body></html>", request.RawUrl);
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            // You must close the output stream.
            output.Close();
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
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                queryString = reader.ReadToEnd();
                body.Close();
                reader.Close();
            }
            return queryString;
        }

        private void OnRequestReceived(string request)
        {
            if (RequestReceived != null)
            {
                RequestReceived(this, new RequestEventArgs(request));
            }
        }

        public delegate void RequestReceivedHandler(object source, RequestEventArgs e);

        private HttpListener mListener;
        private Dictionary<string, HttpCommand> mCommands = new Dictionary<string, HttpCommand>();
        private Dictionary<string, ResourceLocator> mResourceLocators = new Dictionary<string, ResourceLocator>();
        //private byte[] mDummyGif;
    }
}
