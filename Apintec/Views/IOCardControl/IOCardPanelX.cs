using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apintec.views.IOCardControl
{
   
    public partial class IOCtrlElement: UserControl
    {
        public event EventHandler OnWriteIO;

        public Label LabelBit
        {
            get
            {
                return labelBit;
            }

            set
            {
                labelBit = value;
            }
        }
        public string  LabelDIOValue
        {
            get
            {
                return labelDIOValue.Text;
            }

            set
            {
                labelDIOValue.Text = value;
                if(labelDIOValue.Text == "1")
                {
                    labelDIOValue.BackColor = Color.Green;
                }
                else
                {
                    if (this.Name.Contains("DI"))
                    {
                        labelDIOValue.BackColor = Color.LightGray;
                    }
                    else
                    {
                        labelDIOValue.BackColor = Color.White;
                    }
                    
                }

            }
        }

        public int Bit { get; set; }

        public Label LabelDIO
        {
            get
            {
                return labelDIOValue;
            }

            set
            {
                labelDIOValue = value;
            }
        }
        public Label LabelFunction
        {
            get
            {
                return labelFunction;
            }

            set
            {
                labelFunction = value;
            }
        }

        public Size PanelSize
        {
            get
            {
                return this.Size;
            }
        }

        public IOCtrlElement()
        {
            InitializeComponent();
        }

        private void labelDIOValue_Click(object sender, EventArgs e)
        {
            EventArgs result = new EventArgs();
            if(OnWriteIO!=null)
            {
                OnWriteIO(this, result);
            }
        }
    }
}
