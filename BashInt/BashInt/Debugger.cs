using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BashInt
{
    public partial class Debugger : Form
    {
        #region Cols
        private TextStyle BrownStyle = new TextStyle(System.Drawing.Brushes.Brown, null, System.Drawing.FontStyle.Italic);
        private TextStyle GreenStyle = new TextStyle(System.Drawing.Brushes.Green, null, System.Drawing.FontStyle.Italic);
        private TextStyle MagentaStyle = new TextStyle(System.Drawing.Brushes.Magenta, null, System.Drawing.FontStyle.Regular);
        private TextStyle BlueStyle = new TextStyle(System.Drawing.Brushes.Cyan, null, System.Drawing.FontStyle.Regular); 
        #endregion
        public Debugger()
        {
            InitializeComponent();
        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            e.ChangedRange.SetStyle(this.GreenStyle, Regexes.comment);
            e.ChangedRange.SetStyle(this.BrownStyle,Regexes.quotes);
            e.ChangedRange.SetStyle(this.MagentaStyle, "\\d+");
            e.ChangedRange.SetStyle(this.BlueStyle, "function|clear|if|then|else|fi|sleep|true|false|do|done|while|echo|for|printf");
        }
    }
}
