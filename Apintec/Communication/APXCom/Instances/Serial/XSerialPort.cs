using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom.Instances.Serial
{
    public class XSerialPort : SerialPort, IXCom
    {
        private bool _isConnected;
        public bool IsConnected
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler OnReceive;
        public event EventHandler OnSend;

        public XSerialPort()
        {
            DataReceived += XSerialPort_DataReceived;
        }

        public XSerialPort(XSerialParameter para):this()
        {
            PortName = para.PortName;
            BaudRate = para.Baudrate;
            Parity = para.Parity;
            DataBits = para.DataBits;
            StopBits = para.StopBits;
            Handshake = para.Handshake;
        }

        private void XSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            RaiseOnReceiveEvent();
        }

        public bool Connect()
        {
            try
            {
                Open();
                RaiseOnConnectEvent();
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
            try
            {
                Close();
                RaiseOnDisconnectEvent();
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
            int nByte;
            try
            {
                nByte = Read(buffer, offset, count);
                return nByte;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            
        }

        public bool Send(byte[] buffer, int offset, int count)
        {
            try
            {
                Write(buffer, offset, count);
                return true;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            
        }

        protected void RaiseOnConnectEvent()
        {
            if (OnConnect != null)
            {
                OnConnect(this, new EventArgs());
            }
        }

        protected void RaiseOnDisconnectEvent()
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(this, new EventArgs());
            }
        }

        protected void RaiseOnReceiveEvent()
        {
            if (OnReceive != null)
            {
                OnReceive(this, new XComEventArgs(BytesToRead));
            }
        }
        protected void RaiseOnSend()
        {
            if (OnSend != null)
            {
                OnSend(this, new EventArgs());
            }
        }
        private string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("COM port({0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "")
            {
                portName = defaultPortName;
            }
            return portName;
        }

        private int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate({0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        private Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Parity({0}):", defaultPortParity.ToString());
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity);
        }

        private int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Data Bits({0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits);
        }

        private StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available Stop Bits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Stop Bits({0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits);
        }

        private Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Handshake({0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake);
        }
    }
}
