using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apintec.Core.APCoreLib;
using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Communiction.APXCom;
using System.Threading;

namespace Apintec.views.CognexControlLibrary
{
    public partial class CognexPanel : UserControl
    {
        private IXCom _comm;
        public bool IsStart { get; internal set; }
        byte[] _recvBuff = new byte[0];
        public event EventHandler OnMessageReceive;
        public CognexPanel()
        {
            InitializeComponent();
            IsStart = false;
            new CognexManager();
            cvsInSightDisplay.ConnectedChanged += CvsInSightDisplay_ConnectedChanged;
        }

        private void CvsInSightDisplay_ConnectedChanged(object sender, EventArgs e)
        {
            if (cvsInSightDisplay.Connected)
                IsStart = true;
            else
                IsStart = false;
        }

        public void Start(int devIndex)
        {
            string key = "";
            if (IsStart)
                return;
            try
            {
                CognexClientInfo info = CognexManager.CognexCilentDict[devIndex];
                if(info.ProtocoType=="TCP/IP")
                {
                    key = NetKey.BuildNetKey(info.ComKey, new NetParameter(info.IPAddress, info.Port));
                }
                _comm = XComManager.XComsDict[key].Instance();
                cvsInSightDisplay.Connect(info.IPAddress, info.UserName, info.PassWord, true);
                LogOn(info);
               
                
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        private void LogOn(CognexClientInfo info)
        {
            if (info == null)
                return;
            try
            {
                if (_comm != null)
                    _comm.Connect();
                if(!_comm.IsConnected)
                {
                    throw new APXExeception(String.Format("Connect Cognex {0}:{1} fail.",
                        info.IPAddress, info.Port));
                }
                _comm.OnReceive += _comm_OnReceive;
                Send(info.UserName);
                Thread.Sleep(100);
                Send(info.PassWord);
            }
            catch(Exception e)
            {
                throw new APXExeception("Cognex camera " + e.Message);
            }
        }
        public void Send(string message)
        {
            byte[] content = Encoding.ASCII.GetBytes(message + "\r\n");
            try
            {
                _comm.Send(content, 0, content.Length);
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }
        private void _comm_OnReceive(object sender, EventArgs e)
        {
            var args = e as XComEventArgs;
            if (args == null)
                return;
            byte[] recv = new byte[args.ByteToRead];
            Array.Clear(recv, 0, recv.Length);
            try
            {
                _comm.Receive(ref recv, 0, args.ByteToRead);
            }
            catch (Exception)
            {
                //throw new APXExeception(e.Message);
            }
            lock (_recvBuff)
            {
                Gadget.ArrayAppend(_recvBuff, recv);
                RaiseOnMessageReceiveEvent(recv);
            }
        }

        private void RaiseOnMessageReceiveEvent(byte[] recv)
        {
            if(OnMessageReceive!=null)
            {
                OnMessageReceive(this, new CognexMessageArgs(recv));
            }
        }
        public void ShowImage()
        {
            cvsInSightDisplay.ShowGrid = false;
        }
        public void ShowGrid()
        {
            cvsInSightDisplay.ShowGrid = true;
        }
        public void Stop()
        {
            if (!IsStart)
                return;
            try
            {
                cvsInSightDisplay.Disconnect();
                if (_comm != null)
                {
                    _comm.OnReceive -= _comm_OnReceive;
                    _comm.Disconnect();
                }

            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }
    }


}
