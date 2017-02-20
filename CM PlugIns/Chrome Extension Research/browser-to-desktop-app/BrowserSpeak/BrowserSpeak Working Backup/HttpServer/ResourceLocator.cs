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
using System.Net;
using System.Text;

namespace HttpServer
{
    abstract public class ResourceLocator
    {
        public ResourceLocator(string extension)
        {
            mExtensions.Add(extension);
        }
        public abstract void OutputResource(string name, string extension, HttpListenerContext context);

        public List<string> Extensions
        {
            get { return mExtensions; }
        }
        private List<string> mExtensions = new List<string>();

    }
}
