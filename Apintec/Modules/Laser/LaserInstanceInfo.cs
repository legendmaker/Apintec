using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser
{
    public class LaserInstanceInfo
    {
        public LaserInfo Info { get; internal set; }
        public Type Type { get; set; }
        public Laser Instance()
        {
            return (Activator.CreateInstance(Type, Info)) as Laser;
        }
        public LaserInstanceInfo()
        {
        }
        public LaserInstanceInfo(LaserInfo info, Type type)
        {
            Info = info;
            Type = type;
        }
    }
}
