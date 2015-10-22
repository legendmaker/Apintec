using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apintec.Core.APCoreLib
{
    public class APXDoc
    {
        private XNamespace _nameSpace;

        public XNamespace NameSpace
        {
            get { return _nameSpace; }
            protected set { _nameSpace = value; }
        }

        private string _goldenElement;

        public string GoldenElement
        {
            get { return _goldenElement; }
            set { _goldenElement = value; }
        }



        private XDocument _doc;
        public XDocument Doc
        {
            get
            {
                return _doc;
            }

            protected set
            {
                _doc = value;
            }
        }

        private string _docName;
        public string DocName
        {
            get
            {
                return _docName;
            }

            protected set
            {
                _docName = value;
            }
        }



        private XElement _root;

        public XElement Root
        {
            get
            {
                return _root;
            }

            protected set
            {
                _root = value;
            }
        }

        private void Initialize()
        {
            NameSpace = Properties.Resources.CorpWebsite;
            GoldenElement = Properties.Resources.Corp;
        }
        public APXDoc()
        {
            Initialize();
        }
        public APXDoc(string docName)
        {
            Initialize();
            try
            {
                Doc = XDocument.Load(docName);
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            var query = Doc.Elements().Select(n => n.Name);
            if (!query.Contains(NameSpace + GoldenElement))
            {
                Doc = null;
                throw new APXExeception("Illegal xml document.");

            }
            else
            {
                DocName = docName;
                Root = Doc.Root;
            }
        }

        public bool Create(string docName)
        {
            try
            {
                Doc = new XDocument(new XDeclaration("1.0", "UTF-8", "NO"));
                XElement root = new XElement(NameSpace + GoldenElement);
                root.Add(new XElement(NameSpace + "Content"));
                Doc.Add(root);
                DocName = docName;
                Doc.Save(DocName);
                Root = root;
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }

            return true;
        }
        public bool Save()
        {
            if (Doc == null)
            {
                return false;
            }
            try
            {
                Doc.Save(DocName);
            }
            catch (Exception e)
            {
                throw new APXExeception(e.Message);
            }
            return true;
        }
    }
}
