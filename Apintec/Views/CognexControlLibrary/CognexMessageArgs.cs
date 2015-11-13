using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.views.CognexControlLibrary
{
    public class CognexMessageArgs : EventArgs 
    {
        public byte[] Message { get; private set; }
        public CognexMessageArgs(byte[] message)
        {
            Message = message;
        }
    }
}
