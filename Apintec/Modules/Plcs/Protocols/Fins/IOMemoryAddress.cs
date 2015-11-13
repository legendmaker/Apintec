using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols.Fins
{
    public class IOMemoryAddress
    {
        public IOMemoryArea Area { get; private set; }
        public IOMemeoryDataType DataType { get; private set; }
        public Omron.Mode Mode { get; private set; }
        public byte MemroyAreaCode { get; private set; }
        public ushort AddressWord{ get; private set; }
        public byte AddressBit { get; private set; }
        public static Dictionary<IOMemoryArea, Dictionary<IOMemeoryDataType,byte>> MemoryAreaCodeDict { get; private set; }
        static IOMemoryAddress()
        {
            MemoryAreaCodeDict = new Dictionary<IOMemoryArea, Dictionary<IOMemeoryDataType, byte>>();
            Dictionary<IOMemeoryDataType, byte> CIODict = new Dictionary<IOMemeoryDataType, byte>();
            CIODict.Add(IOMemeoryDataType.Bit, 0x30);
            CIODict.Add(IOMemeoryDataType.BitWithForcedStatus, 0x70);
            CIODict.Add(IOMemeoryDataType.Word, 0xB0);
            CIODict.Add(IOMemeoryDataType.WordWithForcedStatus, 0xF0);

            Dictionary<IOMemeoryDataType, byte> WRDict = new Dictionary<IOMemeoryDataType, byte>();
            WRDict.Add(IOMemeoryDataType.Bit, 0x31);
            WRDict.Add(IOMemeoryDataType.BitWithForcedStatus, 0x71);
            WRDict.Add(IOMemeoryDataType.Word, 0xB1);
            WRDict.Add(IOMemeoryDataType.WordWithForcedStatus, 0xF1);

            Dictionary<IOMemeoryDataType, byte> HRDict = new Dictionary<IOMemeoryDataType, byte>();
            HRDict.Add(IOMemeoryDataType.Bit, 0x32);
            HRDict.Add(IOMemeoryDataType.BitWithForcedStatus, 0x72);
            HRDict.Add(IOMemeoryDataType.Word, 0xB2);
            HRDict.Add(IOMemeoryDataType.WordWithForcedStatus, 0xF2);

            Dictionary<IOMemeoryDataType, byte> ARDict = new Dictionary<IOMemeoryDataType, byte>();
            ARDict.Add(IOMemeoryDataType.Bit, 0x33);
            ARDict.Add(IOMemeoryDataType.Word, 0xB3);

            Dictionary<IOMemeoryDataType, byte> TIMDict = new Dictionary<IOMemeoryDataType, byte>();
            TIMDict.Add(IOMemeoryDataType.CompletionFlag, 0x09);
            TIMDict.Add(IOMemeoryDataType.CompletionFlagWithForcedStatus, 0x49);
            TIMDict.Add(IOMemeoryDataType.PV, 0x89);

            Dictionary<IOMemeoryDataType, byte> CNTDict = new Dictionary<IOMemeoryDataType, byte>();
            CNTDict.Add(IOMemeoryDataType.CompletionFlag, 0x09);
            CNTDict.Add(IOMemeoryDataType.CompletionFlagWithForcedStatus, 0x49);
            CNTDict.Add(IOMemeoryDataType.PV, 0x89);

            Dictionary<IOMemeoryDataType, byte> DMDict = new Dictionary<IOMemeoryDataType, byte>();
            DMDict.Add(IOMemeoryDataType.Bit, 0x02);
            DMDict.Add(IOMemeoryDataType.Word, 0x82);

            Dictionary<IOMemeoryDataType, byte> EMDictBankDict = new Dictionary<IOMemeoryDataType, byte>();
            EMDictBankDict.Add(IOMemeoryDataType.Bit, 0x20);
            EMDictBankDict.Add(IOMemeoryDataType.Word, 0xA0);

            Dictionary<IOMemeoryDataType, byte> EMDictBankCurrentDict = new Dictionary<IOMemeoryDataType, byte>();
            EMDictBankCurrentDict.Add(IOMemeoryDataType.Bit, 0x98);
            EMDictBankCurrentDict.Add(IOMemeoryDataType.Word, 0xBC);

            Dictionary<IOMemeoryDataType, byte> TKDict = new Dictionary<IOMemeoryDataType, byte>();
            TKDict.Add(IOMemeoryDataType.Bit, 0x06);
            TKDict.Add(IOMemeoryDataType.Status, 0x46);

            Dictionary<IOMemeoryDataType, byte> IRDict = new Dictionary<IOMemeoryDataType, byte>();
            IRDict.Add(IOMemeoryDataType.PV, 0xDC);

            Dictionary<IOMemeoryDataType, byte> DRDict = new Dictionary<IOMemeoryDataType, byte>();
            DRDict.Add(IOMemeoryDataType.PV, 0xBC);

            Dictionary<IOMemeoryDataType, byte> ClockPulsesDict = new Dictionary<IOMemeoryDataType, byte>();
            ClockPulsesDict.Add(IOMemeoryDataType.Bit, 0x07);

            Dictionary<IOMemeoryDataType, byte> ConditionFlagDict = new Dictionary<IOMemeoryDataType, byte>();
            ConditionFlagDict.Add(IOMemeoryDataType.Bit, 0x07);

            MemoryAreaCodeDict.Add(IOMemoryArea.CIO, CIODict);
            MemoryAreaCodeDict.Add(IOMemoryArea.AR, ARDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.ClockPulses, ClockPulsesDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.CNT, CNTDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.ConditionFlag, ConditionFlagDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.DM, DMDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.DR, DRDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.EMBank, EMDictBankDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.EMBankCurrent, EMDictBankCurrentDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.HR, HRDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.IR, IRDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.TIM, TIMDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.TK, TKDict);
            MemoryAreaCodeDict.Add(IOMemoryArea.WR, WRDict);
        }

        public IOMemoryAddress(IOMemoryArea area, IOMemeoryDataType dataType, Omron.Mode mode, ushort addressWord, byte addressBit)
        {
            Area = area;
            DataType = dataType;
            Mode = mode;
            AddressWord = addressWord;
            AddressBit = addressBit;

        }

        public byte[] ToByte()
        {
            byte[] addr = new byte[4];
            try
            {
                addr[0] = MemoryAreaCodeDict[Area][DataType];
                addr[1] = BitConverter.GetBytes(AddressWord)[1];
                addr[2] = BitConverter.GetBytes(AddressWord)[0];
                addr[3] = AddressBit;
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            return addr;
        }
    }
}
