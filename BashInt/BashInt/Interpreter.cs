using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BashInt
{
    public class Interpreter
    {
        public Form1 form = null;
        public FileInfo file = null;
        public List<string> rawtext = null;

        public List<Code.Function> flist = new List<Code.Function>();
        public Interpreter(Form1 fo,FileInfo fi)
        {
            form = fo;
            file = fi;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Loading: " + fi.Name);
            fo.pbar.Style = ProgressBarStyle.Marquee;

            if (fi.Extension != ".sh")
            {
                if (MessageBox.Show("BashInt doesn't currently support this file type. Try to open anyway?", "File Extention Mismatch",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    Console.WriteLine("Abort!");
                    Program.Abort();
                }
            }
            ParseFile(fi.FullName);
            Debug.LoadFile(rawtext);
            new System.Threading.Thread(()=>
            this.Start()
            ).Start()
            ;
        }
        public void ParseFile(string path)
        {
            Console.Write("Reading file...");
            //using (StreamReader sr = new StreamReader(path))
            //{
            //    string line = sr.ReadLine();
            //    sr.
            //    if (line != null)
            //    {
            //        rawtext.Add(line);
            //        Console.WriteLine(line);
            //    }
            //}
            //rawtext = File.ReadAllLines(path).ToList();
            List<string> rtext = new List<string>();
            foreach (var item in File.ReadAllLines(path))
            {
                rtext.Add(item); //no .ToList(); method fallback
            }
            rawtext = rtext;
            Console.WriteLine("[OK!]");
        }

        public void Start()
        {
            Console.WriteLine("Interpreter Started!");
            int no = 0;
            #region Bin/Bash
            if (rawtext[0] != "#!/bin/bash")
            {
                Program.WriteLine("No bin/bash def", ConsoleColor.Yellow);
            }
            else
            {
                Program.WriteLine("Found bin/bash def", ConsoleColor.Green);
                no = 1;
            }
            #endregion

            flist = Code.Function.ExtractFunctions(rawtext);
            rawtext = Code.Function.StripFunctions(rawtext, flist);

            //Debug.LoadFile(rawtext);
            RunCode(rawtext,no);
            Debug.debugger.fastColoredTextBox1.Clear();
            //form.Clear();
        }

        public void RunCode(List<string> code, int startloc = 0)
        {
            form.pbar.Maximum = code.Count;
            Console.WriteLine("I think that " + code.Count + " = " + form.pbar.Maximum+" meeeeeeeehhhh");
            form.pbar.Style = ProgressBarStyle.Blocks;

            int no = startloc;
            Debug.LoadFile(code);
            while (no < code.Count)
            {
                string loc = code[no];
                Match m = Regexes.comment.Match(loc);
                MatchCollection qms = Regexes.quotes.Matches(loc);
                bool skip = false;
                foreach (Match qm in qms)
                {
                    if (qm.Value.Contains("#"))
                    {
                        skip = false;
                    }
                }
                if (!skip)
                {
                    loc = loc.Remove(m.Index, m.Length);
                }

                Program.WriteLine(loc, ConsoleColor.Cyan);
                if (loc.Trim() == "")
                {
                    Console.WriteLine("Skipped!");
                }
                else
                {
                    new Code.Command(this).RunCommand(loc);
                    Debug.LineNo(no);
                }
                try
                {
                    form.pbar.Value = no;
                }
                catch (Exception e)
                {
                    Program.WriteLine("FATAL: Could not set pbar value. Min = "+form.pbar.Minimum+" max = "+form.pbar.Maximum+Environment.NewLine+e, ConsoleColor.Red);
                }
                no++;
            }
        }
    }
}
