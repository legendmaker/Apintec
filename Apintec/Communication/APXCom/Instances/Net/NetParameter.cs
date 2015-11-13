using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Net
{
    public class NetParameter
    {
        public string ServerIp { get; internal set; }
        public int ServerPort { get; internal set; }
        public int LocalEthIndex { get;internal set; }
        public int LocalPort { get; internal set; }
        public int Timeout { get; internal set; }
        public NetParameter()
        {

        }
        public NetParameter(string serverIP, int serverPort):this()
        {
            ServerIp = serverIP;
            ServerPort = serverPort;
        }
        public NetParameter(string serverIP, int serverPort, int timeout) : this(serverIP, serverPort) 
        {
            Timeout = timeout;
        }
        public NetParameter(string serverIP, int serverPort, int timeout, int localEthIndex, int localPort)
            :this(serverIP,serverPort,timeout)
        {
            LocalEthIndex = localEthIndex;
            LocalPort = localPort;
        }
    }
}
