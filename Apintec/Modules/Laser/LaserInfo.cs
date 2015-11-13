using Apintec.Modules.Laser.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser
{
    public class LaserInfo : HardwareInfo
    {
        public virtual int Sequence { get; internal set; }
        public virtual string IPAddress { get; internal set; }
        public virtual int Port { get; internal set; }       
        public virtual LaserProtocolType ProtType { get; internal set; }
        public string ComKey { get; internal set; }
        public LaserInfo()
        {
        }
        public LaserInfo(string name, string vendor, string module, string deviceID,
            int seq, string ipAddr, int port, LaserProtocolType pt, string comKey) 
            : base(name, vendor, module, deviceID)
        {
            Sequence = seq;
            IPAddress = ipAddr;
            Port = port;
            ProtType = pt;
            ComKey = comKey;
        }
    }
}
