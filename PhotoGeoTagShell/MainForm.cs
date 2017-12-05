using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using NetCharm;

namespace PhotoGeoTagShell
{
    public partial class MainForm : Form
    {
        string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);
        string CacheFolder = "";
        List<string> lastVisitedFolders = new List<string>();
        Dictionary<string, string> appSettings = new Dictionary<string, string>();

        FormMap MapViewer;//= new FormMap();

        private bool item_changed = false;
        private bool selection_changed = true;
        private Mutex mutSelectionChanged = new Mutex();
        List<string> lastSelections = new List<string>();

        string lastMapProvider = "GoogleChinaHybridMap";
        //string[] PhotoExts = { ".jpg", ".jpeg", ".tif",".tiff" };

        private void configUpdate()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            KeyValueConfigurationElement element = new KeyValueConfigurationElement("lastVisitedFolder", tscbVistedFolder.Text);
            config.AppSettings.Settings.Add( element );

            //foreach ( SettingsProperty property in Properties.Settings.Default.Properties)
            //{
            //    KeyValueConfigurationElement element = new KeyValueConfigurationElement(property.Name, tscbVistedFolder.Text);
            //    config.AppSettings.Settings.Add( property.Name, property.DefaultValue.ToString() );
            //}
            config.Save();
        }

        private void configLoad()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            foreach ( string key in config.AppSettings.Settings.AllKeys )
            {
                if ( appSettings.ContainsKey( key ) )
                    appSettings[key] = config.AppSettings.Settings[key].Value.ToString();
                else
                    appSettings.Add( key, config.AppSettings.Settings[key].Value.ToString() );
            }
            if ( appSettings.ContainsKey( "lastVisitedFolder" ) )
                tscbVistedFolder.Text = appSettings["lastVisitedFolder"].ToString();
            else appSettings.Add( "lastVisitedFolder", AppFolder );
        
            tscbVistedFolder.Items.Clear();
            if ( appSettings.ContainsKey( "folderHistory" ) )
            {
                string historyFolder = appSettings["folderHistory"].ToString();
                tscbVistedFolder.Items.AddRange( historyFolder.Split( Path.PathSeparator ) );
            }
            else
            {
                appSettings.Add( "folderHistory", "" );
            }

            if ( appSettings.ContainsKey( "lastMapProvider" ) )
                lastMapProvider = appSettings["lastMapProvider"].ToString();
            else appSettings.Add( "lastMapProvider", "GoogleChinaHybridMap" );

            try
            {
                if (appSettings.ContainsKey("W"))
                    this.Width = Convert.ToInt32(appSettings["W"]);
            }
            catch (Exception) { }
            try
            {
                if (appSettings.ContainsKey("H"))
                    this.Height = Convert.ToInt32(appSettings["H"]);
            }
            catch (Exception) { }
            try
            {
                if (appSettings.ContainsKey("X"))
                    this.Left = Convert.ToInt32(appSettings["X"]);
            }
            catch (Exception) { }
            try
            {
                if (appSettings.ContainsKey("Y"))
                    this.Top = Convert.ToInt32(appSettings["Y"]);
            }
            catch (Exception) { }
        }

        private void configSave()
        {
            if ( appSettings.ContainsKey( "lastVisitedFolder" ) )
                appSettings["lastVisitedFolder"] = tscbVistedFolder.Text;
            else appSettings.Add( "lastVisitedFolder", AppFolder );

            if ( MapViewer != null && MapViewer.Tag != null) lastMapProvider = (string)MapViewer.Tag;
            if ( appSettings.ContainsKey( "lastMapProvider" ) )
                appSettings["lastMapProvider"] = lastMapProvider;
            else appSettings.Add( "lastMapProvider", "GoogleChinaHybridMap" );

            List<string> folders = new List<string>();
            int count = 0;
            foreach ( string folder in tscbVistedFolder.Items )
            {
                if ( folders.Contains( folder ) ) continue;
                if ( count > 24 ) break;
                folders.Add( folder );
                count++;
            }
            string historyFolder = string.Join( $"{Path.PathSeparator}", folders);
            if ( appSettings.ContainsKey( "folderHistory" ) )
                appSettings["folderHistory"] = historyFolder;
            else appSettings.Add( "folderHistory", "" );

            appSettings["W"] = this.Width.ToString();
            appSettings["H"] = this.Height.ToString();
            appSettings["X"] = this.Left.ToString();
            appSettings["Y"] = this.Top.ToString();

            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            foreach ( string k in appSettings.Keys )
            {
                if ( config.AppSettings.Settings.AllKeys.Contains( k ) )
                    config.AppSettings.Settings[k].Value = appSettings[k].ToString();
                else
                    config.AppSettings.Settings.Add( k, appSettings[k].ToString() );
            }

            config.Save();
        }

        private int getTotalPhotos( ShellObjectCollection items )
        {
            int total = 0;


            IEnumerable<ShellObject> fileinfos  = items.Where( f => (
                    f.IsFileSystemObject && 
                    !f.IsLink &&
                    EXIF.PhotoExts.Contains( Path.GetExtension(f.Name), StringComparer.CurrentCultureIgnoreCase )
                    ) );
            total = fileinfos.Count();
            return ( total );
        }

        private int getTotalPhotos( string folder )
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            IEnumerable<FileInfo> fileinfos  = di.EnumerateFiles().Where( f => EXIF.PhotoExts.Contains( f.Extension, StringComparer.CurrentCultureIgnoreCase ) );
            return ( fileinfos.Count() );
        }

        private void ShowSelectedImage(bool force=false)
        {
            #region detect selection items is same or not
            if ( !force && lastSelections.Count == explorerBrowser.SelectedItems.Count )
            {
                bool diff = false;
                foreach ( ShellObject item in explorerBrowser.SelectedItems )
                {
                    if ( !item.IsFileSystemObject || item.IsLink ) continue;
                    //if ( !lastSelections.Contains( item.ParsingName ) )
                    string dp = item.ParsingName;
                    string ext = Path.GetExtension(dp);
                    if ( File.GetAttributes( dp ).HasFlag( FileAttributes.Directory ) ) continue;
                    if ( EXIF.PhotoExts.Contains( ext, StringComparer.CurrentCultureIgnoreCase ) )
                    {
                        if ( !lastSelections.Contains( item.Name ) )
                        {
                            diff = true;
                            selection_changed = true;
                            break;
                        }
                    }
                }
                if ( !diff )
                {
                    selection_changed = false;
                    return;
                }
            }
            else selection_changed = true;
            #endregion
            //lock ( explorerBrowser.SelectedItems )
            {
                BeginInvoke( new MethodInvoker( delegate ()
                {
                    #region get properties
                    if ( !force && !selection_changed ) return;
                    tsFilesSelected.Text = $"Selected: 0";
                    selection_changed = false;

                    List<KeyValuePair<Image, string>> imgs = new List<KeyValuePair<Image, string>>();
                    try
                    {
                        lastSelections.Clear();
                        foreach ( ShellObject item in explorerBrowser.SelectedItems )
                        {
                            if ( !item.IsFileSystemObject || item.IsLink ) continue;
                            if ( lastSelections.Contains( item.Name ) ) continue;

                            #region get property
                            string dn = item.Name;
                            string dp = item.ParsingName;
                            string ext = Path.GetExtension(dp);
                            if ( !File.GetAttributes( dp ).HasFlag( FileAttributes.Directory ) )
                            {
                                if ( EXIF.PhotoExts.Contains( ext, StringComparer.CurrentCultureIgnoreCase ) )
                                {
                                    lastSelections.Add( item.Name );

                                    Image thumb = new Bitmap(item.Thumbnail.MediumBitmap);
                                    ShellPropertyCollection props = item.Properties.DefaultPropertyCollection;

                                    Dictionary<string, string> properties = new Dictionary<string, string>();
                                    //if (props.Contains( "System.Artist" ))
                                    //{

                                    //}

                                    for ( int i = 0; i < props.Count; i++ )
                                    {
                                        if ( props[i].CanonicalName == null ) continue;
                                        string key = props[i].CanonicalName.Replace("System.", "");
                                        string value = "";

                                        //if ( !key.StartsWith( "Date" ) && !key.StartsWith( "Photo.Date" ) && !key.StartsWith( "ItemPathDisplay" ) ) continue;
                                        if ( !key.StartsWith( "Photo.Date" ) && !key.StartsWith( "ItemPathDisplay" ) ) continue;

                                        object objValue = props[i].ValueAsObject;
                                        //object objValue = new object();
                                        //continue;

                                        if ( objValue != null )
                                        {
                                            if ( props[i].ValueType == typeof( string[] ) )
                                            {
                                                value = string.Join( " ; ", (string[]) objValue );
                                            }
                                            else if ( props[i].ValueType == typeof( uint[] ) )
                                            {
                                                value = string.Join( " , ", (uint[]) objValue );
                                            }
                                            else if ( props[i].ValueType == typeof( double[] ) )
                                            {
                                                value = string.Join( " , ", (double[]) objValue );
                                            }
                                            else
                                            {
                                                value = objValue.ToString();
                                            }
                                        }
                                        properties.Add( key, value );
                                    }
                                    //properties.Add( "Artist", properties.ContainsKey( "Author" ) ? properties["Author"] : "" );
                                    //properties.Add( "Copyright", properties.ContainsKey( "Copyright" ) ? properties["Copyright"] : "" );
                                    //properties.Add( "ImageDescription", properties.ContainsKey( "Subject" ) ? properties["Subject"] : "" );
                                    //properties.Add( "Software", properties.ContainsKey( "ApplicationName" ) ? properties["ApplicationName"] : "" );
                                    properties.Add( "FileSize", properties.ContainsKey( "Size" ) ? properties["Size"] : "" );
                                    //properties.Add( "FileName", properties.ContainsKey( "ItemPathDisplay" ) ? properties["ItemPathDisplay"] : "" );
                                    properties.Add( "FilePath", properties.ContainsKey( "ItemPathDisplay" ) ? properties["ItemPathDisplay"] : "" );
                                    properties.Add( "FolderName", properties.ContainsKey( "ItemFolderPathDisplay" ) ? properties["ItemFolderPathDisplay"] : "" );
                                    properties.Add( "FileType", properties.ContainsKey( "ItemTypeText" ) ? properties["ItemTypeText"] : "" );
                                    //properties.Add( "Dimensions", properties.ContainsKey( "Image.Dimensions" ) ? properties["Image.Dimensions"] : "" );
                                    //properties.Add( "Resolution", properties.ContainsKey( "Image.HorizontalResolution" ) && properties.ContainsKey( "Image.VerticalResolution" ) ? $"{properties["Image.HorizontalResolution"]} x {properties["Image.VerticalResolution"]}" : "" );
                                    //properties.Add( "EquipmentModel", properties.ContainsKey( "Photo.CameraModel" ) ? properties["Photo.CameraModel"] : "" );
                                    //properties.Add( "ExposureTime", properties.ContainsKey( "Photo.ExposureTime" ) ? properties["Photo.ExposureTime"] : "" );
                                    //properties.Add( "FNumber", properties.ContainsKey( "Photo.FNumber" ) ? properties["Photo.FNumber"] : "" );
                                    //properties.Add( "FocalLength", properties.ContainsKey( "Photo.FocalLength" ) ? properties["Photo.FocalLength"] : "" );
                                    //properties.Add( "ISOSpeed", properties.ContainsKey( "Photo.ISOSpeed" ) ? properties["Photo.ISOSpeed"] : "" );
                                    //properties.Add( "ImageDescription", properties.ContainsKey( "Title" ) ? properties["Title"] : "" );
                                    properties.Add( "DateTaken", properties.ContainsKey( "Photo.DateTaken" ) ? properties["Photo.DateTaken"] : "" );
                                    //properties.Add( "Rating", "" );
                                    //properties.Add( "StarRating", "" );
                                    //properties.Add( "UserComment", "" );
                                    thumb.Tag = properties;
                                    //if ( item.Properties != null )
                                    //{
                                    //    thumb.Tag = item.Properties.DefaultPropertyCollection;
                                    //}
                                    imgs.Add( new KeyValuePair<Image, string>( thumb, dp ) );
                                }
                            }
                            #endregion
                        }
                    }
                    catch { };
                    tsFilesSelected.Text = $"Selected: {imgs.Count}";

                    try
                    {
                        if ( MapViewer != null && MapViewer.Visible )
                        {
                            MapViewer.ShowImage( imgs );
                        }
                    }
                    catch { };
                    #endregion
                } ) );
            }
        }

        public MainForm()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Icon = Icon.ExtractAssociatedIcon( Application.ExecutablePath );
        }

        private void MainForm_Load( object sender, EventArgs e )
        {
            //
            configLoad();

            //string CacheFolder = Path.Combine(
            //    Path.GetDirectoryName(new Uri(Assembly.GetAssembly(typeof(ImageListView)).GetName().CodeBase).LocalPath),
            //    "Cache"
            //    );
            CacheFolder = AppFolder + Path.DirectorySeparatorChar + "Cache";
            if ( !Directory.Exists( CacheFolder ) )
                Directory.CreateDirectory( CacheFolder );

            //string lastVisited = Properties.Settings.Default.lastVisitedFolder;
            string lastVisited = appSettings["lastVisitedFolder"];
            if ( string.IsNullOrEmpty( lastVisited ) ) lastVisited = AppFolder;

            string[] args = Common.ParseCommandLine( Environment.CommandLine );
            if ( args.Length > 0 )
            {
                if ( Directory.Exists( args[0] ) )
                {
                    lastVisited = args[0];
                }
                else if ( File.Exists( args[0] ) )
                {
                    lastVisited = Path.GetDirectoryName(args[0]);
                }
            }
            lastVisited = Path.GetFullPath( lastVisited );

            // setting KnownFolder
            List<string> knownFolderList = new List<string>();
            foreach ( IKnownFolder folder in KnownFolders.All )
            {
                knownFolderList.Add( folder.CanonicalName );
                //knownFolderList.Add( string.IsNullOrEmpty(folder.LocalizedName) ? folder.CanonicalName : folder.LocalizedName );
            }
            knownFolderList.Sort();
            tscbKnownFolder.Items.AddRange( knownFolderList.ToArray() );

            // setting explorerBrowser
            explorerBrowser.ContentOptions.AutoArrange = true;
            explorerBrowser.ContentOptions.FullRowSelect = true;
            explorerBrowser.ContentOptions.NoSubfolders = false;
            explorerBrowser.ContentOptions.ExtendedTiles = true;
            explorerBrowser.NavigationOptions.AlwaysNavigate = false;

            explorerBrowser.NavigationOptions.PaneVisibility.AdvancedQuery = PaneVisibilityState.Show;
            explorerBrowser.NavigationOptions.PaneVisibility.Query = PaneVisibilityState.Show;

            if(Directory.Exists( lastVisited ))
                explorerBrowser.Navigate( ShellFileSystemFolder.FromFolderPath( lastVisited ) );

            //ShellSearchConnector shellSearch = new ShellSearchConnector(ShellFileSystemFolder.FromFolderPath( lastVisited ));
            //shellSearch.

            lastVisitedFolders.Clear();
            tscbVistedFolder.Items.Clear();
            if ( appSettings.ContainsKey( "folderHistory" ) )
            {
                string historyFolder = appSettings["folderHistory"].ToString();
                lastVisitedFolders.AddRange( historyFolder.Split( Path.PathSeparator ) );
                tscbVistedFolder.Items.AddRange( historyFolder.Split( Path.PathSeparator ) );
            }
            else
            {
                appSettings.Add( "folderHistory", "" );
            }
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            configSave();
        }

        private void tsbtnMapView_Click( object sender, EventArgs e )
        {
            //FormMap fm = (FormMap)Application.OpenForms[MapViewer.Text];
            try
            {
                if ( MapViewer == null ) { MapViewer = new FormMap(); }
                else if ( MapViewer.Visible ) { MapViewer.Activate(); }
                else { MapViewer = new FormMap(); }
            }
            catch
            {
                MapViewer = new FormMap();
            }
            MapViewer.Tag = lastMapProvider;
            MapViewer.Show();
            ShowSelectedImage(true);
        }

        private void tscbKnownFolder_SelectedIndexChanged( object sender, EventArgs e )
        {
            try
            {
                // Navigate to a known folder
                string cName = tscbKnownFolder.Items[tscbKnownFolder.SelectedIndex].ToString();
                IKnownFolder kf = KnownFolderHelper.FromCanonicalName( cName );
                //IKnownFolder kf = KnownFolderHelper.FromParsingName(cName);

                explorerBrowser.Navigate( (ShellObject) kf );
            }
            catch ( COMException )
            {
                MessageBox.Show( "Navigation not possible." );
            }
        }

        private void tscbVistedFolder_SelectedIndexChanged( object sender, EventArgs e )
        {
            // navigating to specific index in navigation log
            explorerBrowser.NavigateLogLocation( tscbVistedFolder.Items.Count - tscbVistedFolder.SelectedIndex - 1 );
        }

        private void tscbVistedFolder_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == Convert.ToChar( Keys.Enter ) )
            {
                tsbtnGo.PerformClick();
            }
        }

        private void tsbtnGo_Click( object sender, EventArgs e )
        {
            if ( lastVisitedFolders.Count > 0 && !tscbVistedFolder.Text.Equals( lastVisitedFolders[0], StringComparison.CurrentCultureIgnoreCase ) )
            {
                string target = tscbVistedFolder.Text;
                if ( !File.GetAttributes( target ).HasFlag( FileAttributes.Directory ) )
                {
                    target = Path.GetDirectoryName( target );
                }
                explorerBrowser.Navigate( ShellFileSystemFolder.FromFolderPath( target ) );
            }
        }

        private void tsbtnTouch_Click( object sender, EventArgs e )
        {
            string folder = tscbVistedFolder.Text;
            //EXIF.TouchPhoto( folder, "", SearchOption.TopDirectoryOnly );
            tsProgress.Minimum = 0;
            tsProgress.Maximum = 100;
            KeyValuePair<string, SearchOption> args = new KeyValuePair<string, SearchOption>(folder, SearchOption.TopDirectoryOnly);
            Cursor = Cursors.WaitCursor;
            bgwTouchTime.RunWorkerAsync( args );
        }

        private void tsmiTouchRecursion_Click( object sender, EventArgs e )
        {
            string folder = tscbVistedFolder.Text;
            tsProgress.Minimum = 0;
            tsProgress.Maximum = 100;
            KeyValuePair<string, SearchOption> args = new KeyValuePair<string, SearchOption>(folder, SearchOption.AllDirectories);
            Cursor = Cursors.WaitCursor;
            bgwTouchTime.RunWorkerAsync( args );
        }

        private void tsmiTouchMeta_Click( object sender, EventArgs e )
        {
            //
            string folder = tscbVistedFolder.Text;
            tsProgress.Minimum = 0;
            tsProgress.Maximum = 100;
            KeyValuePair<string, SearchOption> args = new KeyValuePair<string, SearchOption>(folder, SearchOption.TopDirectoryOnly);
            Cursor = Cursors.WaitCursor;
            bgwTouchMeta.RunWorkerAsync( args );
        }

        private void tsmiTouchMetaRecursion_Click( object sender, EventArgs e )
        {
            //
            string folder = tscbVistedFolder.Text;
            tsProgress.Minimum = 0;
            tsProgress.Maximum = 100;
            KeyValuePair<string, SearchOption> args = new KeyValuePair<string, SearchOption>(folder, SearchOption.AllDirectories);
            Cursor = Cursors.WaitCursor;
            bgwTouchMeta.RunWorkerAsync( args );
        }

        private void explorerBrowser_NavigationComplete( object sender, NavigationCompleteEventArgs e )
        {
            BeginInvoke( new MethodInvoker( delegate ()
            {
                //lastVisitedFolders.Insert(0, e.NewLocation.ParsingName );
                lastVisitedFolders.Clear();
                foreach ( ShellObject location in explorerBrowser.NavigationLog.Locations )
                {
                    lastVisitedFolders.Add( location.ParsingName );
                }
                lastVisitedFolders.Reverse();

                tscbVistedFolder.Items.Clear();
                tscbVistedFolder.Items.AddRange( lastVisitedFolders.ToArray() );
                tscbVistedFolder.Text = explorerBrowser.NavigationLog.CurrentLocation.ParsingName;

                explorerBrowser.Focus();
                //tsFilesTotal.Text = $"Total: {getTotalPhotos( e.NewLocation.ParsingName )}";
                //tsFilesTotal.Text = $"Total: {getTotalPhotos( explorerBrowser.Items )}";
                //tsFilesSelected.Text = $"Selected: {0}";
                //ShowSelectedImage();
                item_changed = false;
            } ) );
        }

        private void explorerBrowser_ViewEnumerationComplete( object sender, EventArgs e )
        {
            tsFilesTotal.Text = $"Total: {getTotalPhotos( explorerBrowser.Items )}";
            tsFilesSelected.Text = $"Selected: {0}";
            ShowSelectedImage();
            item_changed = true;
        }

        private void explorerBrowser_SelectionChanged( object sender, EventArgs e )
        {
            ShowSelectedImage();
        }

        private void explorerBrowser_ItemsChanged( object sender, EventArgs e )
        {
            if ( EXIF.IsTouching ) return;
            if ( !item_changed ) return;
            BeginInvoke( new MethodInvoker( delegate ()
            {
                int total = getTotalPhotos( explorerBrowser.Items );
                int selected = getTotalPhotos( explorerBrowser.SelectedItems);
                tsFilesTotal.Text = $"Total: {total}";
                tsFilesSelected.Text = $"Selected: {selected}";
            } ) );
        }

        private void bgwTouchTime_DoWork( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            KeyValuePair<string, SearchOption> args = (KeyValuePair<string, SearchOption>)e.Argument;
            string folder = args.Key;
            SearchOption option = args.Value;

            DirectoryInfo di = new DirectoryInfo(folder);
            IEnumerable<FileInfo> fileinfos  = di.EnumerateFiles( "*", option ).Where( f => EXIF.PhotoExts.Contains( f.Extension, StringComparer.CurrentCultureIgnoreCase ) );
            int index = 0;
            int count = fileinfos.Count();
            foreach ( FileInfo file in fileinfos )
            {
                EXIF.TouchPhoto( $"{file.DirectoryName}{Path.DirectorySeparatorChar}{file.Name}", "" );
                bgwTouchTime.ReportProgress( (int) Math.Round( ( index++ ) * 100f / count ) );
            }
        }

        private void bgwTouchTime_ProgressChanged( object sender, System.ComponentModel.ProgressChangedEventArgs e )
        {
            tsProgress.Value = e.ProgressPercentage;
            tsInfo.Text = $"Touching file datetime {e.ProgressPercentage}%";
        }

        private void bgwTouchTime_RunWorkerCompleted( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e )
        {
            tsProgress.Value = tsProgress.Maximum;
            Cursor = Cursors.Default;
            tsInfo.Text = $"Touching file(s) datetime 100%";
        }

        private void bgwTouchMeta_DoWork( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            KeyValuePair<string, SearchOption> args = (KeyValuePair<string, SearchOption>)e.Argument;
            string folder = args.Key;
            SearchOption option = args.Value;

            DirectoryInfo di = new DirectoryInfo(folder);
            IEnumerable<FileInfo> fileinfos  = di.EnumerateFiles( "*", option ).Where( f => EXIF.PhotoExts.Contains( f.Extension, StringComparer.CurrentCultureIgnoreCase ) );
            int index = 0;
            int count = fileinfos.Count();
            foreach ( FileInfo file in fileinfos )
            {
                EXIF.TouchPhoto( $"{file.DirectoryName}{Path.DirectorySeparatorChar}{file.Name}", "" );
                bgwTouchMeta.ReportProgress( (int) Math.Round( ( index++ ) * 100f / count ) );
            }
        }

        private void bgwTouchMeta_ProgressChanged( object sender, System.ComponentModel.ProgressChangedEventArgs e )
        {
            tsProgress.Value = e.ProgressPercentage;
            tsInfo.Text = $"Touching file datetime {e.ProgressPercentage}%";
        }

        private void bgwTouchMeta_RunWorkerCompleted( object sender, System.ComponentModel.RunWorkerCompletedEventArgs e )
        {
            tsProgress.Value = tsProgress.Maximum;
            Cursor = Cursors.Default;
            tsInfo.Text = $"Touching file(s) metadata 100%";
        }
    }
}
