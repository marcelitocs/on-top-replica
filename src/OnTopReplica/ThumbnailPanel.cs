using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WindowsFormsAero.Dwm;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using OnTopReplica.Native;

namespace OnTopReplica {

    /// <summary>
    /// Panel that hosts a DWM thumbnail.
    /// </summary>
    class ThumbnailPanel : Panel {

        //DWM Thumbnail stuff
        Thumbnail _thumbnail = null;

        //Labels
        WindowsFormsAero.ThemeLabel _labelGlass;

        /// <summary>
        /// Creates a new instance of the thumbnail panel.
        /// </summary>
        public ThumbnailPanel() {
            InitFormComponents();
        }

        private void InitFormComponents() {
            BackColor = Color.Black;

            //Themed Label
            _labelGlass = new WindowsFormsAero.ThemeLabel {
                Dock = DockStyle.Fill,
                ForeColor = SystemColors.ControlText,
                Location = Point.Empty,
                Size = ClientSize,
                Name = "labelGlass",
                Text = Strings.RightClick,
                TextAlign = HorizontalAlignment.Center,
                TextAlignVertical = VerticalAlignment.Center
            };
            this.Controls.Add(_labelGlass);
        }

        #region Properties and settings

        ThumbnailRegion _currentRegion;

        /// <summary>
        /// Gets or sets the region that is currently shown on the thumbnail. When set, also enables region constrain.
        /// </summary>
        public ThumbnailRegion SelectedRegion {
            get {
                return _currentRegion;
            }
            set {
                _currentRegion = value;
                _regionEnabled = (value != null);
                UpdateThubmnail();
            }
        }

        bool _regionEnabled = false;

        /// <summary>
        /// Gets or sets whether the thumbnail is constrained to a region or not.
        /// </summary>
        public bool ConstrainToRegion {
            get {
                return _regionEnabled;
            }
            set {
                if (_regionEnabled != value) {
                    _regionEnabled = value;
                    UpdateThubmnail();
                }
            }
        }

        /// <summary>
        /// Enables mouse regions drawing, simulating one first click on the panel at the current cursor's position.
        /// </summary>
        public void EnableMouseRegionsDrawingWithMouseDown() {
            if (DrawMouseRegions)
                return;

            var localCursor = this.PointToClient(Cursor.Position);

            DrawMouseRegions = true;
            OnMouseDown(new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, localCursor.X, localCursor.Y, 0));
        }

        bool _drawMouseRegions = false;

