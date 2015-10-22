using Apintec.Core.APCoreLib;
using System;
using System.Collections;

namespace Apintec.Modules.IOCards.Vendors.Adlink
{
    public class IODask : IOCard
    {
        private short _mDev;
        public override int CardType
        {
            get
            {
                return base.CardType;
            }

            protected set
            {
                base.CardType = value;
                Module = GetCardName(Convert.ToUInt16(CardType));
            }
        }

        public override event EventHandler OnClose;
        public override event EventHandler OnOpen;
        public override event EventHandler OnRead;
        public override event EventHandler OnWrite;

        public IODask() : base()
        {
            CardType = DASK.PCI_7230;
            _mDev = 0;
            Module = GetCardName(Convert.ToUInt16(CardType));
            Vendor = "Adlink";
        }
        public IODask(ushort cardType, int channels) : this()
        {
            CardType = cardType;
            Channels = channels;
        }
        public IODask(IOCardInfo info):base(info)
        {
            Name = Module = GetCardName(Convert.ToUInt16(CardType));
            Vendor = "Adlink";
        }

        public IODask(ushort cardType, ushort index, int channels) : this()
        {
            CardType = cardType;
            Index = index;
            Channels = channels;
        }
        private string GetCardName(ushort cardType)
        {
            string cardName = "";
            System.Reflection.FieldInfo[] fields = typeof(DASK).GetFields();
            foreach (System.Reflection.FieldInfo info in fields)
            {
                string tmp = typeof(DASK).GetField(info.Name).GetValue("").ToString();
                if (tmp == cardType.ToString())
                {
                    cardName = info.Name;
                    break;
                }
            }
            return cardName;
        }
        public override bool Close()
        {
            return DaskCardRelease();
        }

        public override bool Open()
        {
            if (IsOpen)
                return true;
            return DaskCardRegister();
        }

        public override bool Read(PortType portType, uint port, out BitArray bitArray)
        {
            uint value = 0;
            bool isOk;
            bitArray = new BitArray(Channels,false);
            if (!IsOpen)
                return false;
            if (portType == PortType.DO)
            {
                isOk = DOReadPort(port, out value);
                RaisedOnReadEvent(new IOCardEventArgs(port,
                        Gadget.IntToBitArray(value, Channels), isOk));
                if (isOk)
                {
                    bitArray = Gadget.IntToBitArray(value, Channels);
                    return true;
                }
                else
                {
                    bitArray = new BitArray(Channels, false);
                    throw new APXExeception(String.Format("{0} Read Fail.", portType.ToString()));
                }
            }
            else if (portType == PortType.DI)
            {
                isOk = DIReadPort(port, out value);
                if (isOk)
                {
                    bitArray = Gadget.IntToBitArray(value, 32);
                    return true;
                }
                else
                {
                    bitArray = new BitArray(32, false);
                    throw new APXExeception("DI Read Fail.");
                }
            }
            else
            {
                bitArray = new BitArray(32, false);
                throw new APXExeception("PortType: {0} is not Support.");
            }

        }

        public override bool Write(PortType portType, uint port, BitArray bitArray)
        {
            bool isOK;
            if (!IsOpen)
                return false;
            isOK = DOWritePort(port, Convert.ToUInt32(Gadget.BitArrayToInt32(bitArray)));
            RaisedOnWriteEvent(new IOCardEventArgs(port, bitArray, isOK));
            if (!isOK)
            {
                throw new APXExeception("DO Write fail.");
            }
            return true;
        }
        private bool DaskCardRegister()
        {
            _mDev = DASK.Register_Card(Convert.ToUInt16(CardType), Convert.ToUInt16(Index));
            if (_mDev < 0)
                return false;
            else
            {
                RaiseOnOpenEvent();
                IsOpen = true;
                return true;
            }
        }

        private bool DaskCardRelease()
        {
            if (_mDev >= 0)
            {
                DASK.Release_Card(Convert.ToUInt16(_mDev));
                IsOpen = false;
                RaiseOnCloseEvent();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DOWritePort(uint Port, uint Value)
        {
            short retVal;
            if (_mDev >= 0)
            {
                retVal = DASK.DO_WritePort(Convert.ToUInt16(_mDev), Port, Value);
                if (retVal != DASK.NoError)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool DOReadPort(uint Port, out uint Value)
        {
            short retVal;
            if (_mDev >= 0)
            {
                retVal = DASK.DO_ReadPort(Convert.ToUInt16(_mDev), Convert.ToUInt16(Port), out Value);
                if (retVal != DASK.NoError)
                {
                    return false;
                }
            }
            else
            {
                Value = 0;
                return false;
            }
            return true;
        }

        private bool DIReadPort(uint Port, out uint Value)
        {
            short retVal;
            if (_mDev >= 0)
            {
                retVal = DASK.DI_ReadPort(Convert.ToUInt16(_mDev), Convert.ToUInt16(Port), out Value);
                if (retVal != DASK.NoError)
                {
                    return false;
                }
            }
            else
            {
                Value = 0;
                return false;
            }
            return true;
        }
        protected void RaisedOnReadEvent(IOCardEventArgs args)
        {
            if (OnRead != null)
            {
                OnRead(this, args);
            }
        }
        protected void RaisedOnWriteEvent(IOCardEventArgs args)
        {
            if (OnWrite != null)
            {
                OnWrite(this, args);
            }
        }

        protected void RaiseOnOpenEvent()
        {
            if (OnOpen != null)
            {
                OnOpen(this, new EventArgs());
            }
        }
        protected void RaiseOnCloseEvent()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }

        public override void Dispose()
        {
            
        }
    }
}
