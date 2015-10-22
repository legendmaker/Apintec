using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Modules.Cameras
{
    public class CameraInfo:HardwareInfo
    {
        public virtual string Address { get; internal set; }
        public virtual long CameraID { get; internal set; }
        public virtual bool InUse { get; internal set; }
        public virtual string IPAddress { get; internal set; }
        public virtual string MacAddress { get; internal set; }
        public virtual long SensorID { get; internal set; }
        public virtual string SerialNumber { get; internal set; }
        public virtual long Status { get; internal set; }
        protected virtual Bitmap SrcImg { get; set; }
        public virtual uint Index { get; set; }
        public virtual int Sequence { get; set; }
        public virtual Size VideoSize { get; internal set; }
        public virtual bool IsSnapStarted { get; internal set; }
    }
}
