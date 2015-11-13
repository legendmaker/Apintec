using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser
{
    public interface ILaser : IHardware 
    {
        bool Run(LaserCommand cmd, params object[] para);
    }
}
