using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BashInt
{
    public class Regexes
    {
        public static Regex comment = new Regex("#.+");
        public static Regex fstart = new Regex(@"function.+(\n|)+{");
        public static Regex part = new Regex(@"[^\s]+");
        public static Regex quotes = new Regex("\"[^\"]*\"",RegexOptions.Multiline);

        public static Regex efuncts = new Regex(@"(\\033|\\e)(\[\d+(;\d+;\d+|)|)(m|t|c)");
        public static Regex litefuncts = new Regex(@"(\033|\e)(\[\d+(;\d+;\d+|)|)(m|t|c)");

    }
}
