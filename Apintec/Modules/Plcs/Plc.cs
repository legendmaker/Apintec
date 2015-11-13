using Apintec.Communiction.APXCom;
using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Protocols;
using Apintec.Modules.Plcs.Protocols.Fins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs
{
    public abstract class Plc : IPlc
    {
        public virtual PlcInfo Info { get; internal set; }
        public virtual PlcProtocol ProtocolInstance { get; protected set; }
        public virtual IXCom Comm { get; internal set; }
        public virtual string Key { get;internal set; }
        public virtual bool IsStarted { get; private set; }
        public Plc(PlcInfo info)
        {
            Info = info;
            IsStarted = false;
        }

        public abstract event EventHandler OnClose;
        public abstract event EventHandler OnOpen;
        public abstract event EventHandler OnStart;
        public abstract event EventHandler OnStop;

        public abstract bool Close();
        public abstract bool Open();
        public virtual bool Start()
        {
            if (IsStarted)
                return true;
            try
            {
                switch (Info.ProtType)
                {
                    case PlcProtocolType.FinsUDP:
                    case PlcProtocolType.FinsTCP:
                        NetParameter para = new NetParameter(Info.IPAddress, Info.Port);
                        Key = NetKey.BuildNetKey(Info.ComKey, para);
                        Comm = XComManager.XComsDict[Key].Instance();
                        ProtocolInstance = new Fins(Info.ProtType, Info.Network, Info.Unit, IPAddress.Parse(Info.IPAddress).GetAddressBytes()[3],
                               Info.Timeout, Info.LocalHostEthIndex, Comm);
                        break;
                    case PlcProtocolType.None:
                    default:
                        ProtocolInstance = new PlcProtocol(Comm);
                        break;
                }
                ProtocolInstance.Start();
                IsStarted = true;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            return true;
        }
        public virtual bool Stop()
        {
            if (!IsStarted)
                return true;
            if(ProtocolInstance!=null)
            {
                try
                {
                    ProtocolInstance.Stop();
                    IsStarted = false;
                }
                catch(Exception e)
                {
                    throw new APXExeception(e.Message);
                }
            }
            return true;
        }
        public abstract bool Write(byte[] buffer, int offset, int length, params object[] parameters);
        public abstract bool Read(ref byte[] buffer, int offset, int length, params object[] parameters);
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
        // ~Plc() {
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
