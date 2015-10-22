using PylonC.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Apintec.Core.APCoreLib;

namespace Apintec.Modules.Cameras.Vendors
{
    public class BaslerPylon:Camera
    {
        private PylonBuffer<Byte> _imgBuf = null;

        PYLON_DEVICE_HANDLE _hDev;
        private Thread _snapThread;

        private bool _isSnapShot = false;
        private const int BufferSize = 2;
        private List<Bitmap> _buffer = new List<Bitmap>();
        private static Queue<PYLON_DEVICE_HANDLE> _hDevQueue = new Queue<PYLON_DEVICE_HANDLE>();
        private static uint _numDevices = 0;

        public override event EventHandler OnClose;
        public override event EventHandler OnOpen;
        public override event EventHandler OnSnap;
        public override event EventHandler OnSnapShot;
        public override event EventHandler OnStart;
        public override event EventHandler OnStop;

        static BaslerPylon()
        {
            Pylon.Initialize();
            _numDevices = Pylon.EnumerateDevices();
            if(_numDevices >0)
            {
                for (int i = 0; i < _numDevices; i++)
                {
                    _hDevQueue.Enqueue(Pylon.CreateDeviceByIndex(Convert.ToUInt32(i)));
                }
            }
        }
        public BaslerPylon() : base()
        {
            try
            {
                /* Get a handle for specific index.  */
                _hDev = _hDevQueue.Dequeue();
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            try
            {
                Pylon.DeviceOpen(_hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);
                InitDeviceInfo();
                if (Pylon.DeviceIsOpen(_hDev))
                {
                    Pylon.DeviceClose(_hDev);
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        private void InitDeviceInfo()
        {
            bool isAvail = false;
            string ipAddr;
            string subnetMask;
            string defaultGateway;
            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, "DeviceModelName");
            if (isAvail)
            {
                Name = Module = Pylon.DeviceFeatureToString(_hDev, "DeviceModelName");
            }

            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, Pylon.cPylonDeviceInfoVendorNameKey);
            if (isAvail)
            {
                Vendor = Pylon.DeviceFeatureToString(_hDev, Pylon.cPylonDeviceInfoVendorNameKey);
            }

            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, Pylon.cPylonDeviceInfoSerialNumberKey);
            if (isAvail)
            {
                SerialNumber = Pylon.DeviceFeatureToString(_hDev, Pylon.cPylonDeviceInfoSerialNumberKey);
            }

            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, Pylon.cPylonDeviceInfoAddressKey);
            if (isAvail)
            {
                Address = Pylon.DeviceFeatureToString(_hDev, Pylon.cPylonDeviceInfoAddressKey);
            }

            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, Pylon.cPylonDeviceInfoDeviceIDKey);
            if (isAvail)
            {
                DeviceID = Pylon.DeviceFeatureToString(_hDev, Pylon.cPylonDeviceInfoDeviceIDKey);
            }

            Pylon.GigEGetPersistentIpAddress(_hDev, out ipAddr, out subnetMask, out defaultGateway);
            IPAddress = ipAddr;

            isAvail = Pylon.DeviceFeatureIsReadable(_hDev, Pylon.cPylonDeviceInfoMacAddressKey);
            if (isAvail)
            {
                MacAddress = Pylon.DeviceFeatureToString(_hDev, Pylon.cPylonDeviceInfoMacAddressKey);
            }
            if (_hDev != null)
            {
                if (Pylon.DeviceIsOpen(_hDev))
                    InUse = true;
            }
        }
        public override bool Close()
        {
            if (!IsOpen)
                return true;
            if (CloseAll())
            {
                RaiseOnCloseEvent();
                return true;
            }
            else
                return false;
        }
        private bool CloseAll()
        {
            IsSnapStarted = false;
            IsOpen = false;
            try
            {
                if (_snapThread != null)
                {
                    _snapThread.Abort();
                }
                if (_hDev == null)
                    return true;
                if (_hDev.IsValid)
                {
                    /* ... Close and release the pylon device. */
                    if (Pylon.DeviceIsOpen(_hDev))
                    {
                        Pylon.DeviceClose(_hDev);
                    }
                    Pylon.DestroyDevice(_hDev);
                }
                IsOpen = false;
            }
            catch (Exception)
            {
                throw new APXExeception(GenApi.GetLastErrorMessage());
            }
            finally
            {
                //Pylon.DestroyDevice(_hDev);
                //Pylon.Terminate();
            }
            return true;
        }

