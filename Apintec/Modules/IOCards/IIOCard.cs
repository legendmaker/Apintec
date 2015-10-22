using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.IOCards
{
    interface IIOCard: IHardware
    {
        bool Read(PortType portType, UInt32 port, out BitArray bitArray);
        bool Write(PortType portType, UInt32 port, BitArray bitArray);

        event EventHandler OnRead;
        event EventHandler OnWrite;
    }
}
