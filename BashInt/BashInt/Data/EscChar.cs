using System;
using System.Collections.Generic;
using System.Text;

namespace BashInt.Data
{
    public class EscChar
    {
        public string esced = "";
        public string unesced = "";
        public int index = 0;
        public EscChar(string _esced, string _unesced, int _index)
        {
            esced = _esced;
            unesced = _unesced;
            index = _index;
        }
    }
}
