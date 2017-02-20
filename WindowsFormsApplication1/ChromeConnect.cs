// ChromeConnect
//
// by Thanga Prakash - [2016-03-21]

using System;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Forms;


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
        public delegate void MessageReceivedHandler(object sender, MessageReceivedEventArgs e);
        public event MessageReceivedHandler MessageReceived;

        //public delegate void ExceptionRaisedHandler(object sender, ExceptionRaisedEventArgs e);
        //public event ExceptionRaisedHandler ExceptionRaised;

        public void Start(string Host)
        {
            if (!HttpListener.IsSupported)
            {
                //MessageBox.Show("Windows XP SP2 or Server 2003 is required.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //OnException(new Exception("HttpListener not supported. Windows XP SP2 or Server 2003 is required."));
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),new Exception("HttpListener not supported. Windows XP SP2 or Server 2003 is required."), true, true);                            
                return;
            }
            try
            {
                
                httpListener = new HttpListener();                
                httpListener.Prefixes.Add(Host);
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(this.ReciveRequest), null);
            }
            //catch (Exception e)
            //{
            //    OnException(e);
            //    //MessageBox.Show(string.Format("Unable to start the web server.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
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
            //catch (Exception e)
            //{
            //    OnException(e);
            //    //MessageBox.Show(string.Format("Command failed.\n\n{0}", e.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
            finally
            {
                httpListener.BeginGetContext(new AsyncCallback(this.ReciveRequest), null);
            }
        }

        private void DispatchCommand(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;

            string sQstring = string.Empty;
            int iURLEndPos = request.RawUrl.IndexOf('?');

            try
            {
                if (request.HttpMethod == "GET")
                {
                    if (iURLEndPos != -1 && iURLEndPos < request.RawUrl.Length - 1)
                    {
                        sQstring = request.RawUrl.Substring(iURLEndPos + 1);
                    }
                }
                else
                {
                    sQstring = GetQueryStringFromPostData(request);
                }
                
                NameValueCollection QuerykeyVal = SplitNameValuePairs(sQstring);
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html";
                response.ContentLength64 = 0;
                Stream output = response.OutputStream;
                
                //string url = iURLEndPos != -1 ? request.RawUrl.Substring(0, iURLEndPos) : request.RawUrl;
                //url = url.Trim(new char[] {'/'});
                //url = url.ToLower();
                //url = url.Substring(0, url.Length - 10);

                if (QuerykeyVal.AllKeys.Length > 0)
                {
                    string sValue = QuerykeyVal["text"];
                    int iParts = int.Parse(QuerykeyVal["req"]);
                    int iMaxParts = int.Parse(QuerykeyVal["totalreqs"]);
                    bool IsFirstRequest = Convert.ToBoolean(QuerykeyVal["clear"]);
                    if (!string.IsNullOrEmpty(sValue))
                    {

                        if (IsFirstRequest)
                            Clear();

                        if (sarrStringParts == null)
                        {
                            sarrStringParts = new string[iMaxParts];
                        }
                        //Debug.Assert(iMaxParts == sarrStringParts.Length);
                        //Debug.Assert(iParts > 0 && iParts <= sarrStringParts.Length);
                        sarrStringParts[iParts - 1] = sValue;
                        bool IsStringMissing = false;
                        for (int i = 0; i < sarrStringParts.Length; i++)
                        {
                            if (sarrStringParts[i] == null)
                            {
                                IsStringMissing = true;
                                break;
                            }
                        }
                        if (!IsStringMissing)
                        {
                            for (int i = 0; i < sarrStringParts.Length; i++)
                            {
                                sbText.Append(sarrStringParts[i]);
                            }
                            sarrStringParts = null;
                            //OnMessageReceived();
                            string sResponse = TransRevice(QuerykeyVal, sbText.ToString());

                            byte[] bResponse = Encoding.ASCII.GetBytes(sResponse);
                            response.ContentType = "text/plain";
                            response.ContentLength64 = bResponse.Length;
                            output.Write(bResponse, 0, bResponse.Length);
                        }
                    }
                    //response.ContentType = "img/gif";
                    //response.ContentLength64 = mDummyGif.Length;
                    //output.Write(mDummyGif, 0, mDummyGif.Length);



                    //response.ContentType = "text/plain";
                    //response.ContentLength64 = Encoding.ASCII.GetBytes("Satheesh Kumar").Length;
                    //output.Write(Encoding.ASCII.GetBytes("Satheesh Kumar"), 0, Encoding.ASCII.GetBytes("Satheesh Kumar").Length);


                    //Add(QuerykeyVal);
                }
                output.Close();
            }
            //catch (Exception e)
            //{
            //    OnException(e);
            //}
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
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
                        
                        queryCollection[args[i].Substring(0, separatorPos)] = System.Web.HttpUtility.UrlDecode(args[i].Substring(separatorPos + 1), Encoding.UTF8);
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

        public void Add(NameValueCollection keyVal)
        {
            string text = keyVal["text"];
            int part = int.Parse(keyVal["req"]);
            int maxParts = int.Parse(keyVal["totalreqs"]);
            bool clear = Convert.ToBoolean(keyVal["clear"]);
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
                        OnMessageReceived();


                        TransRevice(keyVal,sbText.ToString());
                    }
                }
                //catch (Exception e)
                //{
                //    OnException(e);
                //}
                catch (Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                }
            }
        }


        string TransRevice(NameValueCollection keyVal, string sValue)
        {
            if (GV.sProjectID.Length == 0) return GM.ChromeResponse("Project");
            Form frmCompanyList = GM.GetForm("frmCompanyList");
            if (frmCompanyList == null) return GM.ChromeResponse("Project");
            if (sValue.Contains("cmd_ReloadFields::{")) return GM.ChromeResponse("Reload");            
            Form frmContactUpdate = GM.GetForm("FrmContactsUpdate");
            if (frmContactUpdate == null) return GM.ChromeResponse("Company");            
            frmContactUpdate.Invoke((MethodInvoker)delegate { frmContactUpdate.Focus();  });
            string sResponse = ((FrmContactsUpdate)frmContactUpdate).RespondChrome(sValue.Split(new string[] { "}ChromeException::" }, StringSplitOptions.None)[0]);
            return sResponse;                
        }

        public void Clear()
        {
            sbText = new StringBuilder();
            sarrStringParts = null;           
        }        

        

        private void OnMessageReceived()
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageReceivedEventArgs(sbText.ToString()));
            }
        }
        
        //private void OnException(Exception ex)
        //{
        //    if (ExceptionRaised != null)
        //    {
        //        ExceptionRaised(this, new ExceptionRaisedEventArgs(ex));
        //    }
        //}
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Text { get; internal set; }
        
        public MessageReceivedEventArgs(string sReceivedText)
        {
            this.Text = sReceivedText;
        }
    }

    //public class ExceptionRaisedEventArgs : EventArgs
    //{
    //    public Exception Ex { get; internal set; }

    //    public ExceptionRaisedEventArgs(Exception ex)
    //    {
    //        this.Ex = ex;
    //    }
    //}
}
