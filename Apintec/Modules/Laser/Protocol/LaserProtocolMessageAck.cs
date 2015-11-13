using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser.Protocol
{
    public enum LaserProtocolMessageAck
    {
        None = 0xFF,
        Initial = 0x00,
        ReceiveConfirm = 0x01,
        CommandSucceed = 0x02,
        CommandFail = 0x03
    }
}
