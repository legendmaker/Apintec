using Apintec.Communiction.APXCom.Instances;
using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apintec.Communiction.APXCom
{
    public class XComManager
    {

        public string Driver { get; protected set; }

        private APXDoc _xComCfg;

        public APXDoc XComCfg
        {
            get { return _xComCfg; }
            protected set { _xComCfg = value; }
        }
        private const string IOCardCfgFile = "XCom.xml";
        public IXCom ComModule { get; protected set; }

        public XSerialParameter SerialPara { get; protected set; }


        public XComManager()
        {
            if (!File.Exists(IOCardCfgFile))
            {
                try
                {
                    XComCfg = new APXDoc();
                    XComCfg.Create(IOCardCfgFile);
                    XComCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                XComCfg = new APXDoc(IOCardCfgFile);
            }
            Load();

        }
        private bool Load()
        {
            if (XComCfg == null)
                return false;
            try
            {
                var queryIOCard = XComCfg.Doc.Descendants(XComCfg.NameSpace + "XCom").Attributes().Select(n => new { n.Name, n.Value });
                foreach (var item in queryIOCard)
                {
                    switch (item.Name.LocalName)
                    {
                        case "Driver":
                            Driver = item.Value;
                            break;
                        default:
                            break;
                    }
                }
                if(Driver == "XSerialPort")
                {
                    SerialPara = new XSerialParameter();
                    foreach (var item in queryIOCard)
                    {
                        switch (item.Name.LocalName)
                        {
                            case "PortName":
                                SerialPara.PortName = item.Value;
                                break;
                            case "Baudrate":
                                SerialPara.Baudrate = int.Parse(item.Value);
                                break;
                            case "Parity":
                                SerialPara.Parity = (Parity)Enum.Parse(typeof(Parity), item.Value);
                                break;
                            case "DataBits":
                                SerialPara.DataBits = int.Parse(item.Value);
                                break;
                            case "StopBits":
                                SerialPara.StopBits = (StopBits)Enum.Parse(typeof(StopBits), item.Value);
                                break;
                            case "Handshake":
                                SerialPara.Handshake = (Handshake)Enum.Parse(typeof(Handshake), item.Value);
                                break;
                            default:
                                break;
                        }
                    }
                    try
                    {
                        Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                            + "." + "Instances" + "." + Driver, true, true);
                        ComModule = (Activator.CreateInstance(type, SerialPara)) as IXCom;
                    }
                    catch (Exception e)
                    {
                        throw new APXExeception(e.ToString());
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }

            return true;
        }
      
        public void Dispose()
        {
            ComModule.Disconnect();
        }
    }
}
