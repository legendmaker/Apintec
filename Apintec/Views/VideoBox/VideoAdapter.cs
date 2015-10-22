using System;
using System.Reflection;
using Apintec.Modules.Cameras;
using System.Drawing;
using Apintec.Core.APCoreLib;

namespace Apintec.views.VideoBox
{
    public delegate void DelegateSnapProcess(object sender, EventArgs e);
    public class VideoAdapter
    { 
        private VideoPanel _videoPanel;
        private Camera _cameraModule;
        public bool IsBind { get; protected set; }

        public VideoPanel VideoPanel
        {
            get
            {
                return _videoPanel;
            }
        }

        public Camera CameraModule
        {
            get
            {
                return _cameraModule;
            }
        }

        public event DelegateSnapProcess SnapProcess;

        public VideoAdapter()
        {
            IsBind = false;
        }
        public bool BindingCamera(VideoPanel videoPanel, Camera cameraModule)
        {
            bool isOk;
            _videoPanel = videoPanel;
            _cameraModule = cameraModule;
            
            try
            {
                isOk = cameraModule.Open();
                if(isOk!=true)
                {
                    throw new APXExeception(
                        String.Format("Camera {1} Open Fail.", (cameraModule as Camera).Module));
                }

                isOk = cameraModule.Start();

                if (!isOk)
                {
                    throw new APXExeception(
                         String.Format("Camera {1} Start Fail.", (cameraModule as Camera).Module));
                }

                cameraModule.OnSnap += CameraModule_OnSnap;
            }
            catch (Exception e)
            {
                throw new APXExeception(
                    String.Format("{0} Binding Camera {1} Fail.",videoPanel.Name,(_cameraModule as Camera).Module));
            }
            IsBind = true;
            return true;
        }

        public bool UnBindingCamera()
        {
            bool isOk;
            IsBind = false;
            if (CameraModule == null)
                return true;
            try
            {
                _cameraModule.OnSnap -= CameraModule_OnSnap;
                VideoPanel._frameBuffer.Add(new Bitmap(640, 512));
                isOk = CameraModule.Close();
                if (isOk != true)
                {
                    throw new APXExeception(
                       String.Format("Camera {0} Close Fail.", _cameraModule.Module));
                }
            }
            catch
            {
                throw new APXExeception(
                    String.Format("{0} UnBinding Camera {1} Fail.", _videoPanel.Name, (_cameraModule as Camera).Module));
            }
            
            return true;
        }

        private void RemoveEvent<T>(T obj, string name)
        {
            Delegate[] invokeList = GetObjectEventList(obj, name);
            if (invokeList == null)
                return;
            foreach (Delegate del in invokeList)
            {
                typeof(T).GetEvent(name).RemoveEventHandler(obj, del);
            }
        }

        private Delegate[] GetObjectEventList(object obj, string name)
        {
            FieldInfo _Field = obj.GetType().GetField(name, BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (_Field == null)
            {
                return null;
            }
            object _FieldValue = _Field.GetValue(obj);
            if (_FieldValue != null && _FieldValue is Delegate)
            {
                Delegate _ObjectDelegate = (Delegate)_FieldValue;
                return _ObjectDelegate.GetInvocationList();
            }
            return null;
        }

        private void CameraModule_OnSnap(object sender, EventArgs e)
        {
            if(SnapProcess!=null)
            {
                SnapProcess(sender, e);
            }
                
        }
    }
}
