namespace PhotoGeoTagShell
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.status = new System.Windows.Forms.StatusStrip();
            this.tsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsFilesSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsFilesTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.toolContainer = new System.Windows.Forms.ToolStripContainer();
            this.explorerBrowser = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.tsbtnMapView = new System.Windows.Forms.ToolStripButton();
            this.tsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbVistedFolder = new System.Windows.Forms.ToolStripComboBox();
            this.tsbtnGo = new System.Windows.Forms.ToolStripSplitButton();
            this.tsSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnTouch = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiTouchRecursion = new System.Windows.Forms.ToolStripMenuItem();
            this.tscbKnownFolder = new System.Windows.Forms.ToolStripComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.bgwTouch = new System.ComponentModel.BackgroundWorker();
            this.status.SuspendLayout();
            this.toolContainer.ContentPanel.SuspendLayout();
            this.toolContainer.TopToolStripPanel.SuspendLayout();
            this.toolContainer.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.AutoSize = false;
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsInfo,
            this.tsFilesSelected,
            this.tsFilesTotal,
            this.tsProgress});
            this.status.Location = new System.Drawing.Point(0, 551);
            this.status.Name = "status";
            this.status.ShowItemToolTips = true;
            this.status.Size = new System.Drawing.Size(802, 22);
            this.status.TabIndex = 0;
            this.status.Text = "status";
            // 
            // tsInfo
            // 
            this.tsInfo.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsInfo.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsInfo.Name = "tsInfo";
            this.tsInfo.Size = new System.Drawing.Size(677, 17);
            this.tsInfo.Spring = true;
            this.tsInfo.Text = "Ok";
            this.tsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsFilesSelected
            // 
            this.tsFilesSelected.AutoToolTip = true;
            this.tsFilesSelected.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsFilesSelected.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsFilesSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsFilesSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsFilesSelected.Name = "tsFilesSelected";
            this.tsFilesSelected.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsFilesSelected.Size = new System.Drawing.Size(4, 17);
            this.tsFilesSelected.ToolTipText = "Selected Photos";
            // 
            // tsFilesTotal
            // 
            this.tsFilesTotal.AutoToolTip = true;
            this.tsFilesTotal.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsFilesTotal.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsFilesTotal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsFilesTotal.Name = "tsFilesTotal";
            this.tsFilesTotal.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsFilesTotal.Size = new System.Drawing.Size(4, 17);
            this.tsFilesTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsFilesTotal.ToolTipText = "Total Photos";
            // 
            // tsProgress
            // 
            this.tsProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsProgress.AutoToolTip = true;
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // toolContainer
            // 
            // 
            // toolContainer.ContentPanel
            // 
            this.toolContainer.ContentPanel.Controls.Add(this.explorerBrowser);
            this.toolContainer.ContentPanel.Size = new System.Drawing.Size(802, 526);
            this.toolContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolContainer.Location = new System.Drawing.Point(0, 0);
            this.toolContainer.Name = "toolContainer";
            this.toolContainer.Size = new System.Drawing.Size(802, 551);
            this.toolContainer.TabIndex = 2;
            this.toolContainer.Text = "toolStripContainer1";
            // 
            // toolContainer.TopToolStripPanel
            // 
            this.toolContainer.TopToolStripPanel.Controls.Add(this.toolMain);
            this.toolContainer.TopToolStripPanel.Controls.Add(this.menuMain);
            // 
            // explorerBrowser
            // 
            this.explorerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorerBrowser.Location = new System.Drawing.Point(0, 0);
            this.explorerBrowser.Name = "explorerBrowser";
            this.explorerBrowser.PropertyBagName = "Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser";
            this.explorerBrowser.Size = new System.Drawing.Size(802, 526);
            this.explorerBrowser.TabIndex = 3;
            this.explorerBrowser.SelectionChanged += new System.EventHandler(this.explorerBrowser_SelectionChanged);
            this.explorerBrowser.ItemsChanged += new System.EventHandler(this.explorerBrowser_ItemsChanged);
            this.explorerBrowser.NavigationComplete += new System.EventHandler<Microsoft.WindowsAPICodePack.Controls.NavigationCompleteEventArgs>(this.explorerBrowser_NavigationComplete);
            // 
            // menuMain
            // 
            this.menuMain.Dock = System.Windows.Forms.DockStyle.None;
            this.menuMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(44, 20);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "MainMenu";
            this.menuMain.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(41, 16);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolMain
            // 
            this.toolMain.AllowItemReorder = true;
            this.toolMain.Dock = System.Windows.Forms.DockStyle.None;
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnMapView,
            this.tsSep1,
            this.tscbVistedFolder,
            this.tsbtnGo,
            this.tsSep3,
            this.tsbtnTouch,
            this.tscbKnownFolder});
            this.toolMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolMain.Location = new System.Drawing.Point(0, 0);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(802, 25);
            this.toolMain.Stretch = true;
            this.toolMain.TabIndex = 1;
            this.toolMain.Text = "MainToolBar";
            // 
            // tsbtnMapView
            // 
            this.tsbtnMapView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnMapView.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMapView.Image")));
            this.tsbtnMapView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMapView.Name = "tsbtnMapView";
            this.tsbtnMapView.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbtnMapView.Size = new System.Drawing.Size(57, 22);
            this.tsbtnMapView.Text = "View Map";
            this.tsbtnMapView.ToolTipText = "Open The Map Viewer Window";
            this.tsbtnMapView.Click += new System.EventHandler(this.tsbtnMapView_Click);
            // 
            // tsSep1
            // 
            this.tsSep1.Name = "tsSep1";
            this.tsSep1.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbVistedFolder
            // 
            this.tscbVistedFolder.AutoToolTip = true;
            this.tscbVistedFolder.DropDownWidth = 600;
            this.tscbVistedFolder.Margin = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.tscbVistedFolder.Name = "tscbVistedFolder";
            this.tscbVistedFolder.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tscbVistedFolder.Size = new System.Drawing.Size(580, 25);
            this.tscbVistedFolder.ToolTipText = "History Of Visited Folders";
            this.tscbVistedFolder.SelectedIndexChanged += new System.EventHandler(this.tscbVistedFolder_SelectedIndexChanged);
            this.tscbVistedFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tscbVistedFolder_KeyPress);
            // 
            // tsbtnGo
            // 
            this.tsbtnGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnGo.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnGo.Image")));
            this.tsbtnGo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnGo.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this.tsbtnGo.Name = "tsbtnGo";
            this.tsbtnGo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbtnGo.Size = new System.Drawing.Size(33, 22);
            this.tsbtnGo.Text = "GO";
            this.tsbtnGo.ToolTipText = "Goto Folder of left box";
            this.tsbtnGo.Click += new System.EventHandler(this.tsbtnGo_Click);
            // 
            // tsSep3
            // 
            this.tsSep3.Name = "tsSep3";
            this.tsSep3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnTouch
            // 
            this.tsbtnTouch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnTouch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTouchRecursion});
            this.tsbtnTouch.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnTouch.Image")));
            this.tsbtnTouch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnTouch.Name = "tsbtnTouch";
            this.tsbtnTouch.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbtnTouch.Size = new System.Drawing.Size(51, 22);
            this.tsbtnTouch.Text = "Touch";
            this.tsbtnTouch.ToolTipText = "Touch Photos Datetime to Taken";
            this.tsbtnTouch.Click += new System.EventHandler(this.tsbtnTouch_Click);
            // 
            // tsmiTouchRecursion
            // 
            this.tsmiTouchRecursion.Name = "tsmiTouchRecursion";
            this.tsmiTouchRecursion.Size = new System.Drawing.Size(160, 22);
            this.tsmiTouchRecursion.Text = "Touch Recursion";
            this.tsmiTouchRecursion.Click += new System.EventHandler(this.tsmiTouchRecursion_Click);
            // 
            // tscbKnownFolder
            // 
            this.tscbKnownFolder.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tscbKnownFolder.DropDownWidth = 240;
            this.tscbKnownFolder.Margin = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.tscbKnownFolder.Name = "tscbKnownFolder";
            this.tscbKnownFolder.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.tscbKnownFolder.Size = new System.Drawing.Size(121, 20);
            this.tscbKnownFolder.Visible = false;
            this.tscbKnownFolder.SelectedIndexChanged += new System.EventHandler(this.tscbKnownFolder_SelectedIndexChanged);
            // 
            // toolTip
            // 
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Tip";
            // 
            // bgwTouch
            // 
            this.bgwTouch.WorkerReportsProgress = true;
            this.bgwTouch.WorkerSupportsCancellation = true;
            this.bgwTouch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwTouch_DoWork);
            this.bgwTouch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwTouch_ProgressChanged);
            this.bgwTouch.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwTouch_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 573);
            this.Controls.Add(this.toolContainer);
            this.Controls.Add(this.status);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(810, 600);
            this.Name = "MainForm";
            this.Text = "Photo GeoInfo Tag Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.toolContainer.ContentPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.PerformLayout();
            this.toolContainer.ResumeLayout(false);
            this.toolContainer.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
        private System.Windows.Forms.ToolStripContainer toolContainer;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton tsbtnMapView;
        private System.Windows.Forms.ToolStripStatusLabel tsInfo;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripComboBox tscbVistedFolder;
        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser explorerBrowser;
        private System.Windows.Forms.ToolStripComboBox tscbKnownFolder;
        private System.Windows.Forms.ToolStripSplitButton tsbtnGo;
        private System.Windows.Forms.ToolStripSeparator tsSep3;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripStatusLabel tsFilesSelected;
        private System.Windows.Forms.ToolStripStatusLabel tsFilesTotal;
        private System.Windows.Forms.ToolStripSplitButton tsbtnTouch;
        private System.Windows.Forms.ToolStripMenuItem tsmiTouchRecursion;
        private System.ComponentModel.BackgroundWorker bgwTouch;
    }
}