using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using uEye.Defines;
using uEye.Types;

namespace Apintec.Modules.Cameras.Vendors
{
    public class UEyeGigE:Camera
    {
        private int[] _menIDArray;
        private uEye.Camera _camera;
        private int _curMemIndex = 0;
        private PixelFormat _pixelFormat;
        private bool _isSnapShot = false;
        private bool _isAcquisition = false;
        private const int BufferSize = 2;
        private List<Bitmap> _buffer = new List<Bitmap>();
        public override uint Index
        {
            get
            {
                return base.Index;
            }

            set
            {
                Status status;
                CameraInformation[] cameraInfo;
                try
                {
                    status = uEye.Info.Camera.GetCameraList(out cameraInfo);
                    if (status == uEye.Defines.Status.SUCCESS && cameraInfo.Length > value)
                    {
                        Close();
                        InitCameraInfo(cameraInfo[Index]);
                        base.Index = value;
                    }
                    else
                    {
                        throw new APXExeception(String.Format("Camera( total: {0}),  index({1}) out of range",
                            cameraInfo.Length, value));
                    }
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
        }

        public override event EventHandler OnClose;
        public override event EventHandler OnOpen;
        public override event EventHandler OnSnap;
        public override event EventHandler OnSnapShot;
        public override event EventHandler OnStart;
        public override event EventHandler OnStop;

        public UEyeGigE() : base()
        {
        }
        public UEyeGigE(uint index) : this()
        {
            Index = index;
        }

        private void InitCameraInfo(CameraInformation cameraInfo)
        {

            CameraID = cameraInfo.CameraID;
            DeviceID = cameraInfo.DeviceID.ToString();
            InUse = cameraInfo.InUse;
            Module = Name = cameraInfo.Model;
            SensorID = cameraInfo.SensorID;
            SerialNumber = cameraInfo.SerialNumber;
            Status = cameraInfo.Status;
        }
        private bool InitMemory()
        {
            uEye.Defines.Status status;
            this._menIDArray = new int[1];
            for (int i = 0; i < 1; i++)
            {
                int num2;
                status = this._camera.Memory.Allocate(out num2, false);
                if (status != uEye.Defines.Status.Success)
                {
                    throw new APXExeception(status.ToString());
                }
                this._menIDArray[i] = num2;
            }
            return true;
        }
        public override bool Close()
        {
            Status status;
            IsSnapStarted = false;
            if (!IsOpen)
                return true;
            try
            {
                if (_camera == null)
                    return true;
                if (_isAcquisition)
                {
                    status = _camera.Acquisition.Stop();
                    if (status != uEye.Defines.Status.Success)
                    {
                        throw new APXExeception(String.Format(
                            "{0}: {1}", Name, status.ToString()));
                    }
                }

                status = _camera.Exit();
                if (status != uEye.Defines.Status.Success)
                {
                    throw new APXExeception(String.Format(
                             "{0}: {1}", Name, status.ToString()));
                }
                RaiseOnCloseEvent();
                IsOpen = false;
                return true;

            }
            catch (Exception e)
            {
                throw new APXExeception(String.Format(
                 "{0}: {1}", Name, e.Message));
            }

        }

        public override bool Open()
        {
            uEye.Defines.Status status;
            _camera = new uEye.Camera();
            status = _camera.Init(Convert.ToInt32(CameraID));
            if (status != uEye.Defines.Status.SUCCESS)
            {
                throw new APXExeception(String.Format
                    ("Error Code: {0}", Convert.ToInt32(status)));
            }

            if (status != uEye.Defines.Status.Success)
            {
                if (status != uEye.Defines.Status.STARTER_FW_UPLOAD_NEEDED)
                {
                    throw new APXExeception(String.Format(
                        "{0}:{1}", Name, status.ToString()));

                }
                if (this._camera.Init(0x10000) != uEye.Defines.Status.Success)
                {
                    throw new APXExeception(String.Format(
                        "{0}:{1}", Name, status.ToString()));
                }
            }
            this._camera.Parameter.Load();

            if (!this.InitMemory())
            {
                return false;
            }

            this._camera.Memory.SetActive(this._menIDArray[this._curMemIndex]);
            this._camera.EventFrame += new EventHandler(this.onFrameEvent);
            if (this._camera.Acquisition.Capture() != uEye.Defines.Status.Success)
            {
                throw new APXExeception(String.Format("{0} Continous capture fail..", Name));
            }
            _isAcquisition = true;
            Bitmap bitmap = null;
            this._camera.Memory.ToBitmap(this._menIDArray[this._curMemIndex], out bitmap);
            VideoSize = bitmap.Size;
            this._pixelFormat = bitmap.PixelFormat;
            IsOpen = true;
            RaiseOnOpenEvent();
            return true;
        }

        private void onFrameEvent(object sender, EventArgs e)
        {
            this._camera.Memory.SetActive(this._menIDArray[0]);
            if (IsSnapStarted)
            {
                try
                {
                    {
                        SrcImg = new Bitmap(VideoSize.Width, VideoSize.Height, this._pixelFormat);
                        Bitmap srcImg;
                        _camera.Memory.ToBitmap(1, out srcImg);
                        SrcImg = srcImg;
                        lock(_buffer)
                        {
                            if (_buffer.Count < BufferSize)
                            {
                                _buffer.Add(SrcImg.Clone() as Bitmap);
                            }
                            else
                            {
                                _buffer.First().Dispose();
                                _buffer.Remove(_buffer.First());
                            }
                        }
                        if (IsSnapStarted)
                            RaiseOnSnapEvent(new CameraEventArgs(SrcImg.Clone() as Bitmap));
                    }
                    if (_isSnapShot)
                    {
                        _isSnapShot = false;
                        RaiseOnSnapShotEvent(new CameraEventArgs(SrcImg.Clone() as Bitmap));
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (SrcImg != null)
                        SrcImg.Dispose();
                }

                
            }
        }
        public override bool Snap(out Bitmap dstImg)
        {
            if (IsSnapStarted)
            {
                lock(_buffer)
                {
                    dstImg = _buffer.Last();
                }
                return true;
            }
            else
            {
                dstImg = null;
                return false;
            }
        }

        public override bool SnapShot()
        {
            if (!IsSnapStarted)
                return false;
            if (_isAcquisition)
            {
                _isSnapShot = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Start()
        {
            if (!IsOpen)
                return false;
            if (_isAcquisition)
            {
                IsSnapStarted = true;
                RaiseOnStartEvent();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Stop()
        {
            if (!IsOpen)
                return false;
            if (_isAcquisition)
            {
                IsSnapStarted = false;
                RaiseOnStopEvent();
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void RaiseOnSnapEvent(CameraEventArgs args)
        {
            if (OnSnap != null)
            {
                OnSnap(this, args);
            }
        }
        protected void RaiseOnSnapShotEvent(CameraEventArgs args)
        {
            if (OnSnapShot != null)
            {
                OnSnapShot(this, args);
            }
        }
        protected void RaiseOnOpenEvent()
        {
            if (OnOpen != null)
            {
                if (OnOpen != null)
                {
                    OnOpen(this, new EventArgs());
                }
            }
        }
        protected void RaiseOnCloseEvent()
        {
            if (OnClose != null)
            {
                if (OnClose != null)
                {
                    OnClose(this, new EventArgs());
                }
            }
        }
        protected void RaiseOnStartEvent()
        {
            if (OnStart != null)
            {
                OnStart(this, new EventArgs());
            }
        }
        protected void RaiseOnStopEvent()
        {
            if (OnStop != null)
            {
                OnStop(this, new EventArgs());
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
