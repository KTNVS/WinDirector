﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WinDirector.Input
{
    public static class Text
    {
        public static void CopyToClipboard(string text) => Clipboard.SetText(text);
        public static string PasteFromClipboard() => Clipboard.GetText();


        private static readonly StringBuilder result = new StringBuilder(); // does not paste, just returns string
        public static string KeyCodeToUnicode(KeyCode key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = WinApi.GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = WinApi.MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = WinApi.GetKeyboardLayout(0);

            result.Clear();
            WinApi.ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, 5, 0, inputLocaleIdentifier);

            return result.ToString();
        }


        public delegate void NewTextHandler(TextEventArgs e);

        public class NewTextManager
        {
            public event NewTextHandler OnNewText;
            public NewTextManager()
            {
                Key.KeyHook.Instance.OnKeyDown += Key_OnKeyDown;
            }
            ~NewTextManager()
            {
                OnNewText = null;
            }
            public void Key_OnKeyDown(KeyEventArgs e)
            {
                OnNewText?.Invoke(new TextEventArgs(KeyCodeToUnicode(e.KeyCode)));
            }
        }

        public class TextListener
        {
            private readonly HashSet<string> sequencesToBeRemoved;
            private readonly Dictionary<string, ValueTuple<string, bool>> sequencesToBeChanged;
            private readonly Dictionary<string, ValueTuple<string, bool>> sequencesBeingWaitedFor;
            // Full Sequence, Sequence So Far, Is Permament | TODO: make it so one class looks for one word

            public event NewTextHandler OnSequenceFound; // TODO: creating a MessageBox to this event then hitting enter to close the box causes crashes
            private readonly NewTextManager textManager;

            public TextListener()
            {
                sequencesToBeRemoved = new HashSet<string>();
                sequencesToBeChanged = new Dictionary<string, ValueTuple<string, bool>>();
                sequencesBeingWaitedFor = new Dictionary<string, ValueTuple<string, bool>>();

                textManager = new NewTextManager();
                textManager.OnNewText += TextRecorder_OnNewText;
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
            private readonly NewTextManager textManager;

            private readonly StringBuilder recorded_text;
            public string RecordedText => recorded_text.ToString();

            private bool Recording = false, Resumed = false;

            public TextRecorder()
            {
                recorded_text = new StringBuilder();

                textManager = new NewTextManager();
                textManager.OnNewText += TextRecorder_OnNewText;
            }
            ~TextRecorder()
            {
                textManager.OnNewText -= TextRecorder_OnNewText;
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

    }
}
namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
    }
}
