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
    class StopSpeakingCommand : HttpCommand
    {
        public StopSpeakingCommand() :
            base("stopspeaking")
        {
        }

        public override void Execute(NameValueCollection parameters, HttpListenerContext context)
        {
            SpeechController.Instance.StopSpeaking();
        }
    }
}
