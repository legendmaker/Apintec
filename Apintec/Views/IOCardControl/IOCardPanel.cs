using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using Apintec.Modules.IOCards;
using Apintec.Core.APCoreLib;

namespace Apintec.views.IOCardControl
{
    public partial class IOCardPanel : UserControl
    {
        private IOCardManager _ioCardMgr;

        public IOCardManager IOCardMgr
        {
            get
            {
                return _ioCardMgr;
            }
        }

        private BitArray _diValue ;
        private BitArray _doValue = new BitArray(32);
        
        public BitArray DOValue
        {
            get
            {
                return _doValue;
            }
        }

        public IOCardPanel()
        {
            InitializeComponent();
        }      
        public void Initialize(IOCardManager ioCardMgr)
        {
            _ioCardMgr = ioCardMgr;
            if (_ioCardMgr == null)
            {
                return;
            }
            _diValue = new BitArray(_ioCardMgr.Card.Channels);
            _doValue = new BitArray(_ioCardMgr.Card.Channels);
            this.groupBoxDI.SuspendLayout();
            this.groupBoxDO.SuspendLayout();
            this.SuspendLayout();

            
            for (int i = 0; i < _ioCardMgr.Card.Channels*2; i++)
            {
                
                if (i < _ioCardMgr.Card.Channels)
                {
                    IOCtrlElement dio1 = new IOCtrlElement();
                    dio1.Location = new System.Drawing.Point(2, 20 + i * 18);
                    dio1.Name = _ioCardMgr.IOInfo[i].IOType.ToString() + _ioCardMgr.IOInfo[i].Bit.ToString();
                    dio1.Size = new System.Drawing.Size(150, 12);
                    dio1.TabIndex = 0;
                    dio1.Bit = _ioCardMgr.IOInfo[i].Bit;
                    dio1.LabelBit.Text = _ioCardMgr.IOInfo[i].Bit.ToString();
                    dio1.LabelDIOValue = "0";
                    dio1.LabelFunction.Text = _ioCardMgr.IOInfo[i].Function;
                    this.groupBoxDI.Controls.Add(dio1);
                }
                if(i > _ioCardMgr.Card.Channels -1)
                {
                    IOCtrlElement dio2 = new IOCtrlElement();
                    dio2.Location = new System.Drawing.Point(2, 20 + (i- _ioCardMgr.Card.Channels) * 18);
                    dio2.Name = _ioCardMgr.IOInfo[i].IOType.ToString() + _ioCardMgr.IOInfo[i].Bit.ToString();
                    dio2.Size = new System.Drawing.Size(150, 12);
                    dio2.TabIndex = 0;
                    dio2.Bit = _ioCardMgr.IOInfo[i].Bit;
                    dio2.LabelBit.Text = _ioCardMgr.IOInfo[i].Bit.ToString();
                    dio2.LabelDIO.BorderStyle = BorderStyle.Fixed3D;
                    dio2.LabelDIOValue = "0";
                    dio2.LabelFunction.Text = _ioCardMgr.IOInfo[i].Function;
                    dio2.OnWriteIO += Dio2_OnWriteIO;
                    this.groupBoxDO.Controls.Add(dio2);
                }
            }
            this.groupBoxDI.Size = new Size(160, (_ioCardMgr.Card.Channels + 1) * 18);
            this.groupBoxDO.Size = new Size(160, (_ioCardMgr.Card.Channels + 1) * 18);
            this.Size = new Size(groupBoxDI.Size.Width * 2 + 10, groupBoxDI.Size.Height + 10);
            this.groupBoxDI.Text = _ioCardMgr.IOInfo[0].IOType.ToString();
            this.groupBoxDO.Text = _ioCardMgr.IOInfo[_ioCardMgr.Card.Channels].IOType.ToString();

            this.groupBoxDI.ResumeLayout(false);
            this.groupBoxDO.ResumeLayout(false);
            this.ResumeLayout(false);
            if (OpenCard())
            {
                this.timerDIRead.Enabled = true;
                InitDOValue();
            }
            else
            {
                this.groupBoxDO.BackColor = Color.Red;
                this.groupBoxDI.BackColor = Color.Red;
            }
        }

        private bool OpenCard()
        {
            bool isOk;
            if(_ioCardMgr.Card.IsOpen)
            {
                _ioCardMgr.Card.OnWrite += IOCards_OnWrite;
                return true;
            }
            isOk = _ioCardMgr.Card.Open();
            if (!isOk)
            {
                MessageBox.Show(String.Format("{0} Open fail.", _ioCardMgr.Card.Module));
                return false;
            }
            else
            {
                _ioCardMgr.Card.OnWrite += IOCards_OnWrite;
                return true;
            }
        }

