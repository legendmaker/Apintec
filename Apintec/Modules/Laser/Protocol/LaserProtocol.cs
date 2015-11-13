using Apintec.Communiction.APXCom;
using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;

namespace Apintec.Modules.Laser.Protocol
{
    internal class LaserProtocol : IDisposable 
    {
        #region Protocol FixedDefine
        private const uint FrameLength = 32;
        private const uint HeadIndex = 0;
        private const uint MagicIndex = 1;
        private const uint TypeIndex = 2;
        private const uint ContentIndex = 3;
        private const uint ContentLength = 16;
        private const uint ReserveIndex = 19;
        private const uint ReserveLenth = 11;
        private const uint AckIndex = 30;
        private const uint TailIndex = 31;
        #endregion

        private const byte _head = 0xBB;
        public byte Head
        {
            get { return _head; }
        }

        private byte _magic;

        public byte Magic
        {
            get { return _magic; }
            internal set { _magic = value; }
        }

        private byte[] _content = new byte[ContentLength];

        public byte[] Content
        {
            get { return _content; }
            internal set
            {
                Array.Clear(_content, 0, Convert.ToInt32(ContentLength));
                if(value.Length>ContentLength)
                {
                    throw new APXExeception(String.Format("Illegal laser protocol content length:{0}"
                        ,value.Length));
                }
                Array.Copy(value, _content, value.Length);
            }

        }

        private byte[] reserve = new byte[ReserveLenth];

        public byte[] Reserve
        {
            get { return reserve; }
            internal set { reserve = value; }
        }

        private byte _ack;

        public byte Ack
        {
            get { return _ack; }
            internal set { _ack = value; }
        }

        private const byte _tail = 0xFE;

        public byte Tail
        {
            get { return _tail; }
        }

        private object _sync = new object();

        public bool IsStart { get; protected set; }
        private IXCom _comm;
        private byte[] _recvBuff = new byte[0];
        public List<byte[]> Messages { get; private set; }

        public event EventHandler OnMessageReceive;
        public LaserProtocol(IXCom xCom)
        {
            Magic = 0x00;
            Array.Clear(Content, 0, Convert.ToInt32(ContentLength));
            Array.Clear(Reserve, 0, Convert.ToInt32(ReserveLenth));
            Ack = 0x00;
            _comm = xCom;
            IsStart = false;
            Messages = new List< byte[]>();
            OnMessageReceive += LaserProtocol_OnMessageReceive;
        }

