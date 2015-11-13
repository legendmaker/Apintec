using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public class XComInstanceInfo
    {
        public XComInfo Info { get; internal set; }
        public Type Type { get; internal set; }
        public IXCom Instance()
        {
             return (Activator.CreateInstance(Type, Info.Parameter)) as IXCom;
        }
        public XComInstanceInfo()
        {
        }
        public XComInstanceInfo(XComInfo info, Type type)
        {
            Info = info;
            Type = type;
        }
    }
}
