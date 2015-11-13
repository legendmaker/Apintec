using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.views.CognexControlLibrary
{
    internal class CognexClientInfo
    {
        public int Sequence { get; private set; }
        public string IPAddress { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string PassWord { get; private set; }
        public string ProtocoType { get; private set; }
        public string ComKey { get; private set; }
        public CognexClientInfo(int sequence, string ipAddr, int port, string userName, 
            string passWord, string protocolType, string comKey)
        {
            Sequence = sequence;
            IPAddress = ipAddr;
            Port = port;
            UserName = userName;
            PassWord = passWord;
            ProtocoType = protocolType;
            ComKey = comKey;
        }
    }
}
