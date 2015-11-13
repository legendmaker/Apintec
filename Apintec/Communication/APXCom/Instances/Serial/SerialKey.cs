using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Serial
{
    public static class SerialKey
    {
        public static string BuildSerialKey(string key, XSerialParameter para)
        {
            if (key != null && para != null)
                return key + ":" + para.PortName;
            else
                return null;
        }
    }
}
