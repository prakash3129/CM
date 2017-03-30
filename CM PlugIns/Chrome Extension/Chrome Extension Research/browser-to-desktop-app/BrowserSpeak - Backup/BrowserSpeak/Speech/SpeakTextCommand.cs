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

using HttpServer;

namespace BrowserSpeak.Speech
{
    class SpeakTextCommand : TextCommand
    {
        public SpeakTextCommand() :
            base("speaktext")
        {
        }

        public override void Execute(NameValueCollection parameters, HttpListenerContext context)
        {
            string text = GetTextParameter(parameters);
            if (!string.IsNullOrEmpty(text))
            {
                SpeechController.Instance.Speak(text);
            }
            else
            {
                // Not all the text has arrived from the browser,
                // so we'll need to wait.
                WaitingForText = true;
            }
        }

        protected override void TextAvailable()
        {
            SpeechController.Instance.Speak(TextBuffer.Instance.Text);
        }
    }
}
