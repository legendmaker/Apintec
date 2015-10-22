using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.IOCards
{
    public abstract class IOCard :IOCardInfo, IIOCard
    {
        
        public event EventHandler OnStart;
        public event EventHandler OnStop;
        public abstract event EventHandler OnRead;
        public abstract event EventHandler OnWrite;
        public abstract event EventHandler OnOpen;
        public abstract event EventHandler OnClose;

        public IOCard()
        {
            CardType = 0;
            Index = 0;
        }
        public IOCard(IOCardInfo info)
        {
            this.Name = info.Name;
            this.CardType = info.CardType;
            this.Channels = info.Channels;
            this.DeviceID = info.DeviceID;
            this.Index = info.Index;
            this.IsOpen = info.IsOpen;
            this.Module = info.Module;
            this.Vendor = info.Vendor;
        }
        public abstract bool Read(PortType portType, uint port, out BitArray bitArray);
        public abstract bool Write(PortType portType, uint port, BitArray bitArray);
      

        public bool Start()
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public abstract bool Open();
        public abstract bool Close();
        public abstract void Dispose();
    }
}
