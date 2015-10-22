using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Core.APCoreLib
{
    public class APXExeception:Exception
    {
        private string _message;
        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public APXExeception()
        {
            APXlog.Write(APXlog.BuildLogMsg(base.Message));
        }
        public APXExeception(string message)
        {
            _message = message;
            APXlog.Write(APXlog.BuildLogMsg(Message));
        }
    }
}
