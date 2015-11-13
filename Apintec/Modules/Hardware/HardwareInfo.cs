using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules
{
    public class HardwareInfo
    {
        public virtual string Name { get;  set; }
        public virtual string Vendor { get;  set; }
        public virtual string Module { get;  set; }
        public virtual string DeviceID { get;  set; }
        public virtual bool IsOpen { get;  set; }
        public HardwareInfo()
        {
            IsOpen = false;
        }
        public HardwareInfo(string name, string vendor, string module, string deviceID):this()
        {
            Name = name;
            Vendor = vendor;
            Module = module;
            DeviceID = deviceID;
        }
    }
}
