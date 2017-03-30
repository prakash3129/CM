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
using System.Text;
using System.Speech.Synthesis;

namespace BrowserSpeak.Speech
{
    public class SpeechController
    {
        public static SpeechController Instance
        {
            get { return msInstance; }
        }

        public SynthesizerState State
        {
            get { return mSynthesizer.State; }
        }

        public event EventHandler<StateChangedEventArgs> StateChanged
        {
            add { mSynthesizer.StateChanged += value; }
            remove { mSynthesizer.StateChanged -= value; }
        }

        public void Speak(string text)
        {
            if (mSynthesizer.State != SynthesizerState.Ready)
            {
                StopSpeaking();
            }
            mSynthesizer.SpeakAsync(text);
        }

        public void PauseSpeaking()
        {
            mSynthesizer.Pause();
        }

        public void StopSpeaking()
        {
            mSynthesizer.SpeakAsyncCancelAll();
            if (mSynthesizer.State == SynthesizerState.Paused)
            {
                // Need to resume before the cancel will be processed.
                mSynthesizer.Resume();
            }
        }

        public void ResumeSpeaking()
        {
            mSynthesizer.Resume();
        }

        private SpeechController()
        {
        }

        private SpeechSynthesizer mSynthesizer = new SpeechSynthesizer();
        private static SpeechController msInstance = new SpeechController();
    }
}
