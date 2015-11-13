using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public class XComEventArgs : EventArgs 
    {
        public int ByteToRead { get; private set; }
        public byte[] Data { get; private set; }
        public XComEventArgs(int byteToRead)
        {
            ByteToRead = byteToRead;
        }
        public XComEventArgs(int byteToRead, byte[] data):this(byteToRead) 
        {
            Data = data;
        }
    }
}
