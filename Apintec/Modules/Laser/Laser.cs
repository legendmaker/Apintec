using Apintec.Communiction.APXCom;
using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Core.APCoreLib;
using Apintec.Modules.Laser.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser
{
    public abstract class Laser : ILaser
    {
        internal virtual LaserProtocol Protocol { get; set; }
        public virtual LaserInfo Info { get; internal set; }
        public virtual bool IsStarted { get; protected set; }

        public abstract event EventHandler OnOpen;
        public abstract event EventHandler OnClose;
        public abstract event EventHandler OnStart;
        public abstract event EventHandler OnStop;

        public Laser(LaserInfo info)
        {
            Info = info;
            IsStarted = false;
        }
        public abstract bool Open();
        public abstract bool Close();
        public virtual bool Start()
        {
            if (IsStarted)
                return true;
            try
            {
                switch (Info.ProtType)
                {
                    case LaserProtocolType.LaserTCP:
                    case LaserProtocolType.LaserUDP:
                        NetParameter para = new NetParameter(Info.IPAddress, Info.Port);
                        Protocol = new LaserProtocol(XComManager.XComsDict[NetKey.BuildNetKey(Info.ComKey, para)].Instance());
                        break;
                    default:
                        break;
                }
                if(Protocol!=null)
                {
                    Protocol.Start();
                    IsStarted = true;
                }
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
            if (Protocol != null)
            {
                try
                {
                    Protocol.Stop();
                    IsStarted = false;
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.Message);
                }
            }
            return true;
        }
        public abstract bool Run(LaserCommand cmd, params object[] para);
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
                catch (Exception)
                { }
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Laser() {
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
