using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Communiction.APXCom
{
    public class XComInfo
    {
        public string Type { get; internal set; }
        public object Parameter { get; internal set; }
        public string Key { get; internal set; }
        public XComInfo()
        {
        }
        public XComInfo(string type, object para, string key)
        {
            Type = type;
            Parameter = para;
            Key = key;
        }
    }
}
