using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BashInt.Code
{
    public class Function
    {
        public string fname = "";
        public int locstart = 0;
        public int locs = 0;
        public List<string> rawcode = new List<string>();

        public Function(string fn, List<string> code)
        {
            fname = fn;
            rawcode = code;
        }

        public static List<Function> ExtractFunctions(List<string> rawcode)
        {
            Debug.LoadFile(rawcode);
            Program.WriteLine("Extracting Functions", ConsoleColor.Green);
            List<Function> ret = new List<Function>();
            int no = 0;
            bool selecting = false;
            Function singf = null;
            while (no < rawcode.Count)
            {
                string loc = rawcode[no];
                string loctrim = loc.Trim();
                if (loc.Trim().StartsWith("function"))
                {
                    //Program.WriteLine(loc.Trim(), ConsoleColor.Magenta);
                    string fname = "";
                    for (int i = 9; i < loctrim.Length; i++)
                    {
                        if (loctrim[i] != ' ')
                        {
                            fname += loctrim[i];
                        }
                        else
                        {
                            i = loctrim.Length;
                        }
                    }
                   // Console.WriteLine("Name: " + fname);
                    singf = new Function(fname, new List<string>());
                    singf.locstart = no;
                    selecting = true;
                }

                if (selecting)
                {
                    if (!loctrim.StartsWith("}"))
                    {
                        singf.rawcode.Add(loc);
                    }
                    else
                    {
                        selecting = false;
                        string s = string.Join(Environment.NewLine, singf.rawcode.ToArray());
                        Match m = Regexes.fstart.Match(s);
                        List<string> rcode = new List<string>();
                        foreach (var item in (s.Remove(m.Index, m.Length)).Split(new string[] { Environment.NewLine }, int.MaxValue, StringSplitOptions.None))
                        {
                            rcode.Add(item); //older versions of .Net Framework doesn't have the string[].toList(); method so doing it manually...
                        }
                        singf.rawcode = rcode;
                       // Program.WriteLine(string.Join(Environment.NewLine, singf.rawcode), ConsoleColor.Yellow);
                        //Program.WriteLine("}", ConsoleColor.Magenta);
                        singf.locs = no - singf.locstart;
                        ret.Add(singf);

                        Debug.Select(new FastColoredTextBoxNS.Range
                            (Debug.debugger.fastColoredTextBox1, 0, singf.locstart, loc.Length, no),
                            System.Drawing.Color.Purple);
                    }
                }
                else
                {
                    Debug.delay = false;
                    Debug.LineNo(no);
                    Debug.delay = true;
                }
                no++;
            }
            return ret;
        }
        public static List<string> StripFunctions(List<string> text, List<Function> funcs)
        {
            Program.WriteLine("Stripping functions", ConsoleColor.Green);
            List<string> ret = text;
            int offset = 0;
            foreach (var func in funcs)
            {
                ret.RemoveRange(func.locstart-offset, func.locs+1);
                Console.WriteLine("Stripped function: " + func.fname + " w/locno: " + func.locs);
                //ret.RemoveAt(func.locstart);
                offset += func.locs+1;
            }
            Program.WriteLine("Done", ConsoleColor.Green);
            return ret;
        }
    }
}
