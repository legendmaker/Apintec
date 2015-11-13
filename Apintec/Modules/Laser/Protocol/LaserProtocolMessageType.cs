using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser.Protocol
{
    public enum LaserProtocolMessageType
    {
        None = 0xFF,
        Data = 0x00,
        Command = 0x01
    }
}
