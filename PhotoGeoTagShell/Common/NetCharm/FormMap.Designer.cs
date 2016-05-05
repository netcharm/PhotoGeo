namespace NetCharm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMap));
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.cbMapProviders = new System.Windows.Forms.ComboBox();
            this.status = new System.Windows.Forms.StatusStrip();
            this.tsLat = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLon = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsbtnFunction = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiResetMap = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShiftMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExportGPXKML = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImportGPXKML = new System.Windows.Forms.ToolStripMenuItem();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.chkMapShift = new System.Windows.Forms.CheckBox();
            this.btnPinPhoto = new System.Windows.Forms.Button();
            this.picGeoRef = new System.Windows.Forms.PictureBox();
            this.edQuery = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.bgwSetGeo = new System.ComponentModel.BackgroundWorker();
            this.bgwShowImage = new System.ComponentModel.BackgroundWorker();
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
            this.gMap.Size = new System.Drawing.Size(627, 435);
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
            this.cbMapProviders.DropDownWidth = 200;
            this.cbMapProviders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbMapProviders.FormattingEnabled = true;
            this.cbMapProviders.Location = new System.Drawing.Point(12, 456);
            this.cbMapProviders.Name = "cbMapProviders";
            this.cbMapProviders.Size = new System.Drawing.Size(107, 20);
            this.cbMapProviders.Sorted = true;
            this.cbMapProviders.TabIndex = 1;
            this.toolTip.SetToolTip(this.cbMapProviders, "Selecting Map Provider");
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
            this.tsProgress,
            this.tsbtnFunction});
            this.status.Location = new System.Drawing.Point(0, 501);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(652, 22);
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
            this.tsProgress.Size = new System.Drawing.Size(80, 16);
            // 
            // tsbtnFunction
            // 
            this.tsbtnFunction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResetMap,
            this.tsmiShiftMap,
            this.toolStripMenuItem1,
            this.tsmiExportGPXKML,
            this.tsmiImportGPXKML});
            this.tsbtnFunction.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnFunction.Image")));
            this.tsbtnFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnFunction.Name = "tsbtnFunction";
            this.tsbtnFunction.Size = new System.Drawing.Size(51, 20);
            this.tsbtnFunction.Text = "RESET";
            this.tsbtnFunction.ToolTipText = "Switch Shift Map On / Off";
            this.tsbtnFunction.ButtonClick += new System.EventHandler(this.tsmiResetMap_Click);
            // 
            // tsmiResetMap
            // 
            this.tsmiResetMap.Name = "tsmiResetMap";
            this.tsmiResetMap.Size = new System.Drawing.Size(154, 22);
            this.tsmiResetMap.Text = "Reset Map";
            this.tsmiResetMap.ToolTipText = "Reset Map View";
            this.tsmiResetMap.Click += new System.EventHandler(this.tsmiResetMap_Click);
            // 
            // tsmiShiftMap
            // 
            this.tsmiShiftMap.Name = "tsmiShiftMap";
            this.tsmiShiftMap.Size = new System.Drawing.Size(154, 22);
            this.tsmiShiftMap.Text = "Shift Map";
            this.tsmiShiftMap.Click += new System.EventHandler(this.tsmiShiftMap_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(151, 6);
            // 
            // tsmiExportGPXKML
            // 
            this.tsmiExportGPXKML.Name = "tsmiExportGPXKML";
            this.tsmiExportGPXKML.Size = new System.Drawing.Size(154, 22);
            this.tsmiExportGPXKML.Text = "Export GPX/KML";
            this.tsmiExportGPXKML.Click += new System.EventHandler(this.tsmiExportGPXKML_Click);
            // 
            // tsmiImportGPXKML
            // 
            this.tsmiImportGPXKML.Name = "tsmiImportGPXKML";
            this.tsmiImportGPXKML.Size = new System.Drawing.Size(154, 22);
            this.tsmiImportGPXKML.Text = "Import GPX/KML";
            this.tsmiImportGPXKML.Click += new System.EventHandler(this.tsmiImportGPXKML_Click);
            // 
            // trackZoom
            // 
            this.trackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackZoom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackZoom.LargeChange = 2;
            this.trackZoom.Location = new System.Drawing.Point(123, 455);
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new System.Drawing.Size(171, 42);
            this.trackZoom.TabIndex = 4;
            this.trackZoom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip.SetToolTip(this.trackZoom, "Change Map Zoom Level");
            this.trackZoom.Scroll += new System.EventHandler(this.trackZoom_Scroll);
            // 
            // chkMapShift
            // 
            this.chkMapShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapShift.AutoEllipsis = true;
            this.chkMapShift.Location = new System.Drawing.Point(12, 480);
            this.chkMapShift.Name = "chkMapShift";
            this.chkMapShift.Size = new System.Drawing.Size(110, 18);
            this.chkMapShift.TabIndex = 6;
            this.chkMapShift.Text = "Adj. China Map";
            this.toolTip.SetToolTip(this.chkMapShift, "Adjust Map Shift in China mainland");
            this.chkMapShift.UseVisualStyleBackColor = true;
            this.chkMapShift.CheckedChanged += new System.EventHandler(this.chkMapShift_CheckedChanged);
            // 
            // btnPinPhoto
            // 
            this.btnPinPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPinPhoto.Image = ((System.Drawing.Image)(resources.GetObject("btnPinPhoto.Image")));
            this.btnPinPhoto.Location = new System.Drawing.Point(545, 455);
            this.btnPinPhoto.Name = "btnPinPhoto";
            this.btnPinPhoto.Size = new System.Drawing.Size(35, 42);
            this.btnPinPhoto.TabIndex = 7;
            this.toolTip.SetToolTip(this.btnPinPhoto, "Pin Photo(s) to Map");
            this.btnPinPhoto.UseVisualStyleBackColor = true;
            this.btnPinPhoto.Click += new System.EventHandler(this.btnPinPhoto_Click);
            // 
            // picGeoRef
            // 
            this.picGeoRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picGeoRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGeoRef.Location = new System.Drawing.Point(586, 455);
            this.picGeoRef.Name = "picGeoRef";
            this.picGeoRef.Size = new System.Drawing.Size(54, 42);
            this.picGeoRef.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGeoRef.TabIndex = 5;
            this.picGeoRef.TabStop = false;
            this.toolTip.SetToolTip(this.picGeoRef, "Refrence Photo Drag Area");
            this.picGeoRef.DragDrop += new System.Windows.Forms.DragEventHandler(this.picGeoRef_DragDrop);
            this.picGeoRef.DragEnter += new System.Windows.Forms.DragEventHandler(this.picGeoRef_DragEnter);
            // 
            // edQuery
            // 
            this.edQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edQuery.HideSelection = false;
            this.edQuery.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.edQuery.Location = new System.Drawing.Point(286, 466);
            this.edQuery.Name = "edQuery";
            this.edQuery.Size = new System.Drawing.Size(253, 21);
            this.edQuery.TabIndex = 8;
            this.edQuery.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.edQuery, "Simple Query Posiiion By Keyword");
            this.edQuery.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edQuery_KeyPress);
            // 
            // toolTip
            // 
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Tip";
            // 
            // bgwSetGeo
            // 
            this.bgwSetGeo.WorkerReportsProgress = true;
            this.bgwSetGeo.WorkerSupportsCancellation = true;
            this.bgwSetGeo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSetGeo_DoWork);
            this.bgwSetGeo.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwSetGeo_ProgressChanged);
            this.bgwSetGeo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSetGeo_RunWorkerCompleted);
            // 
            // bgwShowImage
            // 
            this.bgwShowImage.WorkerReportsProgress = true;
            this.bgwShowImage.WorkerSupportsCancellation = true;
            this.bgwShowImage.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwShowImage_DoWork);
            this.bgwShowImage.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwShowImage_ProgressChanged);
            this.bgwShowImage.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwShowImage_RunWorkerCompleted);
            // 
            // FormMap
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 523);
            this.Controls.Add(this.edQuery);
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
        private System.Windows.Forms.ToolStripSplitButton tsbtnFunction;
        private System.Windows.Forms.ToolStripMenuItem tsmiResetMap;
        private System.Windows.Forms.ToolStripMenuItem tsmiShiftMap;
        private System.Windows.Forms.TextBox edQuery;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportGPXKML;
        private System.Windows.Forms.ToolStripMenuItem tsmiImportGPXKML;
        private System.ComponentModel.BackgroundWorker bgwSetGeo;
        private System.ComponentModel.BackgroundWorker bgwShowImage;
    }
}

