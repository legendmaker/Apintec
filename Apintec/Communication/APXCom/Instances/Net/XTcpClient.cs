using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Net
{
    public class XTcpClient : TcpClient, IXCom
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

        private ManualResetEvent ConnectDone = new ManualResetEvent(false);
        private ManualResetEvent SendDone = new ManualResetEvent(false);
        private ManualResetEvent RecvDone = new ManualResetEvent(false);
        private NetworkStream _streamToServer;
        private Thread _recvThread;
        private byte[] _recvBuff = new byte[1024];

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler OnReceive;
        public event EventHandler OnSend;

        public XTcpClient(NetParameter para):base()
        {
            Parameter = para;
        }

        public bool Connect()
        {
            try
            {
                ConnectDone.Reset();
                SendDone.Reset();
                RecvDone.Reset();

                BeginConnect(Parameter.ServerIp, Parameter.ServerPort, ConnetEventHandler, this);
                bool bRet = ConnectDone.WaitOne(Parameter.Timeout);
                if (!bRet)
                    throw new APXExeception("Connection Timeout.");
            }
            catch (SocketException e)
            {
                throw new APXExeception("Connection Fail.");
            }
            if (!IsConnected)
                return false;
            _streamToServer = GetStream();
            return true;
        }

        private void ConnetEventHandler(IAsyncResult ar)
        {
            TcpClient tcpClient = ar.AsyncState as TcpClient;
            try
            {
                tcpClient.EndConnect(ar);
                _recvThread = new Thread(RecvProc);
                _recvThread.Start();
                RaiseOnConnectEvent();
                _isConnected = true;
            }
            catch (SocketException e)
            {
                APXlog.Write(APXlog.BuildLogMsg(e.Message));
            }
            finally
            {
                ConnectDone.Set();
            }
        }

        private void RecvProc()
        {
            while (true)
            {
                if(_streamToServer==null)
                {
                    continue;
                }
                if (_streamToServer.CanRead)
                {

                    _streamToServer.BeginRead(_recvBuff, 0, _recvBuff.Length, ReceiveEventHandler, _streamToServer);
                }
                else
                {
                    continue;
                }
                Thread.Sleep(200);

            }
        }

        private void ReceiveEventHandler(IAsyncResult ar)
        {
            try
            {
                int byteRead;
                if (!_streamToServer.CanRead)
                    return;
                byteRead = _streamToServer.EndRead(ar);

                if (byteRead > 0 && OnReceive != null)
                {
                    OnReceive(this, new XComEventArgs(byteRead));
                    Array.Clear(_recvBuff, 0, _recvBuff.Length);
                }

                if (_streamToServer.DataAvailable)
                {
                    _streamToServer.BeginRead(_recvBuff, 0, _recvBuff.Length, new AsyncCallback(ReceiveEventHandler), _streamToServer);
                }
            }
            catch(Exception e)
            {
                throw new APXExeception(e.ToString());
            }
        }

        private void RaiseOnConnectEvent()
        {
            if(OnConnect!=null)
            {
                OnConnect(this, new EventArgs());
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (_recvThread != null)
                    _recvThread.Abort();
                if (_streamToServer != null)
                    _streamToServer.Close();
                if (this != null)
                    this.Close();
                RaiseOnDisconnectEvent();
                _isConnected = false;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.ToString());
            }
            return true;
        }

        private void RaiseOnDisconnectEvent()
        {
            if(OnDisconnect!=null)
            {
                OnDisconnect(this, new EventArgs());
            }
        }

        public int Receive(ref byte[] buffer, int offset, int count)
        {
            try
            {
                buffer = new byte[count];
                Array.Copy(_recvBuff, offset, buffer, 0, count);
                return count;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        public bool Send(byte[] buffer, int offset, int count)
        {
            try
            {
                _streamToServer.BeginWrite(buffer, offset, count, SendEventHandler, this);
            }
            catch (SocketException)
            {
                throw new APXExeception("Tcp send Fail.");
            }
            return true;
        }

        private void SendEventHandler(IAsyncResult ar)
        {
            try
            {
                _streamToServer.EndWrite(ar);
                RaiseOnSendEvent();
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
        }

        private void RaiseOnSendEvent()
        {
            if(OnSend!=null)
            {
                OnSend(this, new EventArgs());
            }
        }
    }
}
