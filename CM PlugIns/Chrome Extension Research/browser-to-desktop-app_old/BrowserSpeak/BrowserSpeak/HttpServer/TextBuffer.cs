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
using System.Diagnostics;
using System.Text;

namespace HttpServer
{
    public class TextBuffer
    {
        public static TextBuffer Instance
        {
            get { return mInstance; }
        }
        private static TextBuffer mInstance = new TextBuffer();

        public event EventHandler TextBufferChanged;

        public void Clear()
        {
            mText = new StringBuilder();
            mStringParts = null;

            OnTextBufferChanged();
        }
        public void Add(string text, int part, int maxParts)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (mStringParts == null)
                {
                    mStringParts = new string[maxParts];
                }
                Debug.Assert(maxParts ==  mStringParts.Length);
                Debug.Assert(part > 0 && part <= mStringParts.Length);

                mStringParts[part - 1] = text;
                bool stringMissing = false;
                for (int i = 0; i < mStringParts.Length; i++ )
                {
                    if (mStringParts[i] == null)
                    {
                        stringMissing = true;
                        break;
                    }
                }
                if (!stringMissing)
                {
                    for (int i = 0; i < mStringParts.Length; i++)
                    {
                        mText.Append(mStringParts[i]);
                    }
                    mStringParts = null;
                    OnTextBufferChanged();
                }
            }
        }

        public string Text
        {
            get { return mText.ToString(); }
        }

        private void OnTextBufferChanged()
        {
            if (TextBufferChanged != null)
            {
                TextBufferChanged(this, EventArgs.Empty);
            }
        }

        private StringBuilder mText = new StringBuilder();
        private string[] mStringParts;
    }
}
