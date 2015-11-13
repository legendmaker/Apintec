using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols
{
    public interface IPlcProtocol
    {
        bool Start();
        bool Stop();
        bool SendMessage(PlcCommand cmd, byte[] content, ref byte[] frame);
        object ParseResult(byte[] frame, int timeOut, params object[] para);
    }
}
