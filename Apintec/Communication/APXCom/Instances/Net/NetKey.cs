using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Net
{
    public static class NetKey
    {
        public static string BuildNetKey(string key, NetParameter para)
        {
            if ((key != null) && (para != null))
                return key + ":" + para.ServerIp + ":" + para.ServerPort;
            else
                return null;
        }
    }
}
