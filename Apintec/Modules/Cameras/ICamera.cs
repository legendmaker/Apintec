using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Cameras
{
    interface  ICamera:IHardware
    {
        bool Snap(out Bitmap dstImg);
        bool SnapShot();

        event EventHandler OnSnap;
        event EventHandler OnSnapShot;
    }
}
