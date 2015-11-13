using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols.Fins
{
    public class FinsCmd
    {
        public PlcCommand Command { get; protected set; }
        private byte _mrc;

        public byte MRC
        {
            get { return _mrc; }
            set { _mrc = value; }
        }

        private byte _src;

        public byte SRC
        {
            get { return _src; }
            set { _src = value; }
        }
        public static Dictionary<PlcCommand, FinsCmd> FinsCmdDic { get; protected set; }
        static FinsCmd()
        {
            FinsCmdDic = new Dictionary<PlcCommand, FinsCmd>();
            FinsCmdDic.Add(PlcCommand.MemoryAreaRead, new FinsCmd(0x01, 0x01));
            FinsCmdDic.Add(PlcCommand.MemoryAreaWrite, new FinsCmd(0x01, 0x02));
        }
        public FinsCmd()
        {

        }
        public FinsCmd(byte mrc, byte src)
        {
            _mrc = mrc;
            _src = src;
        }
        public FinsCmd(PlcCommand cmd)
        {
            Command = cmd;
        }
        public byte[] ToArray()
        {
            byte[] cmdArray = new byte[2];
            FinsCmd cmd = FinsCmdDic[Command];
            cmdArray[0] = cmd.MRC;
            cmdArray[1] = cmd.SRC;
            return cmdArray;
        }
        public static PlcCommand MatchPlcCommand(FinsCmd cmd)
        {
            foreach (var item in FinsCmdDic)
            {
                if((item.Value.MRC ==cmd.MRC) && (item.Value.SRC==cmd.SRC))
                {
                    return item.Key;
                }
            }
            return PlcCommand.None;
        }
    }
}
