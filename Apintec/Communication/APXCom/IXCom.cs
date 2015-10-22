using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public interface IXCom
    {
        bool Connect();
        bool Disconnect();
        bool Send(byte[] buffer, int offset, int count);
        int Receive(byte[] buffer, int offset, int count);

        event EventHandler OnConnect;
        event EventHandler OnDisconnect;
        event EventHandler OnReceive;
        event EventHandler OnSend;
    }
}
