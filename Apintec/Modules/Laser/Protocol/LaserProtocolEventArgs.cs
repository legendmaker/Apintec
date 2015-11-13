using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser.Protocol
{
    public class LaserProtocolEventArgs : EventArgs
    {
        public byte[] Message { get; protected set; }
        public LaserProtocolEventArgs(byte[] message) : base()
        {
            Message = message;
        }
    }
}
