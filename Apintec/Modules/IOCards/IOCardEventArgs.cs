using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.IOCards
{
    public class IOCardEventArgs:EventArgs
    {
        public uint Port { get; set; }
        public BitArray Value { get; set; }
        public bool IsOK { get; set; }
        public IOCardEventArgs(uint port, BitArray value, bool isOK)
        {
            Port = port;
            Value = value;
            IsOK = isOK;
        }
    }
}
