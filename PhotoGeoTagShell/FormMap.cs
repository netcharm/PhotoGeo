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
using GMap.NET.GMap.NET.MapProviders.AMap;
using GMap.NET.GMap.NET.MapProviders.Baidu;
using GMap.NET.GMap.NET.MapProviders.Sohu;
using GMap.NET.GMap.NET.MapProviders.Soso;

namespace PhotoGeoTagShell
{
    public partial class FormMap : Form
    {
        string AppFolder = Path.GetDirectoryName(Application.ExecutablePath);
        string CacheFolder = "";

        Dictionary<string, Guid> mapSource = new Dictionary<string, Guid>();

        MarsWGS PosShift = new MarsWGS();
        bool MapShift = false;

        private GeocodingProvider Geo;

        private AMapProvider AMap = AMapProvider.Instance;
        private AMapSateliteProvider AMapSatelite = AMapSateliteProvider.Instance;

        private BaiduMapProvider BaiduMap = BaiduMapProvider.Instance;
        private BaiduSateliteMapProvider BaiduSateliteMap = BaiduSateliteMapProvider.Instance;

        private SogouMapProvider SogouMap = SogouMapProvider.Instance;
        private SogouSateliteMapProvider SogouSateliteMap = SogouSateliteMapProvider.Instance;

        private SosoMapProvider SosoMap = SosoMapProvider.Instance;
        private SosoSateliteMapProvider SosoSateliteMap = SosoSateliteMapProvider.Instance;

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

        List<KeyValuePair<Image, string>> photos = new List<KeyValuePair<Image, string>>();
        GMarkerGoogle currentMarker;
        bool mouse_down = false;

