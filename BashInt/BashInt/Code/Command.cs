using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BashInt.Code
{
    public class Command
    {
        public Interpreter i = null;
        public Command(Interpreter intp)
        {
            i = intp;
        }
        public void RunCommand(string loc)
        {
            Program.WriteLine("Running Command: " + loc, ConsoleColor.Green);

            string fpart = Regexes.part.Match(loc).Value;

            bool found = false;
            //ParseArgs(loc);
            if (fpart == "printf")
            {
                printf(i, loc, Program.SelectEscQuotes(loc));
                found = true;
            }
            else
            if (fpart == "echo")
            {
                echo(i, loc, Program.SelectEscQuotes(loc+"\""+Environment.NewLine+"\""));
                found = true;
            }
            else
            if (fpart == "sleep")
            {
                found = true;
                sleep(i, loc, ParseArgs(loc));

            }
            if (!found)
            {
                foreach (var func in i.flist)
                {
                    if (func.fname == fpart && !found)
                    {
                        i.RunCode(func.rawcode);
                        Debug.LoadFile(i.rawtext);
                        found = true;
                    }
                }
            }
            if (!found)
            {
                Program.WriteLine("Could Not Run Command: " + loc, ConsoleColor.Red);
            }
            Program.WriteLine("Done", ConsoleColor.Green);
        }
        public static List<string> ParseArgs(string l)
        {
            string loc = "";
            Console.WriteLine("\tParsing Args:");

            List<string> ret = new List<string>();
            Match fpart = Regexes.part.Match(l);
            loc = l.Remove(fpart.Index, fpart.Length);

            while (Regexes.part.IsMatch(loc))
            {
                try
                {
                    Match part = Regexes.part.Match(loc);
                    if (part.Value.StartsWith("\""))
                    {
                        Match ppart = Regexes.quotes.Match(loc);
                        string str = ppart.Value.Substring(1, ppart.Value.Length - 2);
                        ret.Add(str);
                        Program.WriteLine("\t" + str, ConsoleColor.Magenta);
                        loc = loc.Remove(ppart.Index, ppart.Length);
                    }
                    else
                    {
                        ret.Add(part.Value);
                        Program.WriteLine("\t" + part.Value, ConsoleColor.Magenta);
                        loc = loc.Remove(part.Index, part.Length);
                    }
                    //System.Threading.Thread.Sleep(1000);
                }
                catch
                {
                    Program.WriteLine("FATAL: Could not parse command args: " + loc, ConsoleColor.Red);
                    break;
                }
            }

            Console.WriteLine("\tOk!");
            return ret;
        }

        public static void printf(Interpreter i, string s, List<string> args)
        {
            Console.WriteLine("\tPrintf:");

            //List<BashColour> cols = new List<BashColour>();
            i.form.AppendText(args[0]);
            return;
            foreach (var arg in args)
            {
                string text = arg;
                //text.Replace(((char)27).ToString(), @"\e");//27 is the char value for the UNESCAPED \033
                //text.Replace(Regex.Unescape(@"\e"), @"\e");
                MatchCollection efuncts = Regexes.efuncts.Matches(text);

                int no = 0;
                while (no < text.Length)
                {
                    again:
                    int index = -1;
                    for (int j =0; j<efuncts.Count;j++)
                    {
                        if (efuncts[j].Index == no)
                        {
                            index = j;
                            continue;
                        }
                    }

                    if (index != -1)
                    {
                        DoEfunct(i, efuncts[index]);
                        no += efuncts[index].Length;
                        goto again;
                    }
                    else
                    {
                        //print this text
                        try
                        {
                            i.form.AppendText(text[no].ToString());
                        }
                        catch { }
                        no++;
                    }
                }
            }
            //MatchCollection efuncts = Regexes.efuncts.Matches(text);
            //foreach (Match efunct in efuncts)
            //{
            //    if (efunct.Value.StartsWith(@"\e[8;") && efunct.Value.EndsWith("t"))
            //    {
            //        Console.WriteLine("\tSize Setting Arg detected:");
            //        string[] textspl = efunct.Value.Split(';');
            //        int x = Int32.Parse(textspl[2].Remove(textspl[2].Length - 1, 1));
            //        int y = Int32.Parse(textspl[1]);
            //        Console.WriteLine("\t\t" + x);
            //        Console.WriteLine("\t\t" + y);
            //    }
            //}
        }

        public static void echo(Interpreter i, string s, List<string> args)
        {
            Console.WriteLine("\techo:");
            foreach (var arg in args)
            {
                if (arg != "-e")
                {
                    printf(i,s,new List<string>() { arg });
                }
            }
        }

        private static void DoEfunct(Interpreter i, Match efunct)
        {
            Program.WriteLine("Doing efunct: " + efunct.Value, ConsoleColor.Yellow);
            if (efunct.Value.StartsWith(@"\e[8;") && efunct.Value.EndsWith("t"))
            {
                Console.WriteLine("\tSize Setting Arg detected:");
                string[] textspl = efunct.Value.Split(';');
                int x = Int32.Parse(textspl[2].Remove(textspl[2].Length - 1, 1));
                int y = Int32.Parse(textspl[1]);
                Console.WriteLine("\t\t" + x);
                Console.WriteLine("\t\t" + y);
                //i.form.ChangeSize(x, y);
            }
        }

        private static string unesc(string s)
        {
            return 
                s.Replace(@"\033", @"\e")
                .Replace("\n", Environment.NewLine)
                .Replace(@"\'", "`")
                .Replace("\\\"", "\"")
                .Replace("\\0","\0")
                .Replace(@"\\", @"\");
        }

        public static void sleep(Interpreter i, string s, List<string> args)
        {
            int sleep = (int)(decimal.Parse("0"+args[0])*1000);
            System.Threading.Thread.Sleep(sleep);
            Program.WriteLine("Slept for: " + sleep + "ms ", ConsoleColor.Green);
        }
    }
}
