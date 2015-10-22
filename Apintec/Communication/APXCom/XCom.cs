using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public abstract class XCom : IXCom
    {
        public virtual event EventHandler OnConnect;
        public virtual event EventHandler OnDisconnect;
        public virtual event EventHandler OnReceive;
        public virtual event EventHandler OnSend;

        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract int Receive(byte[] buffer, int offset, int count);
        public abstract bool Send(byte[] buffer, int offset, int count);

        protected void RaiseOnConnectEvent()
        {
            if(OnConnect!=null)
            {
                OnConnect(this, new EventArgs());
            }
        }

        protected void RaiseOnDisconnectEvent()
        {
            if(OnDisconnect!=null)
            {
                OnDisconnect(this, new EventArgs());
            }
        }

        protected void RaiseOnReceiveEvent(XComEventArgs e)
        {
            if(OnReceive!=null)
            {
                OnReceive(this, e);
            }
        }
        protected void RaiseOnSend()
        {
            if(OnSend!=null)
            {
                OnSend(this, new EventArgs());
            }
        }
    }
}
