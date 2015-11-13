using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public abstract class XCom : IXCom
    {
        public abstract bool IsConnected { get; }

        public abstract event EventHandler OnConnect;
        public abstract event EventHandler OnDisconnect;
        public abstract event EventHandler OnReceive;
        public abstract event EventHandler OnSend;

        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract int Receive(ref byte[] buffer, int offset, int count);
        public abstract bool Send(byte[] buffer, int offset, int count);
    }
}
