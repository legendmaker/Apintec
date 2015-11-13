using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Net
{
    public class NetPoint
    {
        private Int32 _netInterface = 0;
        private IPAddress _ipAddress;
        private int _netPort;
        private IPEndPoint _netIpEndPoint;
        private static List<NetPoint> _localHostPoint = new List<NetPoint>();

        static NetPoint()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            foreach (IPAddress ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Int32 i = 0;
                    _localHostPoint.Add(new NetPoint(i, ip, 0));
                }
            }
        }

        public IPEndPoint NetIpEndPoint
        {
            get
            {
                return _netIpEndPoint;
            }
        }

        public int NetPort
        {
            get
            {
                return _netPort;
            }

            set
            {
                _netPort = value;
                _netIpEndPoint = new IPEndPoint(_ipAddress, _netPort);
            }
        }

        public IPAddress IPAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {

                _ipAddress = value;
                _netIpEndPoint = new IPEndPoint(_ipAddress, _netPort);
            }
        }

        public static List<NetPoint> LocalHostPoint
        {
            get
            {
                return _localHostPoint;
            }
        }

        private void Initialize(IPAddress ipAddr, int netPort)
        {
            IPAddress = ipAddr;
            NetPort = netPort;
            _netIpEndPoint = new IPEndPoint(IPAddress, NetPort);
        }

        public NetPoint()
        {
            Initialize(IPAddress.Parse("127.0.0.1"), 10000);
        }

        public NetPoint(IPAddress ipAddr, int netPort)
        {
            Initialize(ipAddr, netPort);
        }

        public NetPoint(Int32 netInterface, IPAddress ipAddr, int netPort)
        {
            _netInterface = netInterface;
            Initialize(ipAddr, netPort);
        }
        public NetPoint(string ipAddr, int netPort)
        {
            Initialize(IPAddress.Parse(ipAddr), netPort);
        }
    }
}
