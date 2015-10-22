using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.IOCards
{
    public class IODesc
    {
        public PortType IOType { get; protected set; }
        public int Bit { get; protected set; }
        public string Function { get; protected set; }

        public IODesc(PortType portType, int bit, string function)
        {
            IOType = portType;
            Bit = bit;
            Function = function;
        }
    }
}
