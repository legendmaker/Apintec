using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols.Fins
{
    class FinsHeader
    {
        public byte ICF { set; internal get; }
        public byte RSV { set; internal get; }
        public byte GCT { set; internal get; }
        private byte _dna;
        public byte DNA
        {
            set
            {
                if (value > 0x7F)
                {
                    throw new APXExeception(String.Format(
                        "Set Fins Header DNA out of range,DNA={0}", value));
                }
                else
                {
                    _dna = value;
                }
            }
            internal get
            {
                return _dna;
            }
        }
        public byte DA1 { set; internal get; }
        private byte _da2;
        public byte DA2
        {
            set
            {
                if((value==0x00)||((value>=0x10)&&(value<=1F))
                    ||(value==0xE1)||(value==0xFE))
                {
                    _da2 = value;
                }
                else
                {
                    throw new APXExeception(String.Format(
                        "Set Fins Header DA2 out of range,DA2={0}", value));
                }
            }
            internal get
            {
                return _da2;
            }
        }
        public byte SNA { set; internal get; }
        public byte SA1 { set; internal get; }

        private byte _sa2;
        public byte SA2
        {
            set
            {
                if ((value == 0x00) || ((value >= 0x10) && (value <= 1F))
                    || (value == 0xE1) || (value == 0xFE))
                {
                    _sa2 = value;
                }
                else
                {
                    throw new APXExeception(String.Format(
                        "Fins Header SA2 out of range,SA2={0}", value));
                }
            }
            internal get
            {
                return _sa2;
            }
        }
        public byte SID { set; internal get; }
        public PlcProtocolType ProtType { get; internal set; }
        public FinsHeader()
        {
            InitHeaderValues();
        }
        public FinsHeader(PlcProtocolType pt) : this()
        {
            ProtType = pt;
        }
        public FinsHeader(PlcProtocolType pt, byte sna, byte sunit, byte snode,
            byte dna, byte dunit, byte dnode) :this(pt)
        {
            SNA = sna;
            SA1 = snode;
            SA2 = sunit;
            DNA = dna;
            DA1 = dnode;
            DA2 = dunit;
        }
        private void InitHeaderValues()
        {
            ICF = 0x80;
            RSV = 0x00;
            GCT = 0x02;
            DNA = 0x00;
            DA1 = 0x00;
            DA2 = 0x00;
            SNA = 0x00;
            SA1 = 0x00;
            SA2 = 0x00;
            SID = 0x00;
            BulidCommandHeader();
        }

        public byte[] ToArray()
        {
            byte[] headerArray = new byte[10];
            headerArray[0] = ICF;
            headerArray[1] = RSV;
            headerArray[2] = GCT;
            headerArray[3] = DNA;
            headerArray[4] = DA1;
            headerArray[5] = DA2;
            headerArray[6] = SNA;
            headerArray[7] = SA1;
            headerArray[8] = SA2;
            headerArray[9] = SID;
            return headerArray;
        }
        private void BulidCommandHeader()
        {
            ICF &= ~(1 << 6) & 0xFF;
            
        }
        private void BuildResponseHeader()
        {
            ICF |= (1 << 6) & 0xFF;
        }
        public bool IsResponseHeader(byte data)
        {
            if (((data & ~(1 << 6) & 0xFF) == ICF) 
                && ((data & ((1 << 6) & 0xFF)) != 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
