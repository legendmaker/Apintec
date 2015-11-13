using Apintec.Communiction.APXCom;
using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Modules.Plcs.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs
{
    public class PlcInfo : HardwareInfo 
    {
        public virtual int Sequence { get; internal set; }
        public virtual string IPAddress { get; internal set; }
        public virtual int Port { get; internal set; }
        public virtual byte Network { get; internal set; }
        public virtual byte Unit { get; internal set; }
        public virtual byte Node { get; internal set; }
        public virtual PlcProtocolType ProtType { get; internal set; }
        public virtual int Timeout { get; internal set; }
        public virtual int LocalHostEthIndex { get; set; }
        public string  ComKey { get; internal set; }
        public PlcInfo():base()
        {
        }
        public PlcInfo(string name, string vendor, string module, string deviceID,
            int seq, string ip, int port, byte network, byte unit, 
            byte node, PlcProtocolType pt, int timeout, int ethIndex, string comKey)
            : base(name, vendor, module, deviceID)
        {
            Sequence = seq;
            IPAddress = ip;
            Port = port;
            Network = network;
            Unit = unit;
            Node = node;
            ProtType = pt;
            Timeout = timeout;
            LocalHostEthIndex = ethIndex;
            ComKey = comKey;
           
        }
    }
}
