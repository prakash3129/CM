
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GCC
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
