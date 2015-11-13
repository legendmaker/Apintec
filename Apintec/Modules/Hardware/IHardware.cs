using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules
{
    public interface IHardware : IDisposable 
    {
        bool Open();
        bool Close();
        bool Start();
        bool Stop();

        event EventHandler OnOpen;
        event EventHandler OnClose;
        event EventHandler OnStart;
        event EventHandler OnStop;

       
    }
}