        private delegate void DelegateInitInitDOValue();
        private void IOCards_OnWrite(object sender, EventArgs e)
        {
            IOCardEventArgs result = e as IOCardEventArgs;
            if(IsHandleCreated)
                this.BeginInvoke(new DelegateInitInitDOValue(InitDOValue));
            if(result!=null)
            {
                if(!result.IsOK)
                {
                    throw new APXExeception("DO write Fail.");
                }
            }
        }

        private void Dio2_OnWriteIO(object sender, EventArgs e)
        {
            IOCtrlElement ioCtrlElem = sender as IOCtrlElement;
            if(ioCtrlElem!=null)
            {
                if (ioCtrlElem.LabelDIOValue == "1")
                {
                    _doValue[ioCtrlElem.Bit] = false;
                    if(_ioCardMgr.Card.Write(PortType.DO, 0, _doValue))
                    {
                        ioCtrlElem.LabelDIOValue = "0";
                    }
                    else
                    {
                        _doValue[ioCtrlElem.Bit] = true;
                        MessageBox.Show(String.Format("{0} Set fail.", ioCtrlElem.Name));
                        throw new APXExeception(String.Format("{0} Set fail.", ioCtrlElem.Name));
                    }
                    return;
                }

                if(ioCtrlElem.LabelDIOValue == "0")
                {
                    _doValue[ioCtrlElem.Bit] = true;
                    if (_ioCardMgr.Card.Write(PortType.DO, 0, _doValue))
                    {
                        ioCtrlElem.LabelDIOValue = "1";
                    }
                    else
                    {
                        _doValue[ioCtrlElem.Bit] = false;
                        MessageBox.Show(String.Format("{0} Set fail.", ioCtrlElem.Name));
                        throw new APXExeception(String.Format("{0} Set fail.", ioCtrlElem.Name));
                    }
                    return;
                }
            }
            
        }

        private void InitDOValue()
        {
            bool isOk;
            if (_ioCardMgr != null && _ioCardMgr.Card != null)
            {
                isOk = _ioCardMgr.Card.Read(PortType.DO, 0, out _doValue);
                if (!isOk)
                {
                    this.groupBoxDO.BackColor = Color.Red;
                    throw new APXExeception(String.Format("{0} DI Read Fail.", _ioCardMgr.Card.Module));
                }
                else
                {
                    this.groupBoxDO.BackColor = Control.DefaultBackColor;
                    foreach (var item in this.groupBoxDO.Controls)
                    {
                        IOCtrlElement ioElem = item as IOCtrlElement;
                        if (ioElem == null)
                            continue;
                        for (int i = 0; i < _ioCardMgr.Card.Channels; i++)
                        {
                            if (ioElem.Name == ("DO" + i.ToString()))
                            {
                                if (_doValue[i] == true)
                                {
                                    (item as IOCtrlElement).LabelDIOValue = "1";
                                }
                                else
                                {
                                    (item as IOCtrlElement).LabelDIOValue = "0";
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public void Release()
        {
            timerDIRead.Enabled = false;
        } 
        private void timerDIRead_Tick(object sender, EventArgs e)
        {
            bool isOk;
            if(_ioCardMgr!=null && _ioCardMgr.Card!=null)
            {
                isOk = _ioCardMgr.Card.Read(PortType.DI, 0, out _diValue);
                if(!isOk)
                {
                    this.groupBoxDI.BackColor = Color.Red;
                    APXlog.Write(APXlog.BuildLogMsg(String.Format("{0} DI Read Fail.", _ioCardMgr.Card.Module)));
                }
                else
                {
                    this.groupBoxDI.BackColor = Control.DefaultBackColor;
                    foreach (var item in this.groupBoxDI.Controls)
                    {
                        IOCtrlElement ioElem = item as IOCtrlElement;
                        if (ioElem == null)
                            continue;
                        for (int i = 0; i < _ioCardMgr.Card.Channels; i++)
                        {
                            if (ioElem.Name == ("DI" + i.ToString()))
                            {
                                if (_diValue[i] == true)
                                {
                                    (item as IOCtrlElement).LabelDIOValue = "1";
                                }
                                else
                                {
                                    (item as IOCtrlElement).LabelDIOValue = "0";
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
