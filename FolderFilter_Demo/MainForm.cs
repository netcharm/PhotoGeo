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
            explorerBrowser.ContentOptions.ExtendedTiles = true;
            explorerBrowser.ContentOptions.FullRowSelect = true;
            explorerBrowser.ContentOptions.NoSubfolders = false;
            explorerBrowser.ContentOptions.NoBrowserViewState = false;
            explorerBrowser.ContentOptions.ViewMode = ExplorerBrowserViewMode.Auto;

            explorerBrowser.NavigationOptions.AlwaysNavigate = false;
            explorerBrowser.NavigationOptions.PaneVisibility.AdvancedQuery = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Commands = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.CommandsOrganize = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.CommandsView = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Navigation = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.CommandsOrganize = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Details = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Preview = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Query = PaneVisibilityState.Show;

            SearchCondition searchCondition = SearchConditionFactory.ParseStructuredQuery("*.jpg");
            ShellSearchFolder search = new ShellSearchFolder( searchCondition, new string[] { "E:\\Downloads\\Develop" } );

            //explorerBrowser.Navigate( (ShellObject) KnownFolders.Desktop );
            //explorerBrowser.Navigate( ShellFileSystemFolder.FromFolderPath( AppFolder ) );
            explorerBrowser.Navigate( ShellFileSystemFolder.FromFolderPath( "E:\\Downloads\\Develop" ) );

            // Folder content filter test

        }

        private void explorerBrowser_NavigationComplete( object sender, NavigationCompleteEventArgs e )
        {
            foreach ( ShellObject item in explorerBrowser.Items )
            {
                if ( !item.Name.EndsWith( ".jpg", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    item.Dispose();
                }
            }
        }

        private void explorerBrowser_SelectionChanged( object sender, EventArgs e )
        {
            foreach ( ShellObject item in explorerBrowser.Items )
            {
                if ( !item.Name.EndsWith( ".jpg", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    item.Dispose();
                }
            }
        }
    }
}
