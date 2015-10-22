using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances
{
    public class XSerialParameter
    {
        public string PortName { get; set; }
        public int Baudrate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get;  set; }
        public StopBits StopBits { get; set; }
        public Handshake Handshake { get; set; }
        public XSerialParameter(string portName, int baudRate, Parity parity, 
            int dataBits, StopBits stopBits, Handshake handShake)
        {
            PortName = portName;
            Baudrate = baudRate;
            Parity = parity;
            DataBits = DataBits;
            StopBits = stopBits;
            Handshake = handShake;
        }

        public XSerialParameter()
        {
        }
    }
}
