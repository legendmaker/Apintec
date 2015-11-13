using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Net
{
    public class XUdpClient : UdpClient, IXCom
    {
        public NetParameter Parameter { get; internal set; }

        private bool _isConnected = false;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler OnReceive;
        public event EventHandler OnSend;
        public XUdpClient(IPEndPoint ep):base(ep)
        {
        }
        public XUdpClient(NetParameter para) : this(new IPEndPoint(NetPoint.LocalHostPoint[para.LocalEthIndex].IPAddress, para.LocalPort))
        {
            Parameter = para;
        }
        public bool Connect()
        {
            if (_isConnected)
                return true;
            try
            {
                Connect(new IPEndPoint(IPAddress.Parse(Parameter.ServerIp), Parameter.ServerPort));
                _isConnected = true;
                return true;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        public bool Disconnect()
        {
            if (!_isConnected)
                return true;
            try
            {
                base.Close();
                _isConnected = false;
                return true;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        public int Receive(ref byte[] buffer, int offset, int count)
        {
            
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Parameter.ServerIp), Parameter.ServerPort);
            try
            {
                buffer = Receive(ref ep);
                RaiseOnReceiveEvent(buffer);
                return buffer.Length;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        private void RaiseOnReceiveEvent(byte[] data)
        {
            if(OnReceive!=null)
            {
                OnReceive(this, new XComEventArgs(data.Length, data));
            }
        }

        public bool Send(byte[] buffer, int offset, int count)
        {
            int sendByte = 0;
            try
            {
                while (sendByte!=count)
                {
                    sendByte += base.Send(buffer, count);
                }
                return true;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }
    }
}
