using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Cameras
{
    public abstract class Camera : CameraInfo, ICamera
    {
        public abstract event EventHandler OnSnap;
        public abstract event EventHandler OnSnapShot;
        public abstract event EventHandler OnOpen;
        public abstract event EventHandler OnClose;
        public abstract event EventHandler OnStart;
        public abstract event EventHandler OnStop;

        public Camera()
        {
            VideoSize = new Size(640, 320);
            SrcImg = new Bitmap(VideoSize.Width, VideoSize.Height);
 //           Index = 0;
        }
        public abstract bool Open();
        public abstract bool Close();
        public abstract bool Start();
        public abstract bool Stop();

        public abstract bool Snap(out Bitmap dstImg);
        public abstract bool SnapShot();
        public abstract void Dispose();
    }
}
