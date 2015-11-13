using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Protocols;
using Apintec.Modules.Plcs.Protocols.Fins;
using System;
using System.Threading;

namespace Apintec.Modules.Plcs.Vendors
{
    public class Omron : Plc
    {
        public bool IsFinsProtocol
        {
            get
            {
                if(ProtocolInstance is Fins)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Omron(PlcInfo info):base(info)
        {
        }
        public override event EventHandler OnClose;
        public override event EventHandler OnOpen;
        public override event EventHandler OnStart;
        public override event EventHandler OnStop;

        public override bool Close()
        {
            return true;
        }

        public override bool Open()
        {
            return true;
        }

        public override bool Start()
        {
            return base.Start();
        }
        public override bool Stop()
        {
            return base.Stop();
        }

        public override string ToString()
        {
           return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        }

        public override bool Read(ref byte[] buffer, int offset, int length, params object[] parameters)
        {

            bool isOk = false;
            if(ProtocolInstance!=null)
            {
                if(ProtocolInstance is Fins)
                {
                    IOMemoryAddress ioMemoryAddr = parameters[0] as IOMemoryAddress;
                    if (ioMemoryAddr == null) 
                    {
                        throw new APXExeception("IOMemeoryAddress is null, parameters invalid.");
                    }
                    try
                    {
                        byte[] frame = new byte[0];
                        byte[] len = new byte[2];
                        len[0] = BitConverter.GetBytes(length)[1];
                        len[1] = BitConverter.GetBytes(length)[0];
                        isOk = ProtocolInstance.SendMessage(PlcCommand.MemoryAreaRead, Gadget.ArrayAppend(ioMemoryAddr.ToByte(), len),ref frame);
                        var ret = ProtocolInstance.ParseResult(frame, 1000, ioMemoryAddr);
                        if (ret == null)
                        {
                            return false;
                        }
                        else
                        {
                            buffer = (byte[])ret;
                        }
                    }
                    catch(Exception e)
                    {
                        throw new APXExeception(e.Message);
                    }
                   
                }
            }
            return isOk;
        }

        public override bool Write(byte[] buffer, int offset, int length, params object[] parameters)
        {
            bool isOk = false;
            if (ProtocolInstance != null)
            {
                if (ProtocolInstance is Fins)
                {
                    IOMemoryAddress ioMemoryAddr = parameters[0] as IOMemoryAddress;
                    if (ioMemoryAddr == null)
                    {
                        throw new APXExeception("IOMemeoryAddress is null, parameters invalid.");
                    }
                    try
                    {
                        byte[] frame = new byte[0];
                        byte[] len = new byte[2];
                        len[0] = BitConverter.GetBytes(length)[1];
                        len[1] = BitConverter.GetBytes(length)[0];
                        byte[] data = Gadget.ArrayAppend(len, buffer);
                        isOk = ProtocolInstance.SendMessage(PlcCommand.MemoryAreaWrite, Gadget.ArrayAppend(ioMemoryAddr.ToByte(), data), ref frame);
                        var ret = ProtocolInstance.ParseResult(frame, 1000, ioMemoryAddr);
                        if (ret == null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    catch(Exception e)
                    {
                        throw new APXExeception(e.Message);
                    }
                    
                }
            }
            return isOk;
        }

        public enum Mode
        {
            CS,
            CJ,
            CV
        }
    }
}
