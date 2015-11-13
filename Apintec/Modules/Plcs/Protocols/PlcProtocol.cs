using Apintec.Communiction.APXCom;
using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols
{
    public class PlcProtocol : IPlcProtocol, IDisposable 
    {
        public virtual bool IsStarted { get; protected set; }
        public virtual IXCom Comm { get; protected set; }
        protected byte[] _recvBuff = new byte[0];
        protected Thread _recvThread;
        public virtual List<byte[]> Messages { get; protected set; }

        public PlcProtocol(IXCom comm)
        {
            Comm = comm;
            IsStarted = false;
        }

        public virtual bool Start()
        {
            if (IsStarted)
                return true;
            try
            {
                if (!Comm.Connect())
                {
                    throw new APXExeception("Fins communication connect fail.");
                }
                else
                {
                    Messages = new List<byte[]>();
                    _recvThread = new Thread(ReceiveFun);
                    _recvThread.Start();
                    IsStarted = true;
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
           
        }

        private void ReceiveFun()
        {
            byte[] recv = new byte[0];
            while (true)
            {
                try
                {
                    Comm.Receive(ref recv, 0, 0);
                }
                catch (Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.Message));
                }
                lock (_recvBuff)
                {
                    _recvBuff = Gadget.ArrayAppend(_recvBuff, recv);
                }

            }
        }

        public virtual bool Stop()
        {
            if (!IsStarted)
                return true;
            if (_recvThread != null)
            {
                try
                {
                    _recvThread.Abort();
                    Comm.Disconnect();
                    IsStarted = false;
                    return true;
                }
                catch (Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.Message));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public virtual bool SendMessage(PlcCommand cmd, byte[] content, ref byte[] frame)
        {
            if (!IsStarted)
                return false;
            if(Comm!=null)
            {
                try
                {
                    if (Comm.Send(content, 0, content.Length))
                    {
                        frame = content;
                        return true;
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
            else
            {
                return false;
            }
        }

        public virtual object ParseResult(byte[] frame, int timeOut, params object[] para)
        {
            return _recvBuff;
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
                try
                {
                    Stop();
                }
                catch(Exception)
                { }
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~PlcProtocol() {
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
