using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Apintec.Modules.IOCards
{
    public class IOCardManager
    {
        private IOCardInfo _cardInfo;
        public string Driver { get; protected set; }

        private APXDoc _ioCardCfg;

        public APXDoc IOCardCfg
        {
            get { return _ioCardCfg; }
            protected set { _ioCardCfg = value; }
        }
        private const string IOCardCfgFile = "IOCard.xml";
        public IOCard Card { get; protected set; }

        public List<IODesc> IOInfo { get; protected set; }
        public IOCardManager()
        {
            if (!File.Exists(IOCardCfgFile))
            {
                try
                {
                    IOCardCfg = new APXDoc();
                    IOCardCfg.Create(IOCardCfgFile);
                    IOCardCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                _ioCardCfg = new APXDoc(IOCardCfgFile);
            }

            IOInfo = new List<IODesc>();
            Load();

        }

        private bool ParseCardInfo()
        {
            string vendor ="";
            int cardType = 0;
            int channels = 0;
            int index = 0;
            if (IOCardCfg == null)
                return false;
            try
            {
                
                var queryIOCard = IOCardCfg.Doc.Descendants(IOCardCfg.NameSpace + "IOCard").Attributes().Select(n => new { n.Name, n.Value });

                foreach (var item in queryIOCard)
                {
                    switch (item.Name.LocalName)
                    {
                        case "Vendor":
                            vendor = item.Value;
                            break;
                        case "Driver":
                            Driver = item.Value;
                            break;
                        case "CardType":
                            cardType = Convert.ToInt32(item.Value);
                            break;
                        case "Channels":
                            channels = Convert.ToInt32(item.Value);
                            break;
                        case "Index":
                            index = Convert.ToInt32(item.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
            _cardInfo = new IOCardInfo(vendor,channels,cardType,index);
            return true;
        }

        private bool ParseIODesc()
        {
            PortType portType = new PortType();
            int bit = 0;
            string func = "";
            for (int i = 0; i < _cardInfo.Channels; i++)
            {
                var queryIO = IOCardCfg.Doc.Descendants(IOCardCfg.NameSpace + "DI").Descendants(IOCardCfg.NameSpace + "DI" + i.ToString())
                    .Attributes().Select(n => new { n.Name, n.Value });
                portType = PortType.DI;
                foreach (var item in queryIO)
                {
                    switch (item.Name.LocalName)
                    {
                        case "bit":
                            bit = Convert.ToInt32(item.Value);
                            break;
                        case "Function":
                            func = item.Value;
                            break;
                        default:
                            break;
                    }
                }
                IOInfo.Add(new IODesc(portType, bit, func));
            }

            for (int i = 0; i < _cardInfo.Channels; i++)
            {
                var queryIO = IOCardCfg.Doc.Descendants(IOCardCfg.NameSpace + "DO").Descendants(IOCardCfg.NameSpace + "DO" + i.ToString())
                    .Attributes().Select(n => new { n.Name, n.Value });
                portType = PortType.DO;
                foreach (var item in queryIO)
                {
                    switch (item.Name.LocalName)
                    {
                        case "bit":
                            bit = Convert.ToInt32(item.Value);
                            break;
                        case "Function":
                            func = item.Value;
                            break;
                        default:
                            break;
                    }
                }
                IOInfo.Add(new IODesc(portType, bit, func));
            }
            return true;
        }
        private bool Load()
        {
            if (!ParseCardInfo())
                return false;

            if (!ParseIODesc())
                return false;

            try
            {
                Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                    + "." + "Vendors" + "." + _cardInfo.Vendor + "." + Driver, true, true);
                Card = (Activator.CreateInstance(type, _cardInfo)) as IOCard;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
            return true;
        }
        public void Dispose()
        {
            Card.Close();
        }
    }
}