        /// <summary>
        /// Gets or sets whether the control is is "region drawing" mode and reports them via events.
        /// </summary>
        public bool DrawMouseRegions {
            get {
                return _drawMouseRegions;
            }
            set {
                //Set mode and reset region
                _drawMouseRegions = value;
                _drawingRegion = false;

                //Cursor change
                Cursor = (value) ? Cursors.Cross : Cursors.Default;

                //Refresh gui
                UpdateThubmnail();
                _labelGlass.Visible = !value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets the target opacity of the thumbnail, depending on the control's state.
        /// </summary>
        protected byte ThumbnailOpacity {
            get {
                return (_drawMouseRegions) ? (byte)130 : (byte)255;
            }
        }

        /// <summary>
        /// Gets or sets whether the control should report clicks made on the cloned thumbnail.
        /// </summary>
        public bool ReportThumbnailClicks {
            get;
            set;
        }

        /// <summary>
        /// Gets the thumbnail's size (in effectively thumbnailed pixels).
        /// </summary>
        /// <remarks>
        /// This size varies if the thumbnail has been cropped to a region.
        /// </remarks>
        public Size ThumbnailPixelSize {
            get {
                if (_thumbnail != null && !_thumbnail.IsInvalid) {
                    if (_regionEnabled) {
                        return _currentRegion.ComputeRegionSize(_thumbnail.GetSourceSize());
                    }
                    else {
                        //Thumbnail is not cropped, return full thumbnail source size
                        return _thumbnail.GetSourceSize();
                    }
                }
                else {
#if DEBUG
                    throw new InvalidOperationException(Strings.ErrorNoThumbnail);
#else
                    return Size.Empty;
#endif
                }
            }
        }

        /// <summary>
        /// Gets the thumbnailed window's original size.
        /// </summary>
        /// <remarks>
        /// This size is not influenced by the region cropping applied to the thumbnail.
        /// </remarks>
        public Size ThumbnailOriginalSize {
            get {
                if (_thumbnail != null && !_thumbnail.IsInvalid) {
                    return _thumbnail.GetSourceSize();
                }
                else {
#if DEBUG
                    throw new InvalidOperationException(Strings.ErrorNoThumbnail);
#else
                    return Size.Empty;
#endif
                }
            }
        }

        #endregion

        #region GUI event handling

        /// <summary>
        /// Overridden. Updates the thumbnail when the control is resized.
        /// </summary>
        protected override void OnResize(EventArgs eventargs) {
            base.OnResize(eventargs);
            UpdateThubmnail();
        }

        /// <summary>
        /// Overridden. Handles hit testing to make the panel transparent to mouse clicks when not in a special mode.
        /// </summary>
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            //Check whether this is a hit-test on "client" surface
            if (m.Msg == WM.NCHITTEST && m.Result.ToInt32() == HT.CLIENT) {
                //Check whether clicks must be reported
                if(!DrawMouseRegions && !ReportThumbnailClicks){
                    m.Result = new IntPtr(HT.TRANSPARENT);
                }
            }
        }

        #endregion

        #region Thumbnail interface

        /// <summary>
        /// Creates a new thumbnail of a certain window.
        /// </summary>
        /// <param name="handle">Handle of the window to clone.</param>
        /// <param name="region">Optional region.</param>
        public void SetThumbnailHandle(WindowHandle handle, ThumbnailRegion region) {
            Log.WriteDetails("Setting new thumbnail",
                "HWND {0}, region {1}", handle, region
            );

            if (_thumbnail != null && !_thumbnail.IsInvalid) {
                _thumbnail.Close();
                _thumbnail = null;
            }

            //Attempt to get top level Form from Control
            Form owner = this.TopLevelControl as Form;
            if (owner == null)
                throw new Exception("Internal error: ThumbnailPanel.TopLevelControl is not a Form.");

            _labelGlass.Visible = false;

            //Register new thumbnail, update regioning directly and refresh thumbnail
            _thumbnail = DwmManager.Register(owner, handle.Handle);
            _currentRegion = region;
            _regionEnabled = (region != null);
            UpdateThubmnail();
        }

        /// <summary>
        /// Disposes current thumbnail and enters stand-by mode.
        /// </summary>
        public void UnsetThumbnail() {
            Log.Write("Unsetting thumbnail");

            if (_thumbnail != null && !_thumbnail.IsInvalid) {
                _thumbnail.Close();
            }

            _thumbnail = null;
            _labelGlass.Visible = true;
        }

        /// <summary>
        /// Gets whether the control is currently displaying a thumbnail.
        /// </summary>
        public bool IsShowingThumbnail {
            get {
                return (_thumbnail != null && !_thumbnail.IsInvalid);
            }
        }

        int _padWidth = 0;
        int _padHeight = 0;
        Size _thumbnailSize;

        /// <summary>
        /// Updates the thumbnail options and the right-click label.
        /// </summary>
        private void UpdateThubmnail() {
            if (_thumbnail != null && !_thumbnail.IsInvalid){
                try {
                    //Get thumbnail size and attempt to fit to control, with padding
                    Size sourceSize = ThumbnailPixelSize;
                    _thumbnailSize = sourceSize.Fit(Size);
                    _padWidth = (Size.Width - _thumbnailSize.Width) / 2;
                    _padHeight = (Size.Height - _thumbnailSize.Height) / 2;

                    System.Diagnostics.Debug.WriteLine("Fitting {0} inside {1} as {2}. Padding {3},{4}.", sourceSize, Size, _thumbnailSize, _padWidth, _padHeight);

                    var target = new Rectangle(_padWidth, _padHeight, _thumbnailSize.Width, _thumbnailSize.Height);
                    Rectangle source = (_regionEnabled) ? _currentRegion.ComputeRegionRectangle(_thumbnail.GetSourceSize()) : new Rectangle(Point.Empty, _thumbnail.GetSourceSize());

                    _thumbnail.Update(target, source, ThumbnailOpacity, true, true);
                }
                catch {
                    //Any error updating the thumbnail forces to unset (handle may not be valid anymore)
                    UnsetThumbnail();
                    return;
                }
            }
        }

        #endregion

        #region Region drawing

        const int MinimumRegionSize = 1;

        //Set if currently drawing a window (first click/drag was initiated)
        bool _drawingRegion = false;
        //Set if drawing was suspended because the mouse left the control
        bool _drawingSuspended = false;
        Point _regionStartPoint;
        Point _regionLastPoint;

        /// <summary>
        /// Delegate for the RegionDrawn event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="region">The region that has been drawn.</param>
        public delegate void RegionDrawnHandler(object sender, ThumbnailRegion region);

        /// <summary>
        /// Fired when a region has been drawn by the user.
        /// </summary>
        public event RegionDrawnHandler RegionDrawn;

        /// <summary>
        /// Raises the RegionDrawn event.
        /// </summary>
        /// <param name="region">The region that has been drawn.</param>
        protected virtual void OnRegionDrawn(Rectangle region) {
            //Fix region if necessary (bug report by Gunter, via comment)
            if (region.Width < MinimumRegionSize)
                region.Width = MinimumRegionSize;
            if (region.Height < MinimumRegionSize)
                region.Height = MinimumRegionSize;

            var evt = RegionDrawn;
            if (evt != null)
                evt(this, new ThumbnailRegion(region));
        }

        /// <summary>
        /// Raises a RegionDrawn event, given a starting and an ending point of the drawn region.
        /// </summary>
        protected void RaiseRegionDrawn(Point start, Point end) {
            if (_thumbnailSize.Width < 1 || _thumbnailSize.Height < 1) //causes DivBy0
                return;

            //Compute bounds and clip to boundaries
            int left = Math.Min(start.X, end.X);
            int right = Math.Max(start.X, end.X);
            int top = Math.Min(start.Y, end.Y);
            int bottom = Math.Max(start.Y, end.Y);

            //Clip to boundaries
            left = Math.Max(0, left);
            right = Math.Min(_thumbnailSize.Width, right);
            top = Math.Max(0, top);
            bottom = Math.Min(_thumbnailSize.Height, bottom);

            //Compute region rectangle in thumbnail coordinates
            var startPoint = ClientToThumbnail(new Point(left, top));
            var endPoint = ClientToThumbnail(new Point(right, bottom));
            var final = new Rectangle(
                startPoint.X,
                startPoint.Y,
                endPoint.X - startPoint.X,
                endPoint.Y - startPoint.Y
            );

            //System.Diagnostics.Trace.WriteLine(string.Format("Drawn from {0} to {1}, as region {2}.", start, end, final));

            //Signal
            OnRegionDrawn(final);
        }

        /// <summary>
        /// Overridden. Starts drawing a region when the user clicks on the panel.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e) {
            if (DrawMouseRegions && e.Button == MouseButtons.Left) {
                //Start new region drawing
                _drawingRegion = true;
                _drawingSuspended = false;
                _regionStartPoint = _regionLastPoint = e.Location;

                this.Invalidate();
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Overridden. Finishes drawing a region when the user releases the mouse button.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e) {
            if (DrawMouseRegions && e.Button == MouseButtons.Left) {
                //Region completed
                _drawingRegion = false;
                _drawingSuspended = false;
                RaiseRegionDrawn(_regionStartPoint, _regionLastPoint);

                this.Invalidate();
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Overridden. Suspends region drawing when the mouse leaves the panel.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e) {
            _drawingSuspended = true;

            this.Invalidate();

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Overridden. Resumes region drawing when the mouse enters the panel.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e) {
            _drawingSuspended = false;

            this.Invalidate();

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Overridden. Updates the region drawing when the mouse moves.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e) {
            if (_drawingRegion && e.Button == MouseButtons.Left) {
                //Continue drawing
                _regionLastPoint = e.Location;

                this.Invalidate();
            }
            else if(DrawMouseRegions && !_drawingRegion){
                //Keep track of region start point
                _regionLastPoint = e.Location;

                this.Invalidate();
            }

            base.OnMouseMove(e);
        }

        readonly static Pen RedPen = new Pen(Color.FromArgb(255, Color.Red), 1.5f); //TODO: check width

        /// <summary>
        /// Overridden. Paints the region selection rectangle and crosshairs.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e) {
            if (_drawingRegion) {
                //Is currently drawing, show rectangle
                int left = Math.Min(_regionStartPoint.X, _regionLastPoint.X);
                int right = Math.Max(_regionStartPoint.X, _regionLastPoint.X);
                int top = Math.Min(_regionStartPoint.Y, _regionLastPoint.Y);
                int bottom = Math.Max(_regionStartPoint.Y, _regionLastPoint.Y);

                e.Graphics.DrawRectangle(RedPen, left, top, right - left, bottom - top);
            }
            else if (DrawMouseRegions && ! _drawingSuspended) {
                //Show cursor coordinates
                e.Graphics.DrawLine(RedPen, new Point(0, _regionLastPoint.Y), new Point(ClientSize.Width, _regionLastPoint.Y));
                e.Graphics.DrawLine(RedPen, new Point(_regionLastPoint.X, 0), new Point(_regionLastPoint.X, ClientSize.Height));
            }

            base.OnPaint(e);
        }

        #endregion

        #region Thumbnail clone click

        /// <summary>
        /// Overridden. Raises the CloneClick event when the user clicks on the thumbnail.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);

            if (_thumbnail == null)
                return;

            //Raise clicking event to allow click forwarding
            if (ReportThumbnailClicks) {
                OnCloneClick(ClientToThumbnail(e.Location), e.Button, false);
            }
        }

