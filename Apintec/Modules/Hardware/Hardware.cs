using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules
{
    public abstract class Hardware:HardwareInfo, IHardware
    {
        public virtual event EventHandler OnOpen;
        public virtual event EventHandler OnClose;
        public virtual event EventHandler OnStart;
        public virtual event EventHandler OnStop;

        protected virtual void RaiseOnOpenEvent()
        {
            if (OnOpen != null)
            {
                if (OnOpen != null)
                {
                    OnOpen(this, new EventArgs());
                }
            }
        }
        protected virtual void RaiseOnCloseEvent()
        {
            if (OnClose != null)
            {
                if (OnClose != null)
                {
                    OnClose(this, new EventArgs());
                }
            }
        }
        protected virtual void RaiseOnStartEvent()
        {
            if (OnStart != null)
            {
                OnStart(this, new EventArgs());
            }
        }
        protected virtual void RaiseOnStopEvent()
        {
            if (OnStop != null)
            {
                OnStop(this, new EventArgs());
            }
        }

        public abstract bool Open();
        public abstract bool Close();
        public abstract bool Start();
        public abstract bool Stop();
        public abstract void Dispose();
    }
}
