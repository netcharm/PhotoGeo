using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace PhotoGeoTag
{
    public partial class MainForm : Form
    {
        string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);
        string CacheFolder = "";

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

                    double pos_Lat = double.NaN, pos_Lng = double.NaN;

                    Image photo = new Bitmap(p.FullName);
                    pos_Lat = ImageGeoTag.GetLatitude( photo );
                    pos_Lng = ImageGeoTag.GetLongitude( photo );
                    photo.Dispose();

                    if ( double.IsNaN( pos_Lat ) || double.IsNaN( pos_Lng ) ) continue;
                    
                    item.SetSubItemText( 0, pos_Lng.ToString( "F8" ) );
                    item.SetSubItemText( 1, pos_Lat.ToString( "F8" ) );
                    item.Checked = true;

                    //ImageList_SetOverlayImage()
                }
            }
            lvImage.ResumeLayout();
        }

        private void drawOverlay(Graphics g, Rectangle bounds)
        {

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
            foreach ( int viewmode in Enum.GetValues( typeof( Manina.Windows.Forms.View ) ) )
            {
                tscbViewMode.Items.Add( (Manina.Windows.Forms.View) Enum.Parse( typeof( Manina.Windows.Forms.View ), viewmode.ToString() ) );
            }
            tscbViewMode.SelectedIndex = 3;

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
            //render.DrawOverlay += drawOverlay;
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
            lvImage.Columns.Add( ColumnType.ImageDescription );
            lvImage.Columns.Add( ColumnType.Artist );
            lvImage.Columns.Add( ColumnType.Copyright );
            lvImage.Columns.Add( ColumnType.UserComment );
            lvImage.Columns.Add( ColumnType.Software );
            lvImage.Columns.Add( ColumnType.Custom ); // Geo Tag: Longitude
            lvImage.Columns.Add( ColumnType.Custom ); // Geo Tag: Latitude

            //
            PopulateListView( new DirectoryInfo( AppFolder ) );

            //
            explorerTree.Go( AppFolder );
            //explorerTree.setCurrentPath( AppFolder );
            //explorerTree.SelectedPath = AppFolder;
            //explorerTree.btnGo_Click( this, e );
            //explorerTree.ShowLines = false;
        }

        private void explorerTree_PathChanged( object sender, EventArgs e )
        {
            PopulateListView( new DirectoryInfo( explorerTree.SelectedPath ) );
            tsProgress.Maximum = lvImage.Items.Count;
        }

        private void tsbtnMapView_Click( object sender, EventArgs e )
        {
            //FormMap fm = (FormMap)Application.OpenForms[MapViewer.Text];
            try
            {
                if ( MapViewer == null) { MapViewer = new FormMap();  }
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

        private void lvImage_SelectionChanged( object sender, EventArgs e )
        {
            List<KeyValuePair<Image, string>> imgs = new List<KeyValuePair<Image, string>>();
            foreach ( ImageListViewItem item in lvImage.SelectedItems )
            {
                string d = item.FileName;
                if ( !File.GetAttributes( d ).HasFlag( FileAttributes.Directory ) )
                {
                    imgs.Add( new KeyValuePair<Image, string>( item.GetCachedImage( CachedImageType.Thumbnail ), item.FileName ) );
                }
                else
                {
                    item.Selected = false;
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

    }
}