        private void updatePositions( GMapOverlay overlay, bool force = false, bool fit = true )
        {
            if ( MapShift == chkMapShift.Checked && !force ) return;

            string mapName = gMap.MapProvider.Name;

            MapShift = chkMapShift.Checked;
            #region update marker position
            if ( MapShift )
            {
                if ( string.Equals( overlay.Id, "RefPos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayRefPosMAR.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Photos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayPhotosMAR.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Points", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayPointsMAR.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Routes.Clear();
                    foreach ( GMapRoute route in OverlayRoutesMAR.Routes )
                    {
                        overlay.Routes.Add( route );
                    }
                }
            }
            else
            {
                if ( string.Equals( overlay.Id, "RefPos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayRefPosWGS.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Photos", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayPhotosWGS.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Points", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Markers.Clear();
                    foreach ( GMarkerGoogle marker in OverlayPointsWGS.Markers )
                    {
                        overlay.Markers.Add( marker );
                    }
                }
                else if ( string.Equals( overlay.Id, "Routes", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    overlay.Routes.Clear();
                    foreach ( GMapRoute route in OverlayRoutesWGS.Routes )
                    {
                        overlay.Routes.Add( route );
                    }
                }
            }
            #endregion

            #region zoom fit map
            if ( fit )
            {
                gMap.ZoomAndCenterMarkers( overlay.Id );
                HashSet<PointLatLng> hash = new HashSet<PointLatLng>();
                foreach( GMarkerGoogle marker in overlay.Markers)
                {
                    //hash.Add( marker.Position );
                    PointLatLng pos = new PointLatLng(Math.Round(marker.Position.Lat, 1),
                                                      Math.Round(marker.Position.Lng, 1));
                    hash.Add( pos );
                }
                if ( hash.Count == 1 && overlay.Routes.Count == 0 && overlay.Polygons.Count == 0 )
                {
                    gMap.Zoom = 14;
                }
            }
            #endregion
        }

        private void updatePositions( bool force = false )
        {
            foreach ( GMapOverlay overlay in gMap.Overlays )
            {
                updatePositions( overlay, force );
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

        public void ShowImage( KeyValuePair<Image, string> img)
        {
            PointLatLng pos = gMap.Position;

            Image photo = new Bitmap(img.Value);
            pos.Lat = ImageGeoTag.GetLatitude( photo );
            pos.Lng = ImageGeoTag.GetLongitude( photo );
            photo.Dispose();

            if ( double.IsNaN( pos.Lat ) || double.IsNaN( pos.Lng ) ) return;

            double lat = pos.Lat, lng = pos.Lng;
            PosShift.Convert2Mars( pos.Lng, pos.Lat, out lng, out lat );

            OverlayRefPosWGS.Markers.Clear();
            GMarkerGoogle marker_wgs = new GMarkerGoogle( pos, GMarkerGoogleType.pink_dot );
            marker_wgs.ToolTip = new GMapBaloonToolTip( marker_wgs );
            marker_wgs.ToolTip.Stroke = new Pen(Color.Violet);
            marker_wgs.ToolTip.Fill = new SolidBrush(Color.Snow); //new SolidBrush(Color.WhiteSmoke);
            marker_wgs.ToolTipText = Path.GetFileName( img.Value );
            OverlayRefPosWGS.Markers.Add( marker_wgs );

            OverlayRefPosMAR.Markers.Clear();
            GMarkerGoogle marker_mar = new GMarkerGoogle( new PointLatLng( lat, lng ), GMarkerGoogleType.pink_dot );
            marker_mar.ToolTip = new GMapBaloonToolTip( marker_mar );
            marker_mar.ToolTip.Stroke = new Pen(Color.SlateBlue);
            marker_mar.ToolTip.Fill = new SolidBrush( Color.Snow);
            marker_mar.ToolTipText = Path.GetFileName( img.Value );
            OverlayRefPosMAR.Markers.Add( marker_mar );

            updatePositions( OverlayPhotos, true );
        }

        public void ShowImage( List<KeyValuePair<Image, string>> imgs )
        {
            PointLatLng pos = gMap.Position;

            photos.Clear();
            photos.AddRange( imgs );

            OverlayPhotosWGS.Markers.Clear();
            OverlayPhotosMAR.Markers.Clear();
            foreach ( KeyValuePair<Image, string> img in imgs)
            {
                Image photo = new Bitmap(img.Value);

                pos.Lat = ImageGeoTag.GetLatitude( photo );
                pos.Lng = ImageGeoTag.GetLongitude( photo );

                photo.Dispose();

                if ( double.IsNaN( pos.Lat ) || double.IsNaN( pos.Lng ) ) continue;

                double lat = pos.Lat, lng = pos.Lng;
                PosShift.Convert2Mars( pos.Lng, pos.Lat, out lng, out lat );

                GMarkerGoogle marker_wgs = new GMarkerGoogle( pos, GMarkerGoogleType.green_dot );
                marker_wgs.ToolTip = new GMapBaloonToolTip( marker_wgs );
                marker_wgs.ToolTip.Stroke = new Pen( Color.SlateBlue );
                marker_wgs.ToolTip.Fill = new SolidBrush( Color.Snow );
                marker_wgs.ToolTipText = Path.GetFileName(img.Value);
                //marker_wgs.ToolTipText += img.Key.PropertyItems[];
                OverlayPhotosWGS.Markers.Add( marker_wgs );

                GMarkerGoogle marker_mar = new GMarkerGoogle( new PointLatLng( lat, lng ), GMarkerGoogleType.green_dot );
                marker_mar.ToolTip = new GMapBaloonToolTip( marker_mar );
                marker_mar.ToolTip.Stroke = new Pen(Color.SlateBlue);
                marker_mar.ToolTip.Fill = new SolidBrush( Color.Snow);
                marker_mar.ToolTipText = img.Value;
                OverlayPhotosMAR.Markers.Add( marker_mar );
            }
            updatePositions( OverlayPhotos, true );
        }

        private ImageCodecInfo GetEncoder( ImageFormat format )
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach ( ImageCodecInfo codec in codecs )
            {
                if ( codec.FormatID == format.Guid )
                {
                    return codec;
                }
            }
            return null;
        }

        public void SetImageGeoTag( PointLatLng pos )
        {
            //PointLatLng pos = gMap.Position;
            //return;

            OverlayPhotosWGS.Markers.Clear();
            OverlayPhotosMAR.Markers.Clear();
            foreach ( KeyValuePair<Image, string> img in photos )
            {
                double lat = pos.Lat, lng = pos.Lng;
                double lat_mar = lat, lng_mar = lng;
                double lat_wgs = lat, lng_wgs = lng;
                if(chkMapShift.Checked)
                {
                    PosShift.Convert2WGS( pos.Lng, pos.Lat, out lng, out lat );
                    lat_wgs = lat;
                    lng_wgs = lng;
                }
                else
                {
                    PosShift.Convert2Mars( pos.Lng, pos.Lat, out lng, out lat );
                    lat_mar = lat;
                    lng_mar = lng;
                }

                //Image photo = new Bitmap(img.Value);
                using ( FileStream fs = new FileStream( img.Value, FileMode.Open, FileAccess.Read ) )
                {
                    Image photo = Image.FromStream( fs, true, true );
                    photo = ImageGeoTag.Geotag( photo, lat_wgs, lng_wgs );
                    fs.Close();

                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 92L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    photo.Save(img.Value, jpgEncoder, myEncoderParameters);
                    photo.Dispose();
                }

                Dictionary<string, string> properties = (Dictionary<string, string>)img.Key.Tag;
                if(properties != null)
                {
                    FileInfo fi = new FileInfo( img.Value );
                    DateTime dt = DateTime.Now;

                    if ( !string.IsNullOrEmpty( properties["DateTaken"] ) )
                    {
                        dt = DateTime.Parse( properties["DateTaken"] );
                    }
                    else if ( !string.IsNullOrEmpty( properties["DateCreated"] ) )
                    {
                        dt = DateTime.Parse( properties["DateCreated"] );
                    }
                    else if ( !string.IsNullOrEmpty( properties["DateModified"] ) )
                    {
                        dt = DateTime.Parse( properties["DateModified"] );
                    }
                    else if ( !string.IsNullOrEmpty( properties["DateAccessed"] ) )
                    {
                        dt = DateTime.Parse( properties["DateAccessed"] );
                    }
                    fi.LastAccessTimeUtc = dt.ToUniversalTime();
                    fi.LastWriteTimeUtc = dt.ToUniversalTime();
                    fi.CreationTimeUtc = dt.ToUniversalTime();
                }

                GMarkerGoogle marker_wgs = new GMarkerGoogle( new PointLatLng(lat_wgs, lng_wgs), GMarkerGoogleType.orange_dot );
                marker_wgs.ToolTip = new GMapBaloonToolTip( marker_wgs );
                marker_wgs.ToolTip.Stroke = new Pen( Color.SlateBlue );
                marker_wgs.ToolTip.Fill = new SolidBrush( Color.Snow );
                marker_wgs.ToolTipText = Path.GetFileName( img.Value );
                OverlayPhotosWGS.Markers.Add( marker_wgs );

                GMarkerGoogle marker_mar = new GMarkerGoogle( new PointLatLng( lat_mar, lng_mar ), GMarkerGoogleType.orange_dot );
                marker_mar.ToolTip = new GMapBaloonToolTip( marker_mar );
                marker_mar.ToolTip.Stroke = new Pen( Color.SlateBlue );
                marker_mar.ToolTip.Fill = new SolidBrush( Color.Snow );
                marker_mar.ToolTipText = Path.GetFileName( img.Value );
                OverlayPhotosMAR.Markers.Add( marker_mar );
            }
            if( OverlayRefPos.Markers.Count > 0)
            {
                OverlayRefPos.Markers.RemoveAt( OverlayRefPos.Markers.Count - 1 );
            }
            updatePositions( OverlayPhotos, true );
        }

        public FormMap()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Icon = Icon.ExtractAssociatedIcon( Application.ExecutablePath );
        }

        private void FormMap_Load( object sender, EventArgs e )
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
            cbMapProviders.Items.Add( AMap.Name );
            mapSource.Add( AMap.Name, AMap.Id );
            cbMapProviders.Items.Add( AMapSatelite.Name );
            mapSource.Add( AMapSatelite.Name, AMapSatelite.Id );

            //cbMapProviders.Items.Add( BaiduMap.Name );
            //mapSource.Add( BaiduMap.Name, BaiduMap.Id );
            //cbMapProviders.Items.Add( BaiduSateliteMap.Name );
            //mapSource.Add( BaiduSateliteMap.Name, BaiduSateliteMap.Id );

            //cbMapProviders.Items.Add( SogouMap.Name );
            //mapSource.Add( SogouMap.Name, SogouMap.Id );
            //cbMapProviders.Items.Add( SogouSateliteMap.Name );
            //mapSource.Add( SogouSateliteMap.Name, SogouSateliteMap.Id );

            cbMapProviders.Items.Add( SosoMap.Name );
            mapSource.Add( SosoMap.Name, SosoMap.Id );
            cbMapProviders.Items.Add( SosoSateliteMap.Name );
            mapSource.Add( SosoSateliteMap.Name, SosoSateliteMap.Id );

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
            //gMap.ScaleMode = ScaleModes.Integer;
            gMap.ScalePen = new Pen(Color.Silver);
            gMap.ShowCenter = false;
            gMap.Zoom = trackZoom.Value;
            gMap.ForceDoubleBuffer = false;

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

        private void FormMap_FormClosing( object sender, FormClosingEventArgs e )
        {
            //gMap.MapProvider = EmptyProvider.Instance;
            //gMap.MapProvider.BypassCache = true;
            gMap.Manager.CancelTileCaching();
        }

        private void cbMapProviders_SelectedIndexChanged( object sender, EventArgs e )
        {
            //RectLatLng? latlng = gMap.BoundsOfMap;
            PointLatLng CurrentPos = gMap.Position;
            string mapName = cbMapProviders.SelectedItem.ToString();
            if ( mapName.StartsWith("AMap") ||
                 mapName.StartsWith( "Baidu" ) ||
                 mapName.StartsWith( "Sohu" ) ||
                 mapName.StartsWith( "SoSo" )
                 )
            {
                if( mapName.Equals("AMap", StringComparison.CurrentCultureIgnoreCase))
                {
                    gMap.MapProvider = AMap;
                }
                else if ( mapName.Equals( "AMapSatelite", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = AMapSatelite;
                }
                else if ( mapName.Equals( "BaiduMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = BaiduMap;
                }
                else if ( mapName.Equals( "BaiduSateliteMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = BaiduSateliteMap;
                }
                else if ( mapName.Equals( "SohuMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = SogouMap;
                }
                else if ( mapName.Equals( "SohuSateliteMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = SogouSateliteMap;
                }
                else if ( mapName.Equals( "SosoMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = SosoMap;
                }
                else if ( mapName.Equals( "SoSoSateliteMap", StringComparison.CurrentCultureIgnoreCase ) )
                {
                    gMap.MapProvider = SosoSateliteMap;
                }
            }
            else
            {
                gMap.MapProvider = GMapProviders.TryGetProvider( mapSource[mapName] );
            }
            //gMap.BoundsOfMap = latlng;
        }

        private void chkMapShift_CheckedChanged( object sender, EventArgs e )
        {
            currentMarker = null;
            mouse_down = false;
            updatePositions( true );
        }

        private void trackZoom_Scroll( object sender, EventArgs e )
        {
            gMap.Zoom = trackZoom.Value;
            tsZoom.Text = $"Zoom: {gMap.Zoom.ToString()}";
            tsZoom.ToolTipText = tsZoom.Text;
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
            marker_wgs.ToolTip.Stroke = new Pen( Color.Violet );
            marker_wgs.ToolTip.Fill = new SolidBrush( Color.Snow );
            //marker.ToolTipText = "<html><body><img src=\"./P4083508.jpg\" /></body></html>";
            marker_wgs.ToolTipText = Path.GetFileName( flist[0] );
            OverlayRefPosWGS.Markers.Add( marker_wgs );
            
            OverlayRefPosMAR.Markers.Clear();
            //OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng(lat, lng), GMarkerGoogleType.blue_pushpin ) );
            //OverlayRefPosMAR.Markers.Add( new GMarkerGoogle( new PointLatLng( lat, lng ), getPhotoThumb( picGeoRef.Image ) ) );
            GMarkerGoogle marker_mar = new GMarkerGoogle( new PointLatLng( lat, lng ), GMarkerGoogleType.orange_dot );
            marker_mar.ToolTip = new GMapBaloonToolTip( marker_mar );
            marker_mar.ToolTip.Stroke = new Pen(Color.SlateBlue);
            marker_mar.ToolTip.Fill = new SolidBrush( Color.Snow);
            //markermar.ToolTipText = "<html><body><img src=\"./P4083508.jpg\" /></body></html>";
            marker_mar.ToolTipText = Path.GetFileName( flist[0] );
            OverlayRefPosMAR.Markers.Add( marker_mar );

            //gMap.Zoom = 12;

            updatePositions( OverlayRefPos, true );
        }

        private void btnPinPhoto_Click( object sender, EventArgs e )
        {
            if ( photos.Count <= 0 ) return;

            PointLatLng pos = gMap.Position;
            GMarkerGoogle marker = new GMarkerGoogle( pos, GMarkerGoogleType.yellow );
            marker.ToolTip = new GMapBaloonToolTip( marker );
            marker.ToolTip.Stroke = new Pen( Color.SlateBlue );
            marker.ToolTip.Fill = new SolidBrush( Color.Snow );
            marker.ToolTipText = "Location you want to place photo(s)";
            OverlayRefPos.Markers.Add( marker );
        }

        private void gMap_OnMapTypeChanged( GMapProvider type )
        {
            //GeocodingProvider
            Geo = gMap.MapProvider as GeocodingProvider;

            updatePositions();
        }

        private void gMap_OnMapZoomChanged()
        {
            trackZoom.Value = (int) gMap.Zoom;
            tsZoom.Text = $"Zoom: {gMap.Zoom.ToString()}";
            tsZoom.ToolTipText = tsZoom.Text;
        }

        private void gMap_OnPositionChanged( PointLatLng point )
        {
            double lat = point.Lat;
            double lng = point.Lng;

            if ( MapShift )
            {
                PosShift.Convert2WGS( point.Lng, point.Lat, out lng, out lat );
            }
            string refLat = lat < 0 ? "S" : "N";
            string refLng = lng < 0 ? "W" : "E";
            tsLat.Text = $"Lat: {lat.ToString( "F6" )} {refLat}";
            tsLon.Text = $"Lon: {lng.ToString( "F6" )} {refLng}";
        }

        private void gMap_OnTileLoadStart()
        {
            tsProgress.Value = 0;
        }

        private void gMap_OnTileLoadComplete( long ElapsedMilliseconds )
        {
            //tsInfo.Text = $"Load Time: { (ElapsedMilliseconds / 1000F).ToString("F6") } s";               
            //tsProgress.Value = 100;
        }

        private void gMap_OnMarkerEnter( GMapMarker item )
        {
            if ( !mouse_down ) currentMarker = (GMarkerGoogle) item;
        }

        private void gMap_OnMarkerLeave( GMapMarker item )
        {
            if ( !mouse_down ) currentMarker = null;
        }

        private void gMap_OnMarkerClick( GMapMarker item, MouseEventArgs e )
        {
            //
            if (e.Button == MouseButtons.Left)
            {
                //currentMarker = (GMarkerGoogle)item;
            }

        }

        private void gMap_MouseDown( object sender, MouseEventArgs e )
        {
            mouse_down = true;
        }

        private void gMap_MouseUp( object sender, MouseEventArgs e )
        {
            mouse_down = false;
            if ( currentMarker != null )
            {
                if ( MessageBox.Show( this, "Place photo(s) to here?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == DialogResult.OK )
                {
                    //GeoImage( currentMarker.Position );
                    SetImageGeoTag( gMap.FromLocalToLatLng( e.X, e.Y ) );
                }
            }
        }

        private void gMap_MouseMove( object sender, MouseEventArgs e )
        {
            PointLatLng pos = gMap.FromLocalToLatLng( e.X, e.Y );
            double lat = pos.Lat;
            double lng = pos.Lng;
            if ( MapShift )
            {
                PosShift.Convert2WGS( pos.Lng, pos.Lat, out lng, out lat );
            }
            string refLat = lat < 0 ? "S" : "N";
            string refLng = lng < 0 ? "W" : "E";
            tsInfo.Text = $"Lat: {lat.ToString( "F6" )} {refLat}, Lon: {lng.ToString( "F6" )} {refLng}";

            if ( mouse_down && currentMarker != null )
            {
                //currentMarker.Position = new PointLatLng( lat, lng );
                currentMarker.Position = pos;
            }
        }
    }
}
