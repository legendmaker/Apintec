using Apintec.Modules.Plcs.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs
{
    public class PlcInstanceInfo
    {
        public PlcInfo Info { get; internal set; }
        public Type Type { get; internal set; }
        public Plc Instance()
        {
            return (Activator.CreateInstance(Type, Info)) as Plc;
        }
        public PlcInstanceInfo()
        {
        }
        public PlcInstanceInfo(PlcInfo info, Type type)
        {
            Info = info;
            Type = type;
        }
        
    }
}
