using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Serial
{
    public class XSerialParameter
    {
        public string PortName { get; internal set; }
        public int Baudrate { get; internal set; }
        public Parity Parity { get; internal set; }
        public int DataBits { get; internal set; }
        public StopBits StopBits { get; internal set; }
        public Handshake Handshake { get; internal set; }
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
