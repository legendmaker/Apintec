using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.IOCards
{
    public class IOCardInfo:HardwareInfo
    {
        public virtual int Channels { get; protected set; }
        public virtual int CardType { get; protected set; }
        public virtual int Index { get; protected set; }
        public IOCardInfo()
        {
        }
        public IOCardInfo(string vendor, int channels, int cardType, int index)
        {
            Channels = channels;
            CardType = cardType;
            Index = index;
            Vendor = vendor;
        }
    }
}
