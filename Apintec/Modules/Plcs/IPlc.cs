using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs
{
    public interface IPlc : IHardware 
    {
        bool Write(byte[] buffer, int offset, int length, params object[] parameters);
        bool Read(ref byte[] buffer, int offset, int length, params object[] parameters);
    }
}
