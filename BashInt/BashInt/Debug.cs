using System;
using System.Collections.Generic;
using System.Text;

namespace BashInt
{
    public class Debug
    {
        public static Debugger debugger = null;
        public static bool enabled = true;
        public static bool delay = false;

        public static void LoadFile(List<string> text)
        {
           debugger.fastColoredTextBox1.Clear();
           debugger.fastColoredTextBox1.AppendText(string.Join(Environment.NewLine, text.ToArray()));
           // Console.WriteLine("Waiting...");
           // System.Threading.Thread.Sleep(3000);
        }
        public static void LineNo(int no)
        {
            try
            {
                if (enabled)
                {
                    //  debugger.fastColoredTextBox1.OnScroll(new System.Windows.Forms.ScrollEventArgs(System.Windows.Forms.ScrollEventType.LargeIncrement,no), true);
                    debugger.fastColoredTextBox1.ShowLineNumbers = true;
                    debugger.fastColoredTextBox1.BookmarkLine(no);
                    debugger.fastColoredTextBox1.Selection = new FastColoredTextBoxNS.Range(debugger.fastColoredTextBox1,
                        0, no, debugger.fastColoredTextBox1.Lines[no].Length, no);
                    debugger.fastColoredTextBox1.DoSelectionVisible();
                    debugger.fastColoredTextBox1.SelectionColor = System.Drawing.Color.Yellow;
                    if (delay)
                        System.Threading.Thread.Sleep(1000);
                }
            }
            catch
            {
                Program.WriteLine("ERROR: couldn't update lineno in debugger", ConsoleColor.Red);
            }
        }

        public static void Select(FastColoredTextBoxNS.Range range, System.Drawing.Color c)
        {
            try
            {
                if (enabled)
                {
                    debugger.fastColoredTextBox1.Selection = range;
                    debugger.fastColoredTextBox1.DoSelectionVisible();
                    debugger.fastColoredTextBox1.SelectionColor = c;
                    if (delay)
                    {
                        System.Threading.Thread.Sleep(500);
                        //debugger.fastColoredTextBox1.Selection = new FastColoredTextBoxNS.Range(debugger.fastColoredTextBox1, 0, 0, 0, 0);
                        //debugger.fastColoredTextBox1.SelectionColor = System.Drawing.Color.Yellow;
                    }
                }
            }
            catch
            {
                Program.WriteLine("ERROR: couldn't update sel in debugger", ConsoleColor.Red);
            }
        }
    }
}
