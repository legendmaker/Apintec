using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apintec.Communiction.APXCom;

namespace Apintec.Modules.Robots
{
    public abstract class Robot:Hardware
    {
        private Coordinate _position = new Coordinate();
        public virtual Coordinate Position
        {
            get
            {
                return _position;
            }
            protected set
            {
                RaisedOnPositionUpdate();
                _position = value;
            }
        }
        public virtual IXCom ComModule { get; set; }

        public virtual bool BindComModule(IXCom comModule)
        {
            if(comModule!=null)
            {
                ComModule = comModule;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual event EventHandler OnPositionUpdate;
        protected virtual void RaisedOnPositionUpdate()
        {
            if(OnPositionUpdate!=null)
            {
                OnPositionUpdate(this, new EventArgs());
            }
        }

    }
}
