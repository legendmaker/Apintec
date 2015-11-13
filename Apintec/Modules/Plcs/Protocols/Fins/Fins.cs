using Apintec.Communiction.APXCom;
using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols.Fins
{
    internal class Fins : PlcProtocol 
    {
        private int localHostEthIndex;
        public byte SrcNetwork { get; protected set; }
        public byte SrcUnit { get; protected set; }
        public byte SrcNode { get; protected set; }
        public byte DstNetwork { get; protected set; }
        public byte DstUnit { get; protected set; }
        public byte DstNode { get;protected set; }
        public byte SID { get; protected set; }
        public PlcProtocolType ProtType { get; internal set; }
        public FinsHeader Header { get; internal set; }
        public int FrameIndexBase { get; private set; }

        private int _timeout;
        private object _syncHead = new object();
        private object _syncMessage = new object();
        private const int MinFinsMessageLength = 14;

        public Fins(IXCom comm):base(comm)
        {
            SID = 0;
            Header = new FinsHeader(ProtType, SrcNetwork, SrcUnit, SrcNode, DstNetwork, DstUnit, DstNode);
        }
        public Fins(PlcProtocolType pt, IXCom comm) : this(comm)
        {
            ProtType = pt;
            switch(pt)
            {
                case PlcProtocolType.FinsUDP:
                    FrameIndexBase = 0;
                    break;
                default:
                    break;
            }
            Header = new FinsHeader(ProtType, SrcNetwork, SrcUnit, SrcNode, DstNetwork, DstUnit, DstNode);
        }

        public Fins(PlcProtocolType pt, byte network, byte unit, byte node, IXCom comm) : this(pt, comm)
        {
            DstNetwork = network;
            SrcNetwork = network;
            DstNode = node;
            SrcNode = NetPoint.LocalHostPoint[0].IPAddress.GetAddressBytes()[3];
            DstUnit = unit;
            DstUnit = unit;
            Header = new FinsHeader(ProtType, SrcNetwork, SrcUnit, SrcNode, DstNetwork, DstUnit, DstNode);
        }

        public Fins(PlcProtocolType protocol, byte network, byte unit, byte node, int timeout,
            int localHostEthIndex, IXCom comm) : this(protocol, network, unit, node, comm)
        {
            _timeout = timeout;
            this.localHostEthIndex = localHostEthIndex;
            SrcNode = NetPoint.LocalHostPoint[localHostEthIndex].IPAddress.GetAddressBytes()[3];
            Header = new FinsHeader(ProtType, SrcNetwork, SrcUnit, SrcNode, DstNetwork, DstUnit, DstNode);
        }

        public override bool Start()
        {
            return base.Start();
        }

        public override bool Stop()
        {
            return base.Stop();
        }

        public override bool SendMessage(PlcCommand cmd, byte[] content, ref byte[] frame)
        {
            if (!IsStarted)
                return false;
            try
            {
                frame = BuildFrame(cmd, content);
                if(Comm!=null)
                {
                    if (Comm.Send(frame, 0, frame.Length))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }
        public override object ParseResult(byte[] frame, int timeOut, params object[] para)
        {
            if ((frame == null) || (frame.Length == 0))
                return null;
            lock (_syncMessage)
            {
                var t = new System.Timers.Timer(timeOut);
                t.AutoReset = false;
                t.Elapsed += T_Elapsed;
                t.Start();
                byte[] ret = new byte[0];
                int lengthRatio = 0;
                byte[] tmp = new byte[0];
                int index = 0;
                IOMemoryAddress ioMemoryAddr = para[0] as IOMemoryAddress;
                if (ioMemoryAddr == null)
                {
                    throw new APXExeception("Fins CheckResult parameter invalid");
                }
                switch (ioMemoryAddr.DataType)
                {
                    case IOMemeoryDataType.Bit:
                    case IOMemeoryDataType.BitWithForcedStatus:
                        lengthRatio = 1;
                        break;
                    default:
                        lengthRatio = 2;
                        break;
                }
                while (true)
                {
                    if (!t.Enabled)
                    {
                        break;
                    }
                    if (_recvBuff.Length < FrameIndexBase + 14)
                        continue;
                    lock (_recvBuff)
                    {
                        foreach (var item in _recvBuff)
                        {
                            if (Header.IsResponseHeader(item))
                            {
                                if (_recvBuff.Length - index < FrameIndexBase + 14)
                                    break;
                                if ((frame[3] == _recvBuff[6 + index]) && (frame[4] == _recvBuff[7 + index]) &&
                                    (frame[5] == _recvBuff[8 + index]) && (frame[9] == _recvBuff[9 + index]))
                                {
                                    byte[] rest = null;
                                    byte[] data = null;

                                    switch (FinsCmd.MatchPlcCommand(new FinsCmd(frame[10], frame[11])))
                                    {
                                        case PlcCommand.MemoryAreaRead:
                                            byte[] len = new byte[4];
                                            len[0] = frame[17];
                                            len[1] = frame[16];
                                            int rLength = BitConverter.ToInt32(len, 0) * lengthRatio;
                                            if (rLength + index > _recvBuff.Length)
                                            {
                                                rest = new byte[index];
                                                Array.Copy(_recvBuff, 0, rest, 0, index);
                                                _recvBuff = rest;
                                                t.Stop();
                                                return null;
                                            }
                                            if ((_recvBuff[12 + index] == 0) && (_recvBuff[13 + index] == 0))
                                            {
                                                data = new byte[rLength];
                                                Array.Copy(_recvBuff, index + 14, data, 0, rLength);
                                            }
                                            rest = new byte[_recvBuff.Length - 14 - rLength];
                                            if (rest.Length != 0)
                                            {
                                                Array.Copy(_recvBuff, 0, rest, 0, index);
                                                if (_recvBuff.Length - (index + 14 + rLength) > 0)
                                                    Array.Copy(_recvBuff, index + 14 + rLength, rest, index, _recvBuff.Length - (index + 14 + rLength));
                                            }
                                            break;
                                        case PlcCommand.MemoryAreaWrite:
                                            rest = new byte[_recvBuff.Length - 14];
                                            if (rest.Length != 0)
                                            {
                                                Array.Copy(_recvBuff, 0, rest, 0, index);
                                                if (_recvBuff.Length - (index + 14) > 0)
                                                    Array.Copy(_recvBuff, index + 14, rest, index, _recvBuff.Length - (index + 14));
                                            }
                                            if ((_recvBuff[12 + index] == 0) && (_recvBuff[13 + index] == 0))
                                            {
                                                data = new byte[0];
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    t.Stop();
                                    _recvBuff = rest;
                                    return data;
                                }
                            }
                            index++;
                        }
                    }
                }
                return null;
            }
        }

        private bool IsRespondeMessage(byte[] send, byte[] receive)
        {
            if((send[3] == receive[6])&& (send[4] == receive[7]) && 
                (send[5] == receive[8]) && (send[9] == receive[9]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private byte[] BuildFrame(PlcCommand cmd, byte[] content)
        {
            byte[] frame = new byte[0];
            lock (_syncHead)
            {
                Header.SID = ++SID;
            }
            frame = Gadget.ArrayAppend(frame, Header.ToArray());
            frame = Gadget.ArrayAppend(frame, new FinsCmd(cmd).ToArray());
            frame = Gadget.ArrayAppend(frame, content);
            return frame;
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            APXlog.Write(APXlog.BuildLogMsg("ParseResult time out."));
            lock(_recvBuff)
            {
                for (int i = _recvBuff.Length-1; i>0; i--)
                {
                    if(Header.IsResponseHeader(_recvBuff[i]))
                    {
                        byte[] rest = new byte[_recvBuff.Length - i];
                        Array.Copy(_recvBuff, i, rest, 0, _recvBuff.Length - i);
                        _recvBuff = rest;
                    }
                }
            }
        }
    }
}
