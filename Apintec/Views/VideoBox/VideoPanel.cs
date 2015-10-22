using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Apintec.Modules.Cameras;

namespace Apintec.views.VideoBox
{
    public partial class VideoPanel : UserControl
    {
        private Point _currPosition;
        private Point _offset = new Point(0 , 0);
        private Bitmap _srcImg;
        private Size _size = new Size();
        private double _zoomDelta = 0.1;
        private int newWidth = 640;
        private int newHeight = 480;
        private bool _initOne = false;
        private VideoAdapter _cameraAdapter;
        private const int FrameBufferSize = 25;
        public List<Bitmap> _frameBuffer { get; internal set; }
        

        public VideoAdapter CameraAdap
        {
            get { return _cameraAdapter; }
         //   set { _cameraAdapter = value; }
          
        }

        public VideoPanel()
        {
            InitializeComponent();
            panelMain.MouseWheel += PanelMain_MouseWheel;
            _cameraAdapter = new VideoAdapter();
            _frameBuffer = new List<Bitmap>();
            panelMain.Disposed += PanelMain_Disposed;
            _cameraAdapter.SnapProcess += CameraAdap_SnapProcess;
        }
        private void PanelMain_Disposed(object sender, EventArgs e)
        {
            try
            {
                _cameraAdapter.SnapProcess -= CameraAdap_SnapProcess;
                _cameraAdapter.UnBindingCamera();
            }
            catch (Exception)
            {

            }
            
        }

        private delegate void DelegateReDrawPanel(Point offset);
        private void CameraAdap_SnapProcess(object sender, EventArgs e)
        {
            CameraEventArgs args = e as CameraEventArgs;
            if (args.Img != null)
            {
                if(_frameBuffer.Count<FrameBufferSize)
                {
                    _frameBuffer.Add(args.Img);
                }
                else
                {
                    _frameBuffer.First().Dispose();
                    _frameBuffer.Remove(_frameBuffer.First());
                }
                
                _srcImg = _frameBuffer.Last();                
                if (!_initOne)
                {
                    newWidth = this._cameraAdapter.VideoPanel.Width;
                    newHeight = this._cameraAdapter.VideoPanel.Height;
                    _size = new Size(newWidth, newHeight);
                    _initOne = true;
                }

                if (IsHandleCreated)
                {
                    try
                    {
                        panelMain.Invoke(new DelegateReDrawPanel(ReDrawPanel), _offset);
                    }
                    catch(Exception exp)
                    {

                    }
                }
                    
                    
            }
        }

        private void PanelMain_MouseWheel(object sender, MouseEventArgs e)
        {
            
            if(_srcImg != null)
            {
                if (e.Delta > 0)
                {
                    if (newWidth > 10240)
                        return;
                    newWidth = Convert.ToInt32(newWidth + newWidth * _zoomDelta );
                    newHeight = Convert.ToInt32(newHeight + newHeight * _zoomDelta );
                    
                }
                if (e.Delta < 0)
                {
                    if (newWidth < 32 || newHeight < 32)
                        return;
                    newWidth = Convert.ToInt32(newWidth - newWidth * _zoomDelta);
                    newHeight = Convert.ToInt32(newHeight - newHeight * _zoomDelta);
                }

                _size = new Size(newWidth,  newHeight);
                ReDrawPanel(_offset);
            }
        }

    
        private void panelMain_Click(object sender, EventArgs e)
        {
            panelMain.Focus();
        }

        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            _currPosition = e.Location;
            panelMain.Cursor = Cursors.Hand;
        }

        private void ReDrawPanel(Point offset)
        {
            if (_srcImg == null)
                return;
            if (!CameraAdap.IsBind)
                return;
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            BufferedGraphics myBuffer = currentContext.Allocate(panelMain.CreateGraphics(), panelMain.DisplayRectangle);
            myBuffer.Graphics.Clear(panelMain.BackColor);
            Rectangle rec = new Rectangle(offset, _size);
            try
            {
                myBuffer.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                myBuffer.Graphics.DrawImage(_srcImg, rec);
                myBuffer.Render(panelMain.CreateGraphics());
                myBuffer.Dispose();
            }
            catch(Exception e)
            {

            }
        }
        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _offset.Offset(e.X - _currPosition.X, e.Y - _currPosition.Y);
                ReDrawPanel(_offset);
                _currPosition = e.Location;
            }
        }

        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {
            panelMain.Cursor = Cursors.Default;
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            ReDrawPanel(_offset);
        }
    }
}
