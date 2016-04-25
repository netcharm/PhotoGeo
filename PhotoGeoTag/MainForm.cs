using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace PhotoGeoTag
{
    public partial class MainForm : Form
    {
        string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);
        string CacheFolder = "";

        Dictionary<string, Guid> mapSource = new Dictionary<string, Guid>();

        MarsWGS PosShift = new MarsWGS();
        bool MapShift = false;

        GMapOverlay OverlayRefPos = new GMapOverlay("RefPos");
        GMapOverlay OverlayPhotos = new GMapOverlay("Photos");
        GMapOverlay OverlayPoints = new GMapOverlay("Points");
        GMapOverlay OverlayTracks = new GMapOverlay("Tracks");
        GMapOverlay OverlayRoutes = new GMapOverlay("Routes");

        GMapOverlay OverlayRefPosWGS = new GMapOverlay("RefPos");
        GMapOverlay OverlayPhotosWGS = new GMapOverlay("Photos");
        GMapOverlay OverlayPointsWGS = new GMapOverlay("Points");
        GMapOverlay OverlayTracksWGS = new GMapOverlay("Tracks");
        GMapOverlay OverlayRoutesWGS = new GMapOverlay("Routes");

        GMapOverlay OverlayRefPosMAR = new GMapOverlay("RefPos");
        GMapOverlay OverlayPhotosMAR = new GMapOverlay("Photos");
        GMapOverlay OverlayPointsMAR = new GMapOverlay("Points");
        GMapOverlay OverlayTracksMAR = new GMapOverlay("Tracks");
        GMapOverlay OverlayRoutesMAR = new GMapOverlay("Routes");

        private void updatePositions( GMapOverlay overlay )
        {
            string mapName = gMap.MapProvider.Name;
            //MapShift = chkMapShift.Checked;
            if ( mapName.StartsWith( "GoogleChina" ) || mapName.StartsWith( "BingMap" ) )
            {
                MapShift = true;
                overlay.Markers.Clear();
                if ( string.Equals( overlay.Id, "RefPos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayRefPosMAR.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Photos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayPhotosMAR.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Points", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayPointsMAR.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Tracks", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayTracksMAR.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayRoutesMAR.Markers )
                        overlay.Markers.Add( marker );
                }
            }
            else
            {
                MapShift = false;
                overlay.Markers.Clear();
                if ( string.Equals( overlay.Id, "RefPos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayRefPosWGS.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Photos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayPhotosWGS.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Points", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayPointsWGS.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Tracks", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayTracksWGS.Markers )
                        overlay.Markers.Add( marker );
                }
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMarkerGoogle marker in OverlayRoutesWGS.Markers )
                        overlay.Markers.Add( marker );
                }

            }
        }

        private void updatePositions()
        {
            foreach ( GMapOverlay overlay in gMap.Overlays )
            {
                updatePositions( overlay );
            }
        }

        private Bitmap getPhotoThumb(Bitmap image)
        {
            int w=64, h=64;
            double ar = (double)image.Width / (double)image.Height;
            if ( ar > 1 )
            {
                h = (int) ( w / ar );
            }
            else if ( ar < 1 )
            {
                w = (int) ( h * ar );
            }
            return ( new Bitmap( image, w, h ) );
        }

        private Bitmap getPhotoThumb(Image image)
        {
            return ( getPhotoThumb( (Bitmap) image ) );
        }

        public MainForm()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Icon = Icon.ExtractAssociatedIcon( Application.ExecutablePath );
        }

        private void Form1_Load( object sender, EventArgs e )
        {
            CacheFolder = AppFolder + Path.DirectorySeparatorChar + "Cache";
            
            cbMapProviders.Items.Clear();
            cbMapProviders.BeginUpdate();
            foreach ( GMapProvider map in GMapProviders.List )
            {
                if ( map.Name.StartsWith( "Bing" ) ||
                     map.Name.StartsWith( "Google" ) ||
                     //map.Name.StartsWith( "Yahoo" ) ||
                     map.Name.StartsWith( "Open" ) )
                {
                    cbMapProviders.Items.Add( map.Name );
                    mapSource.Add( map.Name, map.Id );
                }
            }
            cbMapProviders.EndUpdate();
            cbMapProviders.SelectedIndex = cbMapProviders.Items.IndexOf( "GoogleChinaHybridMap" );
            if(Directory.Exists( CacheFolder ))
            {
                Directory.CreateDirectory( CacheFolder );
            }
            trackZoom.Minimum = 2;
            trackZoom.Maximum = 20;
            trackZoom.Value = 12;

            picGeoRef.AllowDrop = true;

            gMap.Manager.BoostCacheEngine = true;
            gMap.Manager.CacheOnIdleRead = true;
            gMap.Manager.UseDirectionsCache = true;
            gMap.Manager.UseGeocoderCache = true;
            gMap.Manager.UseMemoryCache = true;
            gMap.Manager.UsePlacemarkCache = true;
            gMap.Manager.UseRouteCache = true;
            gMap.Manager.UseUrlCache = true;
            gMap.CanDragMap = true;
            gMap.DragButton = MouseButtons.Left;
            gMap.FillEmptyTiles = true;
            gMap.MapScaleInfoEnabled = true;
            gMap.Manager.Mode = AccessMode.ServerAndCache;
            gMap.MaxZoom = trackZoom.Maximum;
            gMap.MinZoom = trackZoom.Minimum;
            //gMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            gMap.RetryLoadTile = 5;
            gMap.ScaleMode = ScaleModes.Fractional;
            gMap.ScalePen = Pens.AntiqueWhite;
            gMap.ShowCenter = false;
            //GeocodingProvider

            gMap.CacheLocation = CacheFolder;
            gMap.MapProvider = GMapProviders.TryGetProvider( mapSource["GoogleChinaHybridMap"] );
            //gMap.MapProvider = GoogleChinaHybridMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMap.SetPositionByKeywords( "Sanya,China" );
            gMap.Zoom = trackZoom.Value;

            gMap.Overlays.Add( OverlayRefPos );
            gMap.Overlays.Add( OverlayPhotos );
            gMap.Overlays.Add( OverlayPoints );
            gMap.Overlays.Add( OverlayTracks );
            gMap.Overlays.Add( OverlayRoutes );
        }

        private void cbMapProviders_SelectedIndexChanged( object sender, EventArgs e )
        {
            //RectLatLng? latlng = gMap.BoundsOfMap;
            PointLatLng CurrentPos = gMap.Position;
            gMap.MapProvider = GMapProviders.TryGetProvider( mapSource[cbMapProviders.SelectedItem.ToString()] );
            //gMap.BoundsOfMap = latlng;
        }

        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            gMap.MapProvider = EmptyProvider.Instance;
            gMap.MapProvider.BypassCache = true;
            gMap.Manager.CancelTileCaching();
        }

        private void trackZoom_Scroll( object sender, EventArgs e )
        {
            gMap.Zoom = trackZoom.Value;
            tsZoom.Text = $"Zoom: {gMap.Zoom.ToString()}";
            tsZoom.ToolTipText = tsZoom.Text;           
        }

        private void gMap_OnMapZoomChanged()
        {
            trackZoom.Value = (int)gMap.Zoom;
            tsZoom.Text = $"Zoom: {gMap.Zoom.ToString()}";
            tsZoom.ToolTipText = tsZoom.Text;
        }

        private void gMap_OnPositionChanged( PointLatLng point )
        {
            double lat = point.Lat;
            double lng = point.Lng;
            
            if ( MapShift)
            {
                PosShift.Convert2WGS( point.Lng, point.Lat, out lng, out lat );
            }
            string refLat = lat < 0 ? "S" : "N";
            string refLng = lng < 0 ? "W" : "E";
            tsLat.Text = $"Lat: {lat.ToString( "F6" )} {refLat}";
            tsLon.Text = $"Lon: {lng.ToString( "F6" )} {refLng}";
        }

        private void picGeoRef_DragEnter( object sender, DragEventArgs e )
        {
            e.Effect = DragDropEffects.Link;
        }

        private void picGeoRef_DragDrop( object sender, DragEventArgs e )
        {
            PointLatLng pos = gMap.Position;

            string[] flist = (string[])e.Data.GetData( DataFormats.FileDrop, true );
            picGeoRef.Load( flist[0] );

            pos.Lat = ImageGeoTag.GetLatitude( picGeoRef.Image, pos.Lat );
            pos.Lng = ImageGeoTag.GetLongitude( picGeoRef.Image, pos.Lng );

            double lat = pos.Lat, lng = pos.Lng;
            PosShift.Convert2Mars( pos.Lng, pos.Lat, out lng, out lat );

            OverlayRefPosWGS.Markers.Clear();
            //OverlayRefPosWGS.Markers.Add( new GMarkerGoogle( pos, GMarkerGoogleType.blue_pushpin ) );
            OverlayRefPosWGS.Markers.Add( new GMarkerGoogle( pos, getPhotoThumb( picGeoRef.Image ) ) );

            OverlayRefPosMAR.Markers.Clear();
            //OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng(lat, lng), GMarkerGoogleType.blue_pushpin ) );
            OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng( lat, lng ), getPhotoThumb( picGeoRef.Image ) ) );

            gMap.Zoom = 12;

            updatePositions();
        }

        private void gMap_OnMapTypeChanged( GMapProvider type )
        {
            //
            updatePositions();
        }
    }
}
