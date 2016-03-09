using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BashInt.Data
{
    public class Cell
    {
        public RectangleF rect = new RectangleF();
        public Color background = BashColour.list[0];
        public Color foreground = BashColour.list[7];
        private Brush backbrush = new SolidBrush(BashColour.list[0]);
        private Brush forebrush = new SolidBrush(BashColour.list[7]);

        public char c = ' ';

        public Font font = new Font("Consolas", 16, FontStyle.Regular);

        public Cell()
        {
        }
        public Cell(RectangleF r, char character, Color f, Color b)
        {
            c = character;
            rect = r;
            background = b;
            foreground = f;
            backbrush = new SolidBrush(b);
            forebrush = new SolidBrush(f);
        }
        public Cell(RectangleF r, char character, BashColour f, BashColour b,string efunc=null)
        {
            c = character;
            rect = r;
            background = b.ToColor();
            foreground = f.ToColor();
            backbrush = new SolidBrush(background);
            forebrush = new SolidBrush(foreground);
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(backbrush, rect);
            g.DrawString(c.ToString(), font, forebrush, new PointF(rect.X, rect.Y));
        }

        public override string ToString()
        {
            return "{" + rect.X + "," + rect.Y + "}";
        }
    }
}
