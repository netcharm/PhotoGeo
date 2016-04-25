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
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
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

        private GeocodingProvider Geo;

        private GMapOverlay OverlayRefPos = new GMapOverlay("RefPos");
        private GMapOverlay OverlayPhotos = new GMapOverlay("Photos");
        private GMapOverlay OverlayPoints = new GMapOverlay("Points");
        private GMapOverlay OverlayRoutes = new GMapOverlay("Routes");

        private GMapOverlay OverlayRefPosWGS = new GMapOverlay("RefPos");
        private GMapOverlay OverlayPhotosWGS = new GMapOverlay("Photos");
        private GMapOverlay OverlayPointsWGS = new GMapOverlay("Points");
        private GMapOverlay OverlayRoutesWGS = new GMapOverlay("Routes");

        private GMapOverlay OverlayRefPosMAR = new GMapOverlay("RefPos");
        private GMapOverlay OverlayPhotosMAR = new GMapOverlay("Photos");
        private GMapOverlay OverlayPointsMAR = new GMapOverlay("Points");
        private GMapOverlay OverlayRoutesMAR = new GMapOverlay("Routes");

        private void updatePositions( GMapOverlay overlay, bool force=false )
        {
            if ( MapShift == chkMapShift.Checked && !force ) return;

            string mapName = gMap.MapProvider.Name;

            MapShift = chkMapShift.Checked;
            //if ( mapName.StartsWith( "GoogleChina" ) || mapName.StartsWith( "BingMap" ) )
            if(MapShift)
            {
                //MapShift = true;
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
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMapRoute route in OverlayRoutesMAR.Routes )
                        overlay.Routes.Add( route );
                }
            }
            else
            {
                //MapShift = false;
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
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    foreach ( GMapRoute route in OverlayRoutesWGS.Routes )
                        overlay.Routes.Add( route );
                }

            }
        }

        private void updatePositions( bool force = false )
        {
            foreach ( GMapOverlay overlay in gMap.Overlays )
            {
                updatePositions( overlay, force );
            }
        }

        private void updateMarkerOffset( GMapOverlay overlay, bool force = false )
        {
            double lng_wgs = 0, lat_wgs = 0;
            double lng_mar = 0, lat_mar = 0;

            if ( MapShift == chkMapShift.Checked && !force ) return;

            string mapName = gMap.MapProvider.Name;

            MapShift = chkMapShift.Checked;

            foreach ( GMarkerGoogle marker in overlay.Markers )
            {
                double offset_x = 0, offset_y = 0;
                if ( MapShift )
                {
                    lng_wgs = marker.Position.Lng;
                    lat_wgs = marker.Position.Lat;

                    PosShift.Convert2Mars( lng_wgs, lat_wgs, out lng_mar, out lat_mar );

                    GPoint pixel_wgs = gMap.MapProvider.Projection.FromLatLngToPixel(lat_wgs, lng_wgs, (int)gMap.Zoom);
                    GPoint pixel_mar = gMap.MapProvider.Projection.FromLatLngToPixel(lat_mar, lng_mar, (int)gMap.Zoom);

                    double res = gMap.MapProvider.Projection.GetGroundResolution( (int)gMap.Zoom, lat_wgs );

                    offset_x = pixel_mar.X - pixel_wgs.X;
                    offset_y = pixel_mar.Y - pixel_wgs.Y;
                }
                marker.Offset = new Point( (int) offset_x, (int) offset_y );
            }

            //double offset_x = 0, offset_y = 0;
            //if ( MapShift )
            //{
            //    lng_wgs = gMap.Position.Lng;
            //    lat_wgs = gMap.Position.Lat;

            //    PosShift.Convert2Mars( lng_wgs, lat_wgs, out lng_mar, out lat_mar );

            //    GPoint pixel_wgs = gMap.MapProvider.Projection.FromLatLngToPixel(lat_wgs, lng_wgs, (int)gMap.Zoom);
            //    GPoint pixel_mar = gMap.MapProvider.Projection.FromLatLngToPixel(lat_mar, lng_mar, (int)gMap.Zoom);

            //    double res = gMap.MapProvider.Projection.GetGroundResolution( (int)gMap.Zoom, lat_wgs );

            //    offset_x = pixel_mar.X - pixel_wgs.X;
            //    offset_y = pixel_mar.Y - pixel_wgs.Y;
            //}
            //gMap.Offset( (int) offset_x, (int) offset_y );
        }

        private void updateMarkerOffset( bool force = false )
        {
            foreach ( GMapOverlay overlay in gMap.Overlays )
            {
                updateMarkerOffset( overlay, force );
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
                     (map.Name.StartsWith( "Google" ) && !map.Name.StartsWith( "GoogleKorea" ) ) ||
                     //map.Name.StartsWith( "Yahoo" ) ||
                     map.Name.StartsWith( "Open" ) ||
                     map.Name.StartsWith( "Ovi" ) ||
                     map.Name.StartsWith( "Yandex" )
                     )
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
            //gMap.ScaleMode = ScaleModes.Fractional;
            gMap.ScaleMode = ScaleModes.Integer;
            gMap.ScalePen = Pens.AntiqueWhite;
            gMap.ShowCenter = false;
            gMap.Zoom = trackZoom.Value;

            gMap.CacheLocation = CacheFolder;
            gMap.MapProvider = GMapProviders.TryGetProvider( mapSource["GoogleChinaHybridMap"] );
            //gMap.MapProvider = GoogleChinaHybridMapProvider.Instance;
            string refurl = gMap.MapProvider.RefererUrl;
//            GMapsProvider.TimeoutMs = 10000;

            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMap.SetPositionByKeywords( "Sanya,China" );

            gMap.MapProvider.MaxZoom = gMap.MaxZoom;
            gMap.MapProvider.MinZoom = gMap.MinZoom;

            gMap.Overlays.Add( OverlayRefPos );
            gMap.Overlays.Add( OverlayPhotos );
            gMap.Overlays.Add( OverlayPoints );
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

            // updateMarkerOffset( OverlayRefPos, true );
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
            //OverlayRefPosWGS.Markers.Add( new GMarkerGoogle( pos, getPhotoThumb( picGeoRef.Image ) ) );
            GMarkerGoogle marker_wgs = new GMarkerGoogle( pos, GMarkerGoogleType.lightblue_dot );
            marker_wgs.ToolTip = new GMapBaloonToolTip( marker_wgs );
            marker_wgs.ToolTip.Stroke = Pens.Violet;
            marker_wgs.ToolTip.Fill = Brushes.Snow; //new SolidBrush(Color.WhiteSmoke);
            //marker.ToolTipText = "<html><body><img src=\"./P4083508.jpg\" /></body></html>";
            marker_wgs.ToolTipText = flist[0];
            OverlayRefPosWGS.Markers.Add( marker_wgs );
            
            OverlayRefPosMAR.Markers.Clear();
            //OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng(lat, lng), GMarkerGoogleType.blue_pushpin ) );
            //OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng( lat, lng ), getPhotoThumb( picGeoRef.Image ) ) );
            GMarkerGoogle marker_mar = new GMarkerGoogle( new PointLatLng( lat, lng ), GMarkerGoogleType.orange_dot );
            marker_mar.ToolTip = new GMapRoundedToolTip( marker_mar );
            marker_mar.ToolTip.Stroke = Pens.SlateBlue;
            marker_mar.ToolTip.Fill = Brushes.Snow;
            //markermar.ToolTipText = "<html><body><img src=\"./P4083508.jpg\" /></body></html>";
            marker_mar.ToolTipText = flist[0];
            OverlayRefPosMAR.Markers.Add( marker_mar );

            //gMap.Zoom = 12;

            updatePositions( OverlayRefPos, true );

            //OverlayRefPos.Markers.Add( marker_wgs );
            //updateMarkerOffset( OverlayRefPos, true );
        }

        private void gMap_OnMapTypeChanged( GMapProvider type )
        {
            //GeocodingProvider
            Geo = gMap.MapProvider as GeocodingProvider;

            updatePositions();
            //updateMarkerOffset();
        }

        private void chkMapShift_CheckedChanged( object sender, EventArgs e )
        {
            updatePositions( true );
            //updateMarkerOffset( true );
        }

        private void gMap_OnTileLoadStart()
        {
            tsProgress.Value = 0;
        }

        private void gMap_OnTileLoadComplete( long ElapsedMilliseconds )
        {
            tsInfo.Text = $"Load Time: { (ElapsedMilliseconds / 1000F).ToString("F6") } s";               
            //tsProgress.Value = 100;
        }
    }
}
