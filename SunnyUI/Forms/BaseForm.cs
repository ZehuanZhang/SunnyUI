using SunnyUI.WindowUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SunnyUI.Forms
{
    [ToolboxBitmap(typeof(Form))]
    public partial class BaseForm : Form
    {
        private int _CornerRadius = 5;
        
        public BaseForm()
            : base()
        {
            //InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            ResetRegion();
            InitializeControlBoxInfo();
            KeyPreview = true;
        }

        public int CornerRadius
        {
            get { return _CornerRadius; }
            set
            {
                _CornerRadius = value > 0 ? value : 0;
                base.Invalidate();
            }
        }
        protected Color rectColor = UIColor.Blue;

        private void DrawFormBorder(Graphics g)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return;
            }

            Rectangle rect = new Rectangle(0, 0, this.Width - 4, this.Height - 4);
            RoundRectangle roundRect = new RoundRectangle(rect, new CornerRadius(this.CornerRadius));
            //GDIHelper.DrawPathBorder(g, roundRect);
            using (GraphicsPath path = this._CornerRadius == 0 ? roundRect.ToGraphicsBezierPath() : roundRect.ToGraphicsArcPath())
            {
                using (Pen pen = new Pen(rectColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }
        /// <summary>
        /// 绘制窗体背景
        /// </summary>
        /// <param name="g">The g.</param>
        /// User:Ryan  CreateTime:2012-8-3 22:22.
        private void DrawFormBackGround(Graphics g)
        {
            //Rectangle rect = new Rectangle(0, 0, this.Width - 2, this.Height - 2);
            //if (SkinManager.CurrentSkin.BackGroundImageEnable)
            //{
            //    GDIHelper.DrawImage(g, rect, SkinManager.CurrentSkin.BackGroundImage, SkinManager.CurrentSkin.BackGroundImageOpacity);
            //    //GDIHelper.DrawImage(g, rect, SkinManager.CurrentSkin.BackGroundImage);
            //}
            //else
            //{
            //    GDIHelper.FillRectangle(g, rect, SkinManager.CurrentSkin.ThemeColor);
            //}
        }
        public static void InitializeGraphics(Graphics g)
        {
            if (g != null)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            InitializeGraphics(g);
            DrawFormBorder(g);
        }
        protected override void OnCreateControl()
        {
            ResetRegion();
            base.OnCreateControl();
        }
        private Rectangle sizeGripRectangle = new Rectangle();
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Created)
            {
                ResetRegion();
                Invalidate();
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.ProcessMouseMove(e.Location);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.ProcessMouseDown(e.Location);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.ProcessMouseUp(e.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ProcessMouseLeave(PointToClient(MousePosition));
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                this.Close();
            }
            base.OnKeyPress(e);
        }
        protected override void SetClientSizeCore(int x, int y)
        {
            _inPosChanged = true;
            base.SetClientSizeCore(x, y);
            _inPosChanged = false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowMessages.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    break;
                case (int)WindowMessages.WM_NCPAINT:
                    m.Result = new IntPtr(1); break;
                case (int)WindowMessages.WM_NCACTIVATE:
                    m.Result = new IntPtr(1); break;
                case (int)WindowMessages.WM_NCCALCSIZE:
                    {
                        int wparam = m.WParam.ToInt32();
                        NCCALCSIZE_PARAMS nccp = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                        if (wparam > 0)
                        {
                            // 重新计算客户端、非客户端大小
                            // 输入为
                            // rgrc1 - 位移后窗体位置
                            // rgrc2 - 位移前窗体位置
                            // rgrc3 - 位移前客户区位置
                            // 返回为
                            // rgrc1 - 位移后客户区位置
                            // rgrc2 - 位移后窗体位置
                            // rgrc3 - 位移前有效窗体位置（客户区）
                            RECT BArea = nccp.rgrc1; 
                            RECT BCArea = nccp.rgrc1; 
                            RECT ACArea = nccp.rgrc3;
                            // 若当前处于最大化模式
                            if (this.WindowState == FormWindowState.Maximized)
                            {
                                Rectangle rect = Screen.GetWorkingArea(this);
                                BArea = new RECT(rect);
                                BCArea = BArea;
                            }
                            else if (WindowState == FormWindowState.Normal)
                            {
                                //BArea.right += 1;
                                //nccp.rgrc1.top += 1;
                                //nccp.rgrc1.left += 1;
                                //BCArea.top += 1;
                                //BCArea.left += 1;
                                //BCArea.right -= 2;
                                //BCArea.bottom -= 2;
                                //BArea.right += 3;
                                BCArea.left += 1;
                                BCArea.right -= 1;
                                BCArea.top += 1;
                                BCArea.bottom -= 1;
                            }
                            nccp.rgrc1 = BCArea;
                            nccp.rgrc2 = BArea;
                            nccp.rgrc3 = ACArea;

                            //nccp.rgrc2.top = this.Top;
                            //nccp.rgrc2.left = this.Left;
                            //nccp.rgrc2.right = this.Left + this.Width;
                            //nccp.rgrc2.bottom = this.Top + this.Height;
                            Marshal.StructureToPtr(nccp, m.LParam, false);
                            //ResetRegion();
                            //m.Result = new IntPtr(1);
                        }
                    }
                    Invalidate();
                    break;
                case (int)WindowMessages.WM_WINDOWPOSCHANGED:
                    _inPosChanged = true;
                    base.WndProc(ref m);
                    _inPosChanged = false;
                    break;
                case (int)WindowMessages.WM_GETMINMAXINFO:
                    WmMinMaxInfo(ref m);
                    break;
                case (int)WindowMessages.WM_LBUTTONDBLCLK:
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        private void ResetRegion()
        {
            if (base.Region != null)
            {
                base.Region.Dispose();
            }
            int rgn = Win32.CreateRoundRectRgn(0, 0,
                this.Size.Width - 2 * _BorderWidth, this.Size.Height - 2 * _BorderWidth,
                this._CornerRadius - 2, this._CornerRadius - 2);
            Win32.SetWindowRgn(this.Handle, rgn, true);
        }

        #region fields

        private EnumControlState _MinBoxState;

        private EnumControlState _MaxBoxState;

        private EnumControlState _CloseBoxState;

        #endregion

        #region Initializes

        /// <summary>
        /// Initializes the control box info.
        /// </summary>
        /// User:Ryan  CreateTime:2012-8-3 15:59.
        private void InitializeControlBoxInfo()
        {
            this._MinBoxState = EnumControlState.Default;
            this._MaxBoxState = EnumControlState.Default;
            this._CloseBoxState = EnumControlState.Default;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the state of the min box.
        /// </summary>
        /// <value>The state of the min box.</value>
        /// User:Ryan  CreateTime:2012-8-3 15:59.
        private EnumControlState MinBoxState
        {
            get { return this._MinBoxState; }
            set
            {
                this._MinBoxState = value;
            }
        }

        private EnumControlState MaxBoxState
        {
            get { return this._MaxBoxState; }
            set
            {
                this._MaxBoxState = value;
            }
        }

        private EnumControlState CloseBoxState
        {
            get { return this._CloseBoxState; }
            set
            {
                this._CloseBoxState = value;
            }
        }
        #endregion

        #region ControlBox Properties

        private Rectangle CloseBoxRect = new Rectangle();
        private Rectangle MinimizeBoxRect = new Rectangle();
        private Rectangle MaximizeBoxRect = new Rectangle();
        private Rectangle CaptionRect
        {
            get
            {
                return new Rectangle(0, 0, Width, _CaptionHeight);
            }
        }
        private Rectangle LogoRect = new Rectangle();
        private Size _LogoSize = new Size();
        private bool _IsResizeable = true;
        private int _CaptionHeight = 28;
        private int _BorderWidth = 1;
        /// <summary>
        /// 边距
        /// </summary>
        private Point _Offset = new Point(3, 0);

        #endregion
        bool _inPosChanged;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.Style |= (int)(WindowStyle.WS_THICKFRAME);
                    if (ControlBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_SYSMENU;
                    }

                    if (MinimizeBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX;
                    }

                    if (!MaximizeBox)
                    {
                        cp.Style &= ~(int)WindowStyle.WS_MAXIMIZEBOX;
                    }

                    if (this._inPosChanged)
                    {
                        cp.Style &= ~((int)(
                            WindowStyle.WS_THICKFRAME |
                            WindowStyle.WS_SYSMENU |
                            WindowStyle.WS_CAPTION |
                            WindowStyle.WS_BORDER|
                            WindowStyle.WS_DLGFRAME));
                        cp.ExStyle &= ~((int)WindowStyleEx.WS_EX_DLGMODALFRAME |
                            (int)WindowStyleEx.WS_EX_WINDOWEDGE);
                    }
                }

                return cp;
            }
        }
        #region ControlBox methods

        /// <summary>
        /// 处理鼠标移动
        /// </summary>
        /// <param name="p">The Point.</param>
        /// User:Ryan  CreateTime:2011-07-28 10:44.
        protected virtual void ProcessMouseMove(Point p)
        {
            this.CloseBoxState = EnumControlState.Default;
            this.MaxBoxState = EnumControlState.Default;
            this.MinBoxState = EnumControlState.Default;
            if (this.CloseBoxRect.Contains(p))
            {
                this.CloseBoxState = EnumControlState.HeightLight;
            }

            if (this.MinimizeBoxRect.Contains(p))
            {
                this.MinBoxState = EnumControlState.HeightLight;
            }

            if (this.MaximizeBoxRect.Contains(p))
            {
                this.MaxBoxState = EnumControlState.HeightLight;
            }

            this.Invalidate(this.CaptionRect);
        }

        /// <summary>
        /// 处理鼠标按下
        /// </summary>
        /// <param name="p">The Point.</param>
        /// User:Ryan  CreateTime:2011-07-28 10:44.
        protected virtual void ProcessMouseDown(Point p)
        {
            Rectangle closeRect = this.CloseBoxRect;
            Rectangle maxRect = this.MaximizeBoxRect;
            Rectangle minRect = this.MinimizeBoxRect;
            if (!closeRect.IsEmpty && closeRect.Contains(p))
            {
                this.CloseBoxState = EnumControlState.Focused;
            }

            if (!maxRect.IsEmpty && maxRect.Contains(p))
            {
                this.MaxBoxState = EnumControlState.Focused;
            }

            if (!minRect.IsEmpty && minRect.Contains(p))
            {
                this.MinBoxState = EnumControlState.Focused;
            }

            this.Invalidate(this.CaptionRect);
        }

        /// <summary>
        /// 处理鼠标离开
        /// </summary>
        /// <param name="p">The Point.</param>
        /// User:Ryan  CreateTime:2011-07-28 10:44.
        protected virtual void ProcessMouseUp(Point p)
        {
            Rectangle closeRect = this.CloseBoxRect;
            Rectangle maxRect = this.MaximizeBoxRect;
            Rectangle minRect = this.MinimizeBoxRect;
            if (!closeRect.IsEmpty && closeRect.Contains(p))
            {
                base.Close();
                this.CloseBoxState = EnumControlState.Default;
            }

            if (!maxRect.IsEmpty && maxRect.Contains(p))
            {
                FormWindowState fs = FormWindowState.Normal;
                switch (base.WindowState)
                {
                    case FormWindowState.Maximized:
                        fs = FormWindowState.Normal;
                        break;
                    case FormWindowState.Normal:
                    default:
                        fs = FormWindowState.Maximized;
                        break;
                }

                base.WindowState = fs;
                this.MaxBoxState = EnumControlState.Default;
            }

            if (!minRect.IsEmpty && minRect.Contains(p))
            {
                base.WindowState = FormWindowState.Minimized;
                this.MinBoxState = EnumControlState.Default;
            }

            this.Invalidate(this.CaptionRect);
        }

        /// <summary>
        /// 处理鼠标离开
        /// </summary>
        /// <param name="p">The Point.</param>
        /// User:Ryan  CreateTime:2011-07-28 10:44.
        protected virtual void ProcessMouseLeave(Point p)
        {
            Rectangle closeRect = this.CloseBoxRect;
            Rectangle maxRect = this.MaximizeBoxRect;
            Rectangle minRect = this.MinimizeBoxRect;
            if (!closeRect.IsEmpty)
            {
                this.CloseBoxState = EnumControlState.Default;
            }

            if (!maxRect.IsEmpty)
            {
                this.MaxBoxState = EnumControlState.Default;
            }

            if (!minRect.IsEmpty)
            {
                this.MinBoxState = EnumControlState.Default;
            }

            this.Invalidate(this.CaptionRect);
        }

        #endregion

        #region Message Methods

        #region WmNcHitTest

        /// <summary>
        /// 对窗体鼠标消息的处理
        /// </summary>
        /// <param name="m">windows窗体消息</param>
        /// User:Ryan  CreateTime:2011-07-28 10:22.
        protected virtual void WmNcHitTest(ref Message m)
        {
            int wparam = m.LParam.ToInt32();
            Point point = new Point(
                Win32.LOWORD(wparam), Win32.HIWORD(wparam));
            point = PointToClient(point);

            if (this.LogoRect.Contains(point))
            {
                m.Result = new IntPtr((int)NCHITTEST.HTSYSMENU);
                return;
            }

            ////调整窗体大小
            if (this._IsResizeable && this._CaptionHeight > 0)
            {
                int w = 5;
                if (point.X <= w && point.Y <= w)
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTTOPLEFT);
                    return;
                }

                if (point.X >= (base.Width - w) && point.Y <= w)
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTTOPRIGHT);
                    return;
                }

                if (point.X >= (base.Width - w) && point.Y >= (base.Height - w))
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTBOTTOMRIGHT);
                    return;
                }

                if (point.X <= w && point.Y >= (base.Height - w))
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTBOTTOMLEFT);
                    return;
                }

                if (point.Y <= w)
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTTOP);
                    return;
                }

                if (point.Y >= (base.Height - w))
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTBOTTOM);
                    return;
                }

                if (point.X <= w)
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTLEFT);
                    return;
                }

                if (point.X >= (base.Width - w))
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTRIGHT);
                    return;
                }
            }

            if (point.Y <= this._CaptionHeight)
            {
                if (!this.CloseBoxRect.Contains(point)
                    && !this.MaximizeBoxRect.Contains(point)
                    && !this.MinimizeBoxRect.Contains(point)
                    )
                {
                    m.Result = new IntPtr((int)NCHITTEST.HTCAPTION);
                    return;
                }
                else
                {
                    ////终于算是找着你了，都可以移动了
                    m.Result = new IntPtr((int)NCHITTEST.HTCLIENT);
                    return;
                }
            }

            if (this._CaptionHeight > 0)
            {
                m.Result = new IntPtr((int)NCHITTEST.HTCAPTION);
            }
        }
        #endregion

        #region WmMinMaxInfo

        /// <summary>
        /// 对窗体状态的控制
        /// </summary>
        /// <param name="m">The Message.</param>
        /// User:Ryan  CreateTime:2011-07-28 11:10.
        private void WmMinMaxInfo(ref Message m)
        {
            MINMAXINFO minMaxInfo = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
            if (MaximumSize != Size.Empty)
            {
                minMaxInfo.maxTrackSize = base.MaximumSize;
            }
            else
            {
                Rectangle rect = Screen.GetWorkingArea(this);
                minMaxInfo.maxPosition = new Point(rect.X, rect.Y);
                minMaxInfo.maxTrackSize = new Size(rect.Width, rect.Height);
            }

            if (MinimumSize != Size.Empty)
            {
                minMaxInfo.minTrackSize = base.MinimumSize;
            }
            else
            {
                minMaxInfo.minTrackSize = new Size(this.CloseBoxRect.Width + this.MinimizeBoxRect.Width + this.MaximizeBoxRect.Width + this._Offset.X * 2 + this._LogoSize.Width,
                    this._CaptionHeight);
            }

            Marshal.StructureToPtr(minMaxInfo, m.LParam, false);
        }

        #endregion

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "BaseForm";
            this.ResumeLayout(false);

        }
    }
}
