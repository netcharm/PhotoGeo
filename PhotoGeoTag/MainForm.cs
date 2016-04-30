using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace PhotoGeoTagShell
{
    public partial class MainForm : Form
    {
        string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);
        string CacheFolder = "";
        List<string> lastVisitedFolders = new List<string>();
        Dictionary<string, string> appSettings = new Dictionary<string, string>();

        FormMap MapViewer;//= new FormMap();

        private void PopulateListView( DirectoryInfo path )
        {
            lvImage.Items.Clear();
            lvImage.SuspendLayout();
            //int i = 0;

            foreach ( DirectoryInfo d in path.GetDirectories( "*.*", SearchOption.TopDirectoryOnly ) )
            {
                lvImage.Items.Add( d.FullName, Properties.Resources.ImageFolder );
                lvImage.Items[lvImage.Items.Count - 1].Text = d.Name;
            }
            foreach ( FileInfo p in path.GetFiles( "*.*", SearchOption.TopDirectoryOnly ) )
            {
                if ( p.Name.EndsWith( ".jpg", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".jpeg", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".png", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".bmp", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".ico", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".cur", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".emf", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".wmf", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".tif", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".tiff", StringComparison.OrdinalIgnoreCase ) ||
                     p.Name.EndsWith( ".gif", StringComparison.OrdinalIgnoreCase ) )
                {
                    lvImage.Items.Add( p.FullName );
                    ImageListViewItem item = lvImage.Items[lvImage.Items.Count - 1];
                    item.Checked = false;

                    //Image photo = new Bitmap(p.FullName);
                    double pos_Lat = double.NaN, pos_Lng = double.NaN;
                    using ( FileStream fs = new FileStream( p.FullName, FileMode.Open, FileAccess.Read ) )
                    {
                        using ( Image photo = Image.FromStream( fs ) )
                        {
                            pos_Lat = ImageGeoTag.GetLatitude( photo );
                            pos_Lng = ImageGeoTag.GetLongitude( photo );

                            item.Tag = photo.PropertyItems;

                            photo.Dispose();
                        }
                        fs.Close();
                    }

                    if ( double.IsNaN( pos_Lat ) || double.IsNaN( pos_Lng ) ) continue;

                    item.SetSubItemText( 0, pos_Lng.ToString( "F8" ) );
                    item.SetSubItemText( 1, pos_Lat.ToString( "F8" ) );
                    item.Checked = true;
                }
            }
            lvImage.ResumeLayout();
        }

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

        }

        private void configSave()
        {
            if ( appSettings.ContainsKey( "lastVisitedFolder" ) )
                appSettings["lastVisitedFolder"] = tscbVistedFolder.Text;
            else appSettings.Add( "lastVisitedFolder", AppFolder );

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

        private void updateLastVisited()
        {
            lastVisitedFolders.Insert( 0, explorerTree.SelectedPath );
            tscbVistedFolder.Items.Clear();
            tscbVistedFolder.Items.AddRange( lastVisitedFolders.ToArray() );
            tscbVistedFolder.Text = explorerTree.SelectedPath;
        }

        public MainForm()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Icon = Icon.ExtractAssociatedIcon( Application.ExecutablePath );
        }

        private void MainForm_Load( object sender, EventArgs e )
        {
            //string CacheFolder = Path.Combine(
            //    Path.GetDirectoryName(new Uri(Assembly.GetAssembly(typeof(ImageListView)).GetName().CodeBase).LocalPath),
            //    "Cache"
            //    );
            CacheFolder = AppFolder + Path.DirectorySeparatorChar + "Cache";
            if ( !Directory.Exists( CacheFolder ) )
                Directory.CreateDirectory( CacheFolder );

            tscbViewMode.Items.Clear();
            tscbViewMode.Items.AddRange( Enum.GetNames( typeof( Manina.Windows.Forms.View ) ) );
            tscbViewMode.SelectedIndex = (int) Manina.Windows.Forms.View.Thumbnails;

            //
            lvImage.AllowCheckBoxClick = false;
            lvImage.AutoRotateThumbnails = true;
            lvImage.PersistentCacheDirectory = CacheFolder;
            lvImage.CacheMode = CacheMode.OnDemand;
            lvImage.IntegralScroll = true;
            lvImage.ShellIconFallback = true;
            lvImage.ShellIconFromFileContent = true;
            lvImage.ShowFileIcons = true;
            lvImage.ShowCheckBoxes = true;
            lvImage.UseEmbeddedThumbnails = UseEmbeddedThumbnails.Auto;
            lvImage.UseWIC = UseWIC.Auto;

            //lvImage.View = Manina.Windows.Forms.View.Thumbnails;
            //lvImage.View = Manina.Windows.Forms.View.Details;
            //lvImage.View = (Manina.Windows.Forms.View) tscbViewMode.SelectedIndex;

            // Create a new TilesRenderer and set the size
            // of the description area to 180 pixels
            //lvImage.SetRenderer( new ImageListViewRenderers.TilesRenderer( 180 ) );

            // Create a new ZoomingRenderer and set the
            // zoom factor to 50%
            lvImage.SetRenderer( new ImageListViewRenderers.ZoomingRenderer( 0.5f ) );
            ImageListViewRenderers.ZoomingRenderer render = new ImageListViewRenderers.ZoomingRenderer( 0.5f );
            lvImage.Colors = ImageListViewColor.Mandarin;

            // Displays the control with a dark theme.
            //lvImage.SetRenderer( new ImageListViewRenderers.NoirRenderer() );

            //
            lvImage.Columns.Add( ColumnType.Name );
            lvImage.Columns.Add( ColumnType.Dimensions );
            lvImage.Columns.Add( ColumnType.FileSize );
            lvImage.Columns.Add( ColumnType.FolderName );
            lvImage.Columns.Add( ColumnType.DateCreated );
            lvImage.Columns.Add( ColumnType.DateTaken );
            lvImage.Columns.Add( ColumnType.EquipmentModel );
            lvImage.Columns.Add( ColumnType.FNumber );
            lvImage.Columns.Add( ColumnType.ExposureTime );
            lvImage.Columns.Add( ColumnType.ISOSpeed );
            lvImage.Columns.Add( ColumnType.FocalLength );
            lvImage.Columns.Add( ColumnType.Resolution );
            lvImage.Columns.Add( ColumnType.ImageDescription );
            lvImage.Columns.Add( ColumnType.Artist );
            lvImage.Columns.Add( ColumnType.Copyright );
            lvImage.Columns.Add( ColumnType.UserComment );
            lvImage.Columns.Add( ColumnType.Software );
            lvImage.Columns.Add( ColumnType.FileName );
            lvImage.Columns.Add( ColumnType.FileType );
            lvImage.Columns.Add( ColumnType.FilePath );
            lvImage.Columns.Add( ColumnType.Custom ); // Geo Tag: Longitude
            lvImage.Columns.Add( ColumnType.Custom ); // Geo Tag: Latitude

            //
            configLoad();

            //string lastVisited = Properties.Settings.Default.lastVisitedFolder;
            string lastVisited = appSettings["lastVisitedFolder"];
            if ( string.IsNullOrEmpty( lastVisited ) ) lastVisited = AppFolder;
            //explorerTree.setCurrentPath( lastVisited );
            //PopulateListView( lastVisited );
            explorerTree.Go( lastVisited );

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

        private void explorerTree_PathChanged( object sender, EventArgs e )
        {
            PopulateListView( new DirectoryInfo( explorerTree.SelectedPath ) );
            tsProgress.Maximum = lvImage.Items.Count;
            updateLastVisited();
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
            MapViewer.Show();
        }

        private void tscbViewMode_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( tscbViewMode.SelectedIndex >= 0 )
            {
                lvImage.View = (Manina.Windows.Forms.View) tscbViewMode.SelectedIndex;
            }
        }

        private void lvImage_ItemHover( object sender, ItemHoverEventArgs e )
        {
            if ( e.Item != null )
            {
                string d = e.Item.FileName;
                if ( File.GetAttributes( d ).HasFlag( FileAttributes.Directory ) )
                {
                    //return e.Item.;
                    e.Item.Selected = false;
                    return;
                }
            }
        }

        private void lvImage_ItemDoubleClick( object sender, ItemClickEventArgs e )
        {
            //File.GetAttributes( e.FullName ).HasFlag( FileAttributes.Directory )
            if ( e.Item != null )
            {
                string d = e.Item.FileName;
                if ( File.GetAttributes( d ).HasFlag( FileAttributes.Directory ) )
                {
                    explorerTree.Go( d );
                }
            }
        }

        private void lvImage_SelectionChanged( object sender, EventArgs e )
        {
            List<KeyValuePair<Image, string>> imgs = new List<KeyValuePair<Image, string>>();
            foreach ( ImageListViewItem item in lvImage.SelectedItems )
            {
                if ( !File.GetAttributes( item.FileName ).HasFlag( FileAttributes.Directory ) )
                {
                    Image thumb = item.GetCachedImage( CachedImageType.Thumbnail );
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    properties.Add( "Artist", item.Artist == null ? "" : item.Artist.Trim() );
                    properties.Add( "Copyright", item.Copyright == null ? "" : item.Copyright.Trim() );
                    properties.Add( "DateAccessed", item.DateAccessed == null ? "" : item.DateAccessed.ToString() );
                    properties.Add( "DateCreated", item.DateCreated == null ? "" : item.DateCreated.ToString() );
                    properties.Add( "DateModified", item.DateModified == null ? "" : item.DateModified.ToString() );
                    properties.Add( "DateTaken", item.DateTaken == null ? "" : item.DateTaken.ToString() );
                    properties.Add( "Dimensions", item.Dimensions == null ? "" : item.Dimensions.ToString() );
                    properties.Add( "EquipmentModel", item.EquipmentModel == null ? "" : item.EquipmentModel.Trim() );
                    properties.Add( "ExposureTime", item.ExposureTime.ToString() );
                    properties.Add( "FileName", item.FileName == null ? "" : item.FileName.Trim() );
                    properties.Add( "FilePath", item.FilePath == null ? "" : item.FilePath.Trim() );
                    properties.Add( "FileSize", item.FileSize.ToString() );
                    properties.Add( "FileType", item.FileType == null ? "" : item.FileType.Trim() );
                    properties.Add( "FNumber", item.FNumber.ToString() );
                    properties.Add( "FocalLength", item.FocalLength.ToString() );
                    properties.Add( "FolderName", item.FolderName == null ? "" : item.FolderName.Trim() );
                    properties.Add( "ImageDescription", item.ImageDescription == null ? "" : item.ImageDescription.Trim() );
                    properties.Add( "ISOSpeed", item.ISOSpeed.ToString() );
                    properties.Add( "Rating", item.Rating.ToString() );
                    properties.Add( "Resolution", item.Resolution == null ? "" : item.Resolution.ToString() );
                    properties.Add( "Software", item.Software == null ? "" : item.Software.Trim() );
                    properties.Add( "StarRating", item.StarRating.ToString() );
                    properties.Add( "UserComment", item.UserComment == null ? "" : item.UserComment.Trim() );
                    thumb.Tag = properties;
                    if ( item.Tag != null )
                    {
                        foreach ( PropertyItem propitem in (PropertyItem[]) item.Tag )
                        {
                            thumb.SetPropertyItem( propitem );
                        }
                        //thumb.Tag = item;
                    }
                    imgs.Add( new KeyValuePair<Image, string>( thumb, item.FileName ) );
                }
                else
                {
                    if ( lvImage.SelectedItems.Count > 1 )
                    {
                        item.Selected = false;
                    }
                }
            }
            try
            {
                if ( MapViewer != null && MapViewer.Visible )
                {
                    MapViewer.ShowImage( imgs );
                }
            }
            catch { }
        }

        private void lvImage_KeyPress( object sender, KeyPressEventArgs e )
        {
        }

        private void lvImage_KeyUp( object sender, KeyEventArgs e )
        {
            if ( e.Control && e.KeyCode == Keys.A )
            {
                lvImage.SelectAll();
            }
            else if ( e.KeyCode == Keys.Enter )
            {
                foreach ( ImageListViewItem item in lvImage.SelectedItems )
                {
                    string d = item.FileName;
                    if ( File.GetAttributes( d ).HasFlag( FileAttributes.Directory ) )
                    {
                        explorerTree.Go( d );
                        //explorerTree.setCurrentPath( d );
                        break;
                    }
                }
            }
            else if ( e.KeyCode == Keys.Back || e.KeyCode == Keys.BrowserBack )
            {
                string target = Path.GetDirectoryName( explorerTree.SelectedPath );
                //if ( lastVisitedFolders.Count > 1 ) target = lastVisitedFolders[1];
                //explorerTree.setCurrentPath( target );
                explorerTree.Go( target );
            }
        }
    }
}
