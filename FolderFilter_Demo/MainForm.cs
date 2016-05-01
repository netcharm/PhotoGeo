using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;

namespace FolderFilter_Demo
{
    public partial class MainForm : Form
    {
        private string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);

        public MainForm()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Icon = Icon.ExtractAssociatedIcon( Application.ExecutablePath );
        }

        private void MainForm_Load( object sender, EventArgs e )
        {
            // setting explorerBrowser
            explorerBrowser.ContentOptions.AutoArrange = true;
            explorerBrowser.ContentOptions.FullRowSelect = true;
            explorerBrowser.ContentOptions.NoSubfolders = false;
            explorerBrowser.ContentOptions.ExtendedTiles = true;
            explorerBrowser.NavigationOptions.AlwaysNavigate = true;

            explorerBrowser.NavigationOptions.PaneVisibility.AdvancedQuery = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Query = PaneVisibilityState.Show;

            explorerBrowser.Navigate( (ShellObject) KnownFolders.Desktop );
            //explorerBrowser.Navigate( ShellFileSystemFolder.FromFolderPath( AppFolder ) );

            // Folder content filter test

        }
    }
}
