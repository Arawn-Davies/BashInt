using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace BashInt
{
    public partial class Form1 : Form
    {
        public Form1(bool showFS=true)
        {
            InitializeComponent();
            if (!showFS)
            {
                FSPanel.Hide();
            }
           // ChangeSize(80, 30);
        }

        private void Display_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("Drag Enter!");
            FileInfo f = new FileInfo(((string[])(e.Data.GetData(DataFormats.FileDrop)))[0]);
            Console.WriteLine("\tFile Parsed!");
            FSPanel.Hide();
            new Interpreter(this, f);
        }

        private void Display_MouseUp(object sender, MouseEventArgs e)
        {
            // Console.WriteLine("MUP");
            // this.AppendText("><");
            // this.NewLine();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Abort();
        }

        //public List<Data.Cell> buffer = new List<Data.Cell>();
        public List<BashColour> cols = new List<BashColour>();
        public int curback = 0;
        public int curfront = 7;

        //public int curcol = 0;
        //public int currow = 0;

        //public float cellwidth = 10;
        //public float cellheight = 10;

        //public float fontsize = 10;

        //public int rowskip = 0;

        //private void Display_Paint(object sender, PaintEventArgs e)
        //{
        //    for (int i = rowskip; i < buffer.Count; i++)
        //    {
        //        buffer[i].font = new Font(buffer[i].font.FontFamily, fontsize, buffer[i].font.Style);
        //        buffer[i].Draw(e.Graphics);
        //       // Console.WriteLine("Drew: " + cell.ToString());
        //    }
        //}

        //public void ShiftBufferUp()
        //{
        //    foreach (var cell in buffer)
        //    {
        //        cell.rect.Y -= cellheight;
        //    }
        //    //buffer.RemoveRange(0, (int)(Display.Width / cellwidth));
        //}

        //public void AppendText(string s)
        //{
        //    foreach(var c in s)
        //    {
        //        Data.Cell newcell = new Data.Cell(
        //            new RectangleF(curcol*cellwidth,currow*cellheight,cellwidth,cellheight),
        //            c,BashColour.list[curfront],BashColour.list[curback]
        //            );
        //        curcol++;
        //        if (curcol * cellwidth > Display.Width-cellwidth)
        //        {
        //            curcol = 0;
        //            currow++;
        //            if (currow * cellheight > Display.Height-(cellheight*1))
        //            {
        //                rowskip++;
        //                ShiftBufferUp();
        //                currow--;
        //                curcol = 0;
        //               // buffer.Clear();
        //            }
        //        }
        //        buffer.Add(newcell);
        //    }
        //    Display.Refresh();
        //   // Display.Update();
        //}
        //public void NewLine()
        //{
        //    int no = (int)(Display.Width / cellwidth)-curcol;
        //    while (no > 0)
        //    {
        //        AppendText(" ");
        //        no--;
        //    }
        //}
        //public void Clear()
        //{
        //    this.buffer.Clear();
        //    this.Display.Refresh();
        //    curcol = 0;
        //    currow = 0;
        //}

        //public void ChangeSize(int cols, int rows)
        //{
        //    while ((int)(Display.Width / cellwidth) != cols && (int)(Display.Height / cellheight) != rows)
        //    {
        //        float newwidth = this.Size.Width;
        //        float newheight = this.Size.Height;

        //        if ((int)(Display.Width / cellwidth) > cols)
        //        {
        //            newwidth  -= cellwidth;
        //        }
        //        else if ((int)(Display.Width / cellwidth) < cols)
        //        {
        //            newwidth += cellwidth;
        //        }

        //        if ((int)(Display.Height / cellheight) > rows)
        //        {
        //            newheight -= cellheight;
        //        }
        //        else if ((int)(Display.Height / cellheight) < rows)
        //        {
        //            newheight += cellheight;
        //        }

        //        this.Size = new Size((int)newwidth, (int)newheight);
        //    }
        //    Console.WriteLine("Size Changed to: {0},{1}", cols, rows);
        //}

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            //Console.WriteLine("Display Size: " + (int)(Display.Width / cellwidth) + "," + (int)(Display.Height / cellheight));
            // Console.WriteLine(richTextBox1.Font.Size + ", " + richTextBox1.Font.SizeInPoints);

            //float dp = richTextBox1.Font.Size * richTextBox1.Font.FontFamily.GetCellDescent(FontStyle.Regular) / richTextBox1.Font.FontFamily.GetEmHeight(FontStyle.Regular);
            //Console.WriteLine("Dp: " + dp);
            //float num = richTextBox1.Size.Width / dp;
            //Console.WriteLine("= " + num);
          //  Display.AppendText("test");
         //   cols.Add(new BashColour(2, 2, true));
          //  SetCols();

            Size s = TextRenderer.MeasureText("a", Display.Font);
            //Console.WriteLine(s);
            decimal row = Display.Size.Width / s.Width*2;
      //      Console.WriteLine("1 row = " + row);
            decimal height = Display.Size.Height / s.Height;
            // Console.WriteLine("1 column = " + height);
            Console.WriteLine("Display Size: {0},{1}", row, height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Size s = TextRenderer.MeasureText("a", Display.Font);
            this.Size = new Size((int)((int)(this.Size.Width/ s.Width) * s.Width),
                (int)((int)(this.Size.Height / s.Height) * s.Height));
           // Console.WriteLine(this.Size);
        }


        public void SettingsChanged(object sender, EventArgs e)
        {
            //cellwidth = (float)numericUpDown1.Value;
            //cellheight = (float)numericUpDown2.Value;
            //fontsize = (float)numericUpDown3.Value;
            Display.Font = new Font("Consolas", (float)numericUpDown3.Value, FontStyle.Regular);
            //Display.Refresh();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Console.WriteLine(Display.TextLength);
            SetCols();
        }
        public void AppendText(string txt)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                if ((int)txt[i] == 27)//27 = \033 unescaped using Regex.Unescape();
                {
                   // Program.WriteLine("LOOOO:" + txt, ConsoleColor.Cyan);
                    string func = "";
                    while (func == "")
                    {
                        for (int j = i+1; j < txt.Length; j++)
                        {
                           // Program.WriteLine(j + ": " + txt[j], ConsoleColor.Green);
                            func += txt[j];
                            if (Char.IsLetter(txt[j])) break;
                        }
                    }
                    txt = txt.Remove(i, func.Length+1);//+1 for the \e we skipped earlier
                    Program.WriteLine("Func parsed: " + func, ConsoleColor.Cyan);
                    DoEfunct(func);
                   // i += func.Length;
               // }
                    //THESE PIECES OF CODE BOTH DONT WORK!! WHYYYY ^v A: Because the /e was unescaped and for some reason not allowing the string to be printed to the console! 
                    //FIXED THIS BUG BY MISSING OUT THE /e: I.E STARTING j at i+1 instead of i in the for loop
                    //int j = i;
                    //for (j = j; j < txt.Length; j++)
                    //{
                    //    Program.WriteLine(txt[j].ToString(), ConsoleColor.Magenta);
                    //    if (Char.IsLetter(txt[j]))
                    //    {
                    //        break;
                    //    }
                    //}
                    //Console.WriteLine(txt.Substring(i, j - i));
                }
            }
            Display.AppendText(txt);
            SetCols();
        }
        public void DoEfunct(string efunct)
        {
            if (efunct.StartsWith(@"[8;") && efunct.EndsWith("t"))
            {
                Console.WriteLine("\tSize Setting Arg detected:");
                string[] textspl = efunct.Split(';');
                int x = Int32.Parse(textspl[2].Remove(textspl[2].Length - 1, 1));
                int y = Int32.Parse(textspl[1]);
                Console.WriteLine("\t\t" + x);
                Console.WriteLine("\t\t" + y);
                SetSize(x, y);

            }
        }
        public void SetSize(int w, int h)
        {
            Size s = TextRenderer.MeasureText("a", Display.Font);
            decimal curw = Display.Size.Width / s.Width;//* 2;
            decimal curh = Display.Size.Height / s.Height;

            while (curw != w && curh != h)
            {
                int aw = 0;
                int ah = 0;
                if (curw != w)
                {
                    if (curw > w)
                    {
                        aw--;
                    }
                    else if(curw < w)
                    {
                        aw++;
                    }
                }
                if (curh != h)
                {
                    if (curh > h)
                    {
                        ah--;
                    }
                    else if (curh < h)
                    {
                        ah++;
                    }
                }
                this.Size = new Size(this.Size.Width + aw, this.Size.Height + ah);
                s = TextRenderer.MeasureText("a", Display.Font);
                curw = Display.Size.Width / s.Width;//*2;
                curh = Display.Size.Height / s.Height;
            }
        }
        public void SetCols()
        {
            if (checkBox2.Checked)
            {
              //  pbar.Value = 0;
              //  pbar.Maximum = cols.Count;
                SortCols();
                int sels = Display.SelectionStart;
                int sell = Display.SelectionLength;
                for (int i = 0; i != cols.Count; i++)
                {
                    try
                    {
                        Display.SelectionStart = cols[i].charno;
                        Display.SelectionLength = Display.Text.Length - cols[i].charno;
                        if (!cols[i].background)
                        {
                            Display.SelectionColor = cols[i].ToColor();
                        }
                        else
                        {
                            Display.SelectionBackColor = cols[i].ToColor();
                        }
                    }
                    catch { }
                    //pbar.Increment(1);
                }
                Display.SelectionStart = sels;
                Display.SelectionLength = sell;
               // pbar.Value = 0;
            }
        }
        public void SortCols()
        {
            System.Collections.Generic.List<BashColour> ret = cols;
            while (!isSorted(ret))
            {
                int last = 0;
                for (int no = 0; no < ret.Count; no++)
                {
                    try
                    {
                        if (ret[no].charno - last < 0 || (ret[no].charno - last == 0 && ret[no].code < ret[no - 1].code))
                        {
                            //move this back (swap)
                            var temp = ret[no - 1];
                            ret[no - 1] = ret[no];
                            ret[no] = temp;
                        }
                    }
                    catch { }
                    last = ret[no].charno;
                }
            }
            cols = ret;
        }
        public bool isSorted(List<BashColour> l)
        {
            int no = 0;
            foreach (var item in l)
            {
                int dif = item.charno - no;
                if (dif < 0)
                {
                    return false;
                }
                no = item.charno;
            }
            return true;
        }

        private void FSPanel_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void FSPanel_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("Drag Enter!");
            FileInfo f = new FileInfo(((string[])(e.Data.GetData(DataFormats.FileDrop)))[0]);
            Console.WriteLine("\tFile Parsed!");
            FSPanel.Hide();
            new Interpreter(this, f);//.Start();//OLOPOO
        }
    }
}
