using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using BashInt.Data;
using System.Text.RegularExpressions;

namespace BashInt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Start(args);
            //foreach (var item in SelectEscQuotes(Console.ReadLine()))Program.WriteLine(item, ConsoleColor.Cyan);
        }
        public static void Start(string[] args)
        {
            BashColour.serialize(true);
            Debug.debugger = new Debugger();
            Debug.debugger.Show();

            Form1 fo = new Form1();
            foreach (var arg in args)
            {
                Console.WriteLine("ARG:\t" + arg);
            }
            try
            {
                if (args[0] != null)
                {
                    Console.WriteLine("Path Arg was passed!");
                    FileInfo f = new FileInfo(args[0]);
                    Console.WriteLine("Sucessfully Parsed path!");
                    fo = new Form1(false);
                    new Interpreter(fo, f);//.Start();//OLOPOO
                }
            }
            catch { Console.WriteLine("No Path Arg was passed"); }

            Application.Run(fo);
        }
        public static void Abort()
        {
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.ProcessName == "BashInt")
                {
                    proc.Kill();
                }
            }
        }
        public static void WriteLine(string s, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(s);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Write(string s, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.Write(s);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static List<string> SelectEscQuotesOld(string s)
        {
            int no = 0;
            bool selecting = false;
            string curbuf = "";
            List<string> ret = new List<string>();
            while (no < s.Length)
            {
                char c = '\0';
                try
                {
                    c = s[no - 1];
                }
                catch { }

                if (s[no] == '"')
                {
                    if (!selecting)
                    {
                        selecting = true;
                        if (no + 1 < s.Length)
                        {
                            no++;
                        }
                    }
                    else
                    {
                        if (c == '\\')
                        {
                            curbuf = curbuf.Remove(curbuf.Length - 1, 1);//rem last char (which was a \)
                                                                         // s = s.Remove(s.Length - 1, 1);
                            s = s;
                            curbuf += '"';
                        }
                        else
                        {
                            selecting = false;
                            ret.Add(curbuf);
                            curbuf = "";
                            Program.WriteLine(curbuf, ConsoleColor.Yellow);
                        }
                    }
                }
                if (selecting)
                {
                    if (s[no] == '0' && c == '\\')
                    {
                        // if (c == '\\')
                        {
                            curbuf = curbuf.Remove(curbuf.Length - 1, 1);
                            curbuf += '\0';
                        }
                    }
                    else
                    if (s[no] == '\\' && c == '\\')
                    {
                        // if (c == '\\')
                        {
                            curbuf = curbuf.Remove(curbuf.Length - 1, 1);
                            curbuf += '\\';
                        }
                    }


                    else
                    {
                        curbuf += s[no];
                    }
                }
                no++;
            }

            return ret;
        }
        public static List<string> SelectEscQuotes(string s)
        {
            List<string> ret = new List<string>();
            List<EscChar> l = new List<EscChar>();
            string remmed = s;
            while (remmed.Contains("\\\""))
            {
                int i = remmed.IndexOf("\\\"");
                l.Add(new EscChar("\\\"", "\"", i-0));
                remmed = remmed.Remove(i-0, 2);
            }

            MatchCollection ms = Regexes.quotes.Matches(remmed);
            foreach (Match m in ms)
            {
                string parsed = "";
                for (int i = 0; i <= m.Length; i++)
                {
                    foreach (var item in l)
                    {
                        if (item.index == i + m.Index)
                        {
                            parsed += item.unesced;
                        }
                    }
                    try
                    {
                        parsed += m.Value[i];
                    }
                    catch { }
                }
                parsed = parsed.Substring(1, parsed.Length - 2);
                ret.Add(Regex.Unescape(parsed));
            }
            return ret;
        }
    }
}
