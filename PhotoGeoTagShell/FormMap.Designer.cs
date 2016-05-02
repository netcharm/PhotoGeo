namespace PhotoGeoTagShell
{
    partial class FormMap
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.cbMapProviders = new System.Windows.Forms.ComboBox();
            this.status = new System.Windows.Forms.StatusStrip();
            this.tsLat = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLon = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.chkMapShift = new System.Windows.Forms.CheckBox();
            this.btnPinPhoto = new System.Windows.Forms.Button();
            this.picGeoRef = new System.Windows.Forms.PictureBox();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGeoRef)).BeginInit();
            this.SuspendLayout();
            // 
            // gMap
            // 
            this.gMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMap.Bearing = 0F;
            this.gMap.CanDragMap = true;
            this.gMap.EmptyTileColor = System.Drawing.Color.LightGray;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(13, 13);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 18;
            this.gMap.MinZoom = 0;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(615, 421);
            this.gMap.TabIndex = 0;
            this.gMap.Zoom = 2D;
            this.gMap.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gMap_OnMarkerClick);
            this.gMap.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.gMap_OnMarkerEnter);
            this.gMap.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.gMap_OnMarkerLeave);
            this.gMap.OnPositionChanged += new GMap.NET.PositionChanged(this.gMap_OnPositionChanged);
            this.gMap.OnTileLoadComplete += new GMap.NET.TileLoadComplete(this.gMap_OnTileLoadComplete);
            this.gMap.OnTileLoadStart += new GMap.NET.TileLoadStart(this.gMap_OnTileLoadStart);
            this.gMap.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.gMap_OnMapZoomChanged);
            this.gMap.OnMapTypeChanged += new GMap.NET.MapTypeChanged(this.gMap_OnMapTypeChanged);
            this.gMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gMap_MouseDown);
            this.gMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gMap_MouseMove);
            this.gMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gMap_MouseUp);
            // 
            // cbMapProviders
            // 
            this.cbMapProviders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbMapProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMapProviders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbMapProviders.FormattingEnabled = true;
            this.cbMapProviders.Location = new System.Drawing.Point(12, 452);
            this.cbMapProviders.Name = "cbMapProviders";
            this.cbMapProviders.Size = new System.Drawing.Size(144, 20);
            this.cbMapProviders.Sorted = true;
            this.cbMapProviders.TabIndex = 1;
            this.cbMapProviders.SelectedIndexChanged += new System.EventHandler(this.cbMapProviders_SelectedIndexChanged);
            // 
            // status
            // 
            this.status.AutoSize = false;
            this.status.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLat,
            this.tsLon,
            this.tsZoom,
            this.tsInfo,
            this.tsProgress});
            this.status.Location = new System.Drawing.Point(0, 487);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(640, 22);
            this.status.TabIndex = 2;
            this.status.Text = "OK";
            // 
            // tsLat
            // 
            this.tsLat.AutoSize = false;
            this.tsLat.AutoToolTip = true;
            this.tsLat.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsLat.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsLat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsLat.Name = "tsLat";
            this.tsLat.Size = new System.Drawing.Size(110, 17);
            this.tsLat.Text = "Lat:";
            this.tsLat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsLon
            // 
            this.tsLon.AutoSize = false;
            this.tsLon.AutoToolTip = true;
            this.tsLon.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsLon.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsLon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsLon.Name = "tsLon";
            this.tsLon.Size = new System.Drawing.Size(110, 17);
            this.tsLon.Text = "Lon:";
            this.tsLon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsZoom
            // 
            this.tsZoom.AutoSize = false;
            this.tsZoom.AutoToolTip = true;
            this.tsZoom.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsZoom.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.tsZoom.Name = "tsZoom";
            this.tsZoom.Size = new System.Drawing.Size(60, 17);
            this.tsZoom.Text = "Zoom:";
            this.tsZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsInfo
            // 
            this.tsInfo.Name = "tsInfo";
            this.tsInfo.Size = new System.Drawing.Size(212, 17);
            this.tsInfo.Spring = true;
            this.tsInfo.Text = "OK";
            // 
            // tsProgress
            // 
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // trackZoom
            // 
            this.trackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackZoom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackZoom.LargeChange = 2;
            this.trackZoom.Location = new System.Drawing.Point(156, 441);
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new System.Drawing.Size(247, 42);
            this.trackZoom.TabIndex = 4;
            this.trackZoom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackZoom.Scroll += new System.EventHandler(this.trackZoom_Scroll);
            // 
            // chkMapShift
            // 
            this.chkMapShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMapShift.AutoSize = true;
            this.chkMapShift.Location = new System.Drawing.Point(409, 454);
            this.chkMapShift.Name = "chkMapShift";
            this.chkMapShift.Size = new System.Drawing.Size(108, 16);
            this.chkMapShift.TabIndex = 6;
            this.chkMapShift.Text = "Map Need Shift";
            this.chkMapShift.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMapShift.UseVisualStyleBackColor = true;
            this.chkMapShift.CheckedChanged += new System.EventHandler(this.chkMapShift_CheckedChanged);
            // 
            // btnPinPhoto
            // 
            this.btnPinPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPinPhoto.Image = global::PhotoGeoTagShell.Properties.Resources.map_pin_md_32x32;
            this.btnPinPhoto.Location = new System.Drawing.Point(526, 441);
            this.btnPinPhoto.Name = "btnPinPhoto";
            this.btnPinPhoto.Size = new System.Drawing.Size(37, 42);
            this.btnPinPhoto.TabIndex = 7;
            this.btnPinPhoto.UseVisualStyleBackColor = true;
            this.btnPinPhoto.Click += new System.EventHandler(this.btnPinPhoto_Click);
            // 
            // picGeoRef
            // 
            this.picGeoRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picGeoRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGeoRef.Location = new System.Drawing.Point(574, 441);
            this.picGeoRef.Name = "picGeoRef";
            this.picGeoRef.Size = new System.Drawing.Size(54, 42);
            this.picGeoRef.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGeoRef.TabIndex = 5;
            this.picGeoRef.TabStop = false;
            this.picGeoRef.DragDrop += new System.Windows.Forms.DragEventHandler(this.picGeoRef_DragDrop);
            this.picGeoRef.DragEnter += new System.Windows.Forms.DragEventHandler(this.picGeoRef_DragEnter);
            // 
            // FormMap
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 509);
            this.Controls.Add(this.btnPinPhoto);
            this.Controls.Add(this.chkMapShift);
            this.Controls.Add(this.picGeoRef);
            this.Controls.Add(this.trackZoom);
            this.Controls.Add(this.status);
            this.Controls.Add(this.cbMapProviders);
            this.Controls.Add(this.gMap);
            this.DoubleBuffered = true;
            this.Name = "FormMap";
            this.Text = "Maps";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMap_FormClosing);
            this.Load += new System.EventHandler(this.FormMap_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGeoRef)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.ComboBox cbMapProviders;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel tsLat;
        private System.Windows.Forms.ToolStripStatusLabel tsLon;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.ToolStripStatusLabel tsZoom;
        private System.Windows.Forms.PictureBox picGeoRef;
        private System.Windows.Forms.CheckBox chkMapShift;
        private System.Windows.Forms.ToolStripStatusLabel tsInfo;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.Button btnPinPhoto;
    }
}

