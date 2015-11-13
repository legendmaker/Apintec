using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Apintec.Modules.Cameras
{
    public class CameraManager
    { 

        public string Driver { get; protected set; }

        private APXDoc _cameraCfg;

        public APXDoc CameraCfg
        {
            get { return _cameraCfg; }
            protected set { _cameraCfg = value; }
        }
        private const string CamerasCfgFile = "Cameras.xml";
        public List<CameraInstanceInfo> Cameras { get; protected set; }

        public CameraManager()
        {
            if (!File.Exists(CamerasCfgFile))
            {
                try
                {
                    CameraCfg = new APXDoc();
                    CameraCfg.Create(CamerasCfgFile);
                    CameraCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                CameraCfg = new APXDoc(CamerasCfgFile);
            }
            Cameras = new List<CameraInstanceInfo>();
            try
            {
                Load();
                ReinInstance();
            }
            catch(Exception e)
            {
                APXlog.Write(APXlog.BuildLogMsg(e.Message));
            }
        }
        public void ReinInstance()
        {
            foreach (var item in Cameras)
            {
                if(!item.IsInstance)
                {
                    try
                    {
                        Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                            + "." + "Vendors" + "." + item.Vendor, true, true);
                        item.Instance = (Activator.CreateInstance(type)) as Camera;
                        item.IsInstance = true;
                        item.Instance.Sequence = -1;
                        if(!String.IsNullOrEmpty( item.Instance.IPAddress))
                        {
                            foreach (var caminfo in Cameras)
                            {
                                if (caminfo.IpAddress == item.Instance.IPAddress)
                                    item.Instance.Sequence = caminfo.Sequence;
                            }
                        }
                        else
                        {
                            item.Instance.Sequence = item.Sequence;
                        }
                    }
                    catch (Exception e)
                    {
                        APXlog.Write(APXlog.BuildLogMsg( e.ToString()));
                    }
                }
            }
        }

        private bool Load()
        {
            string vendor = "";
            uint index = 0;
            int sequence = 0;
            string ipAddr = "";
            if (CameraCfg == null)
                return false;
            try
            {
                var queryVendor = CameraCfg.Doc.Descendants(CameraCfg.NameSpace + "Camera").Descendants().Select(n=>new { n.Name,n});
                foreach (var item in queryVendor)
                {
                    vendor = item.Name.LocalName;
                    for (XAttribute attr = item.n.FirstAttribute; attr!=null ; attr=attr.NextAttribute)
                    {
                        switch (attr.Name.LocalName)
                        {
                            case "Index":
                                index = uint.Parse(attr.Value);
                                break;
                            case "Sequence":
                                sequence = int.Parse(attr.Value);
                                break;
                            case "IPAddress":
                                ipAddr = attr.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    Cameras.Add(new CameraInstanceInfo(vendor, index, sequence, ipAddr));
                }

            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }

            return true;
        }
    }
   
}
