using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WinDirector.Input
{
    public static class Text
    {
        private static readonly StringBuilder result = new StringBuilder();
        public static string KeyCodeToUnicode(KeyCode key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            result.Clear();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, 5, 0, inputLocaleIdentifier);

            return result.ToString();
        }

        public delegate void NewTextHandler(TextEventArgs e);
        public static event NewTextHandler OnNewText;


        private static readonly NewTextManager newTextManager = new NewTextManager();
        private class NewTextManager
        {
            public NewTextManager()
            {
                Key.OnKeyDown += Key_OnKeyDown;
            }
            ~NewTextManager()
            {
                Key.OnKeyDown -= Key_OnKeyDown;
                OnNewText = null;
            }
            private void Key_OnKeyDown(KeyEventArgs e)
            {
                OnNewText?.Invoke(new TextEventArgs(KeyCodeToUnicode(e.KeyCode)));
            }
        }

        public class TextListener
        {
            private readonly HashSet<string> sequencesToBeRemoved;
            private readonly Dictionary<string, ValueTuple<string, bool>> sequencesToBeChanged;
            private readonly Dictionary<string, ValueTuple<string, bool>> sequencesBeingWaitedFor;
            // Full Sequence, Sequence So Far, Is Permament

            public event NewTextHandler OnSequenceFound;

            public TextListener()
            {
                sequencesToBeRemoved = new HashSet<string>();
                sequencesToBeChanged = new Dictionary<string, ValueTuple<string, bool>>();
                sequencesBeingWaitedFor = new Dictionary<string, ValueTuple<string, bool>>();
                OnNewText += TextRecorder_OnNewText;
            }
            ~TextListener()
            {
                OnNewText -= TextRecorder_OnNewText;
            }
            private void TextRecorder_OnNewText(TextEventArgs e)
            {
                if (sequencesBeingWaitedFor.Count == 0) { return; }

                foreach (var sequence in sequencesBeingWaitedFor)
                {
                    string fullSequence = sequence.Key;
                    string sequenceSoFar = sequence.Value.Item1;
                    bool isPermament = sequence.Value.Item2;

                    if(fullSequence.ElementAt(sequenceSoFar.Length).ToString() == e.Text)
                    {
                        string newSequenceSoFar = sequenceSoFar + e.Text;
                        if(fullSequence == newSequenceSoFar)
                        {
                            if (isPermament)
                            {
                                sequencesToBeChanged.Add(fullSequence, (string.Empty, true));
                            }
                            else
                            {
                                sequencesToBeRemoved.Add(fullSequence);
                            }

                            OnSequenceFound?.Invoke(new TextEventArgs(fullSequence));
                        }
                        else
                        {
                            sequencesToBeChanged.Add(fullSequence, (newSequenceSoFar, isPermament));
                        }
                    }
                    else if(sequenceSoFar.Length != 0)
                    {
                        if(fullSequence.First().ToString() == e.Text)
                        {
                            sequencesToBeChanged.Add(fullSequence, (e.Text, isPermament));
                        }
                        else
                        {
                            sequencesToBeChanged.Add(fullSequence, (string.Empty, isPermament));
                        }
                        
                    }
                }
                foreach (string sequence in sequencesToBeRemoved)
                {
                    sequencesBeingWaitedFor.Remove(sequence);
                }
                sequencesToBeRemoved.Clear();

                foreach (var sequence in sequencesToBeChanged)
                {
                    sequencesBeingWaitedFor[sequence.Key] = sequence.Value;
                }
                sequencesToBeChanged.Clear();
            }
            public void AddSequence(string sequence, bool oneTimeOnly = false)
            {
                sequencesBeingWaitedFor.Add(sequence, (string.Empty, !oneTimeOnly));
            }
            public void RemoveSequence(string sequence)
            {
                sequencesBeingWaitedFor.Remove(sequence);
            }
            
        }
        public class TextRecorder
        {
            private readonly StringBuilder recorded_text;
            public string RecordedText => recorded_text.ToString();

            private bool Recording = false, Resumed = false;

            public TextRecorder()
            {
                recorded_text = new StringBuilder();
                OnNewText += TextRecorder_OnNewText;
            }
            ~TextRecorder()
            {
                OnNewText -= TextRecorder_OnNewText;
            }

            private void TextRecorder_OnNewText(TextEventArgs e)
            {
                if(!Recording || Resumed) { return; }

                recorded_text.Append(e);
            }

            public void StartRecording()
            {
                recorded_text.Clear();
                Recording = true;
                Resumed = false;
            }
            public void ResumeRecording()
            {
                if (Recording)
                {
                    Resumed = true;
                }
            }
            public void ContinueRecording()
            {
                if(Recording)
                {
                    Resumed = false;
                }
            }
            public string StopRecording()
            {
                Recording = false;
                return RecordedText;
            }
        }
        public class TextEventArgs : EventArgs
        {
            public TextEventArgs(string text)
            {
                Text = text;
            }
            public string Text { get; }
        }

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
    }
}
