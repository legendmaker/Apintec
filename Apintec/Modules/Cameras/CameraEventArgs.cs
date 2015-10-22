using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Cameras
{
    public class CameraEventArgs:EventArgs
    {
        private Bitmap _img;

        public Bitmap Img
        {
            get { return _img; }
            protected set { _img = value; }
        }
        public CameraEventArgs()
        {

        }
        public CameraEventArgs(Bitmap img)
        {
            Img = img;
        }
    }
}