        public override bool Open()
        {
            bool isAvail = false;
            if (_hDev == null)
            {
                return false;
            }
            if (IsOpen)
            {
                return true;
            }
            try
            {
                /* Before using the device, it must be opened. Open it for configuring
                parameters and for grabbing images. */
                Pylon.DeviceOpen(_hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                /* Set the pixel format to Mono8, where gray values will be output as 8 bit values for each pixel. */
                /* ... Check first to see if the device supports the Mono8 format. */
                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "EnumEntry_PixelFormat_Mono8");

                if (!isAvail)
                {
                    /* Feature is not available. */
                    throw new APXExeception(String.Format("{0} don't support Mono8 fomat", Name));
                }

                /* ... Set the pixel format to Mono8. */
                Pylon.DeviceFeatureFromString(_hDev, "PixelFormat", "Mono8");

                /* Disable acquisition start trigger if available. */
                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "EnumEntry_TriggerSelector_AcquisitionStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerSelector", "AcquisitionStart");
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerMode", "Off");
                }

                /* Disable frame burst start trigger if available */
                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "EnumEntry_TriggerSelector_FrameBurstStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerSelector", "FrameBurstStart");
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerMode", "Off");
                }

                /* Disable frame start trigger if available */
                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "EnumEntry_TriggerSelector_FrameStart");
                if (isAvail)
                {
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerSelector", "FrameStart");
                    Pylon.DeviceFeatureFromString(_hDev, "TriggerMode", "Off");
                }

                /* For GigE cameras, we recommend increasing the packet size for better 
                   performance. If the network adapter supports jumbo frames, set the packet 
                   size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                   to 1500. */
                /* ... Check first to see if the GigE camera packet size parameter is supported 
                    and if it is writable. */
                isAvail = Pylon.DeviceFeatureIsWritable(_hDev, "GevSCPSPacketSize");

                if (isAvail)
                {
                    /* ... The device supports the packet size feature. Set a value. */
                    Pylon.DeviceSetIntegerFeature(_hDev, "GevSCPSPacketSize", 1500);
                }
                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "Width");
                Size videoSize = new Size();
                if (isAvail)
                {
                    videoSize.Width = Convert.ToInt32(Pylon.DeviceGetIntegerFeatureMax(_hDev, "Width"));
                }

                isAvail = Pylon.DeviceFeatureIsAvailable(_hDev, "Height");

                if (isAvail)
                {
                    videoSize.Height = Convert.ToInt32(Pylon.DeviceGetIntegerFeatureMax(_hDev, "Height"));
                }

                VideoSize = videoSize;
            }
            catch (Exception e)
            {
                throw new APXExeception(GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail());
            }
            IsOpen = true;
            RaiseOnOpenEvent();
            return true;
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
            if (IsSnapStarted)
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
            if (_hDev == null)
            {
                throw new APXExeception(String.Format("{0} handle is null.", Name));
            }

            try
            {
                _snapThread = new Thread(SnapFunc);
                _snapThread.Start();
                IsSnapStarted = true;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            RaiseOnStartEvent();
            return true;
        }

        private void SnapFunc()
        {
            while (true)
            {
                PylonGrabResult_t grabResult = new PylonGrabResult_t();
                grabResult.Status = EPylonGrabStatus.Failed;
                /* Grab one single frame from stream channel 0. The 
                       camera is set to "single frame" acquisition mode.
                       Wait up to 500 ms for the image to be grabbed. 
                       If imgBuf is null a buffer is automatically created with the right size.*/

                try
                {
                    if (!Pylon.DeviceGrabSingleFrame(_hDev, 0, ref _imgBuf, out grabResult, 500))
                    {
                    }
                }
                catch(Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.Message));
                }

                /* Check to see if the image was grabbed successfully. */
                if (grabResult.Status == EPylonGrabStatus.Grabbed)
                {
                    /* Success. Perform image processing. */
                    /* Check if the image is compatible with the currently used bitmap. */
                    //if (IsCompatible(SrcImg, grabResult.SizeX, grabResult.SizeY, false))
                    //{
                    //    /* Update the bitmap with the image data. */
                    //    UpdateBitmap(SrcImg, _imgBuf.Array, grabResult.SizeX, grabResult.SizeY, false);
                    //}
                    //else /* A new bitmap is required. */
                    try
                    {
                        Bitmap src;
                        CreateBitmap(out src, grabResult.SizeX, grabResult.SizeY, false);
                        SrcImg = src;
                        UpdateBitmap(SrcImg, _imgBuf.Array, grabResult.SizeX, grabResult.SizeY, false);
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

                        if (_isSnapShot)
                        {
                            _isSnapShot = false;
                            RaiseOnSnapShotEvent(new CameraEventArgs(SrcImg.Clone() as Bitmap));
                        }
                    }
                    catch(Exception)
                    { }
                    finally
                    {
                        if (SrcImg != null)
                            SrcImg.Dispose();
                    }
                }
                else if (grabResult.Status == EPylonGrabStatus.Failed)
                {
                    if (SrcImg == null)
                    {
                        Bitmap src;
                        CreateBitmap(out src, VideoSize.Width, VideoSize.Height, false);
                        SrcImg = src;
                    }
                }
            }
        }
        private PixelFormat GetFormat(bool color)
        {
            return color ? PixelFormat.Format32bppRgb : PixelFormat.Format8bppIndexed;
        }

        /* Calculates the length of one line in byte. */
        private int GetStride(int width, bool color)
        {
            return color ? width * 4 : width;
        }

        /* Compares the properties of the bitmap with the supplied image data. Returns true if the properties are compatible. */
        private bool IsCompatible(Bitmap bitmap, int width, int height, bool color)
        {
            if (bitmap == null
                || bitmap.Height != height
                || bitmap.Width != width
                || bitmap.PixelFormat != GetFormat(color)
             )
            {
                return false;
            }
            return true;
        }

        /* Creates a new bitmap object with the supplied properties. */
        private void CreateBitmap(out Bitmap bitmap, int width, int height, bool color)
        {
            bitmap = new Bitmap(width, height, GetFormat(color));

            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }
        }

        /* Copies the raw image data to the bitmap buffer. */
        private void UpdateBitmap(Bitmap bitmap, byte[] buffer, int width, int height, bool color)
        {
            /* Check if the bitmap can be updated with the image data. */
            if (!IsCompatible(bitmap, width, height, color))
            {
                //                RaiseOnExceptionEvent(new apcorelib.ExceptionEventArgs("Cannot update incompatible bitmap."));
                throw new Exception("Cannot update incompatible bitmap.");
            }

            /* Lock the bitmap's bits. */
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            /* Get the pointer to the bitmap's buffer. */
            IntPtr ptrBmp = bmpData.Scan0;
            /* Compute the width of a line of the image data. */
            int imageStride = GetStride(width, color);
            /* If the widths in bytes are equal, copy in one go. */
            if (imageStride == bmpData.Stride)
            {
                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else /* The widths in bytes are not equal, copy line by line. This can happen if the image width is not divisible by four. */
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            /* Unlock the bits. */
            bitmap.UnlockBits(bmpData);
        }

        public override bool Stop()
        {
            IsSnapStarted = false;
            if (!IsOpen)
                return false;
            try
            {
                if (_snapThread != null)
                {
                    _snapThread.Abort();
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            RaiseOnStopEvent();
            return true;
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
 //           Pylon.Terminate();
        }
    }
}
