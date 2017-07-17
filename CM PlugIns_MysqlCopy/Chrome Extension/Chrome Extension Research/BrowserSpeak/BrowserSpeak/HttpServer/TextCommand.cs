

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace HttpServer
{
    abstract public class TextCommand : HttpCommand
    {
        public TextCommand(string name) :
            base(name)
        {
            TextBuffer.Instance.TextBufferChanged += new EventHandler(OnTextBufferChanged);
        }

        public string GetTextParameter(NameValueCollection parameters)
        {
            string text = parameters["text"];
            if (string.IsNullOrEmpty(text))
            {
                text = TextBuffer.Instance.Text;
            }
            return text;
        }

        abstract protected void TextAvailable();

        protected bool WaitingForText
        {
            get { return mWaitingForText; }
            set { mWaitingForText = value; }
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            if (mWaitingForText)
            {
                mWaitingForText = false;
                TextAvailable();
            }
        }

        private bool mWaitingForText;
    }
}
