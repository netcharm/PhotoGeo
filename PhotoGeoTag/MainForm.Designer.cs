namespace PhotoGeoTag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.status = new System.Windows.Forms.StatusStrip();
            this.tsInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.toolContainer = new System.Windows.Forms.ToolStripContainer();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.explorerTree = new WindowsExplorer.ExplorerTree();
            this.lvImage = new Manina.Windows.Forms.ImageListView();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.tsbtnMapView = new System.Windows.Forms.ToolStripButton();
            this.tscbViewMode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.status.SuspendLayout();
            this.toolContainer.ContentPanel.SuspendLayout();
            this.toolContainer.TopToolStripPanel.SuspendLayout();
            this.toolContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsInfo,
            this.tsProgress});
            this.status.Location = new System.Drawing.Point(0, 551);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(792, 22);
            this.status.TabIndex = 0;
            this.status.Text = "status";
            // 
            // tsInfo
            // 
            this.tsInfo.Name = "tsInfo";
            this.tsInfo.Size = new System.Drawing.Size(675, 17);
            this.tsInfo.Spring = true;
            this.tsInfo.Text = "Ok";
            this.tsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.toolContainer.ContentPanel.Controls.Add(this.splitMain);
            this.toolContainer.ContentPanel.Size = new System.Drawing.Size(792, 526);
            this.toolContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolContainer.Location = new System.Drawing.Point(0, 0);
            this.toolContainer.Name = "toolContainer";
            this.toolContainer.Size = new System.Drawing.Size(792, 551);
            this.toolContainer.TabIndex = 2;
            this.toolContainer.Text = "toolStripContainer1";
            // 
            // toolContainer.TopToolStripPanel
            // 
            this.toolContainer.TopToolStripPanel.Controls.Add(this.menuMain);
            this.toolContainer.TopToolStripPanel.Controls.Add(this.toolMain);
            // 
            // splitMain
            // 
            this.splitMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.explorerTree);
            this.splitMain.Panel1MinSize = 240;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.lvImage);
            this.splitMain.Size = new System.Drawing.Size(792, 526);
            this.splitMain.SplitterDistance = 240;
            this.splitMain.SplitterWidth = 2;
            this.splitMain.TabIndex = 2;
            // 
            // explorerTree
            // 
            this.explorerTree.AutoScroll = true;
            this.explorerTree.BackColor = System.Drawing.Color.White;
            this.explorerTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorerTree.HideSelection = false;
            this.explorerTree.Location = new System.Drawing.Point(0, 0);
            this.explorerTree.Name = "explorerTree";
            this.explorerTree.SelectedPath = "D:\\Develop\\MS\\VS2015\\Common7\\IDE";
            this.explorerTree.ShowAddressbar = false;
            this.explorerTree.ShowLines = true;
            this.explorerTree.ShowMyDocuments = true;
            this.explorerTree.ShowMyFavorites = true;
            this.explorerTree.ShowMyNetwork = true;
            this.explorerTree.ShowNodeTooltip = false;
            this.explorerTree.ShowToolbar = false;
            this.explorerTree.Size = new System.Drawing.Size(236, 522);
            this.explorerTree.TabIndex = 1;
            this.explorerTree.PathChanged += new WindowsExplorer.ExplorerTree.PathChangedEventHandler(this.explorerTree_PathChanged);
            // 
            // lvImage
            // 
            this.lvImage.AllowCheckBoxClick = false;
            this.lvImage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvImage.ColumnHeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvImage.GroupHeaderFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lvImage.Location = new System.Drawing.Point(0, 0);
            this.lvImage.Name = "lvImage";
            this.lvImage.PaneWidth = 320;
            this.lvImage.PersistentCacheDirectory = "";
            this.lvImage.PersistentCacheSize = ((long)(100));
            this.lvImage.ShowCheckBoxes = true;
            this.lvImage.ShowFileIcons = true;
            this.lvImage.Size = new System.Drawing.Size(546, 522);
            this.lvImage.TabIndex = 0;
            this.lvImage.ThumbnailSize = new System.Drawing.Size(120, 120);
            this.lvImage.ItemHover += new Manina.Windows.Forms.ItemHoverEventHandler(this.lvImage_ItemHover);
            this.lvImage.ItemDoubleClick += new Manina.Windows.Forms.ItemDoubleClickEventHandler(this.lvImage_ItemDoubleClick);
            this.lvImage.SelectionChanged += new System.EventHandler(this.lvImage_SelectionChanged);
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
            this.toolStripSeparator1,
            this.tscbViewMode});
            this.toolMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolMain.Location = new System.Drawing.Point(3, 0);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(196, 25);
            this.toolMain.TabIndex = 1;
            this.toolMain.Text = "MainToolBar";
            // 
            // tsbtnMapView
            // 
            this.tsbtnMapView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnMapView.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnMapView.Image")));
            this.tsbtnMapView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMapView.Name = "tsbtnMapView";
            this.tsbtnMapView.Size = new System.Drawing.Size(57, 22);
            this.tsbtnMapView.Text = "View Map";
            this.tsbtnMapView.Click += new System.EventHandler(this.tsbtnMapView_Click);
            // 
            // tscbViewMode
            // 
            this.tscbViewMode.AutoToolTip = true;
            this.tscbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbViewMode.Name = "tscbViewMode";
            this.tscbViewMode.Size = new System.Drawing.Size(121, 25);
            this.tscbViewMode.SelectedIndexChanged += new System.EventHandler(this.tscbViewMode_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.toolContainer);
            this.Controls.Add(this.status);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Photo Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.toolContainer.ContentPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolContainer.TopToolStripPanel.PerformLayout();
            this.toolContainer.ResumeLayout(false);
            this.toolContainer.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
        private System.Windows.Forms.ToolStripContainer toolContainer;
        private System.Windows.Forms.SplitContainer splitMain;
        private Manina.Windows.Forms.ImageListView lvImage;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton tsbtnMapView;
        private WindowsExplorer.ExplorerTree explorerTree;
        private System.Windows.Forms.ToolStripStatusLabel tsInfo;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.ToolStripComboBox tscbViewMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}