        public bool SendMessage(LaserProtocolMessageType type, LaserProtocolMessageAck ack,  byte[] content, ref byte[] frame)
        {
            Content = content;
            bool isOK = false;
            try
            {
                frame = BuildFrame(type, ack);
                if (_comm != null)
                {
                    isOK = _comm.Send(frame, 0, frame.Length);
                    if(isOK)
                    {
                        if(ack == LaserProtocolMessageAck.Initial)
                        {
                            LaserProtocolMessageAck ackTmp = CheckResult(frame, 200);
                            if (ackTmp != LaserProtocolMessageAck.ReceiveConfirm)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
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
        public LaserProtocolMessageAck CheckResult(byte[] frame, int timeout)
        {
            bool isFind = false;
            var t = new System.Timers.Timer(timeout);
            t.AutoReset = false;
            t.Elapsed += T_Elapsed;
            t.Start();
            while (true)
            {
                if (!t.Enabled)
                {
                    break;
                }
                lock (Messages)
                {
                    foreach (var item in Messages)
                    {
                        for (int i = 0; i < AckIndex; i++)
                        {
                            if (item[i] == frame[i])
                                isFind = true;
                            else
                                isFind = false;
                        }
                        if (isFind)
                        {
                            Messages.Remove(item);
                            return (LaserProtocolMessageAck)Enum.ToObject(typeof(LaserProtocolMessageAck), item[AckIndex]);
                        }
                    }
                }
            }
            return LaserProtocolMessageAck.None;
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new APXExeception("Laser CheckResult time out.");
        }

        private void LaserProtocol_OnMessageReceive(object sender, EventArgs e)
        {
            LaserProtocolEventArgs args = e as LaserProtocolEventArgs;
            if (args == null)
                return;
            if(MessageNeedReceiveConfirm(args.Message))
            {
                SendReceiveConfirmMessage(args.Message);
            }
        }

        private void SendReceiveConfirmMessage(byte[] message)
        {
            byte[] confirm = new byte[FrameLength];
            Array.Copy(message, confirm, FrameLength);
            confirm[AckIndex] = Convert.ToByte(LaserProtocolMessageAck.ReceiveConfirm);
            try
            {
                _comm.Send(confirm, 0, Convert.ToInt32(FrameLength));
            }
            catch(Exception)
            { }
        }

        private bool MessageNeedReceiveConfirm(byte[] message)
        {
            if ((message[AckIndex] == Convert.ToByte(LaserProtocolMessageAck.CommandSucceed))
                || (message[AckIndex] == Convert.ToByte(LaserProtocolMessageAck.CommandFail)))
                return true;
            else
                return false;
        }

        private byte[] BuildFrame(LaserProtocolMessageType type, LaserProtocolMessageAck ack)
        {
            try
            {
                byte[] _frame = new byte[FrameLength];
                _frame[HeadIndex] = Head;
                lock(_sync)
                {
                    _frame[MagicIndex] = ++Magic;
                }
                _frame[TypeIndex] = Convert.ToByte(type);
                Array.Copy(Content, 0, _frame, ContentIndex, ContentLength);
                Array.Copy(Reserve, 0, _frame, ReserveIndex, ReserveLenth);
                _frame[AckIndex] = Convert.ToByte(ack);
                _frame[TailIndex] = Tail;
                return _frame;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }
        public bool Start()
        {
            if (IsStart)
                return true;
            try
            {
                if (!_comm.Connect())
                {
                    return false;
                }
                _comm.OnReceive += _comm_OnReceive;
                IsStart = true;
                return true;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        private void _comm_OnReceive(object sender, EventArgs e)
        {
            var args = e as XComEventArgs;
            if (args == null)
                return;
            byte[] recv = new byte[0];
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
                ParseMessage();
            }
        }

        private void ParseMessage()
        {
            int headIndex = 0;
            if (_recvBuff.Length < FrameLength)
                return;
            foreach (var item in _recvBuff)
            {
                if (item == Head) 
                {
                    if(_recvBuff.Length<headIndex+FrameLength)
                    {
                        return;
                    }
                    else
                    {
                        if(_recvBuff[headIndex+ FrameLength - 1]==Tail)
                        {
                            byte[] message = new byte[FrameLength];
                            Array.Copy(_recvBuff, headIndex, message, 0, FrameLength);
                            byte[] rest = new byte[_recvBuff.Length - (headIndex + FrameLength)];
                            Array.Copy(_recvBuff, headIndex + FrameLength, rest, 0, _recvBuff.Length - (headIndex + FrameLength));
                            _recvBuff = rest;
                            lock(Messages)
                            {
                                Messages.Add(message);
                            }
                            RaiseOnMessageReceivedEvent(message);
                        }
                        else
                        {
                            headIndex++;
                        }
                    }
                }
                else
                {
                    headIndex++;
                }
            }
        }

        private void RaiseOnMessageReceivedEvent(byte[] message)
        {
            if(OnMessageReceive!=null)
            {
                OnMessageReceive(this, new LaserProtocolEventArgs(message));
            }
        }

        public bool Stop()
        {
            if (!IsStart)
                return true;
            try
            {
                _comm.OnReceive -= _comm_OnReceive;
                if (!_comm.Disconnect())
                    return false;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            IsStart = false;
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                Stop();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~LaserProtocol() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
