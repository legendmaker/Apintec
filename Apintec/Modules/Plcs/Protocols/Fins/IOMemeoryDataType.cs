using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Plcs.Protocols.Fins
{
    public enum IOMemeoryDataType
    {
        Bit,
        BitWithForcedStatus,
        Word,
        WordWithForcedStatus,
        CompletionFlag,
        CompletionFlagWithForcedStatus,
        PV,
        EM,
        Status
    }
}
