using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules
{
    public class HardwareInfo
    {
        public virtual string Name { get; protected set; }
        public virtual string Vendor { get; protected set; }
        public virtual string Module { get; protected set; }
        public virtual string DeviceID { get; protected set; }
        public virtual bool IsOpen { get; protected set; }
    }
}