        /// <summary>
        /// Overridden. Raises the CloneClick event when the user double-clicks on the thumbnail.
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);

            if (_thumbnail == null)
                return;

            //Raise double clicking event to allow click forwarding
            if (ReportThumbnailClicks) {
                OnCloneClick(ClientToThumbnail(e.Location), e.Button, true);
            }
        }

        /// <summary>
        /// Is raised when the thumbnail clone is clicked.
        /// </summary>
        public event EventHandler<CloneClickEventArgs> CloneClick;

        /// <summary>
        /// Raises the CloneClick event.
        /// </summary>
        /// <param name="location">The location of the click in thumbnail coordinates.</param>
        /// <param name="buttons">The mouse buttons that were pressed.</param>
        /// <param name="doubleClick">Whether the click was a double-click.</param>
        protected virtual void OnCloneClick(Point location, MouseButtons buttons, bool doubleClick){
            var evt = CloneClick;
            if(evt != null)
                evt(this, new CloneClickEventArgs(location, buttons, doubleClick));
        }

        #endregion

        /// <summary>
        /// Convert a point in client coordinates to a point expressed in terms of a cloned thumbnail window.
        /// </summary>
        /// <param name="position">Point in client coordinates.</param>
        protected Point ClientToThumbnail(Point position) {
            //Compensate padding
            position.X -= _padWidth;
            position.Y -= _padHeight;

            //Determine position in fractional terms (on the size of the thumbnail control)
            PointF proportionalPosition = new PointF(
                (float)position.X / _thumbnailSize.Width,
                (float)position.Y / _thumbnailSize.Height
            );

            //Get real pixel region info
            Size source = ThumbnailPixelSize;
            Point offset = (_regionEnabled) ? SelectedRegion.Offset : Point.Empty;

            return new Point(
                (int)((proportionalPosition.X * source.Width) + offset.X),
                (int)((proportionalPosition.Y * source.Height) + offset.Y)
            );
        }

    }

}
