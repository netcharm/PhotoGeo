using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace PhotoGeoTag
{
    // Usage:
    // ======
    // Geotag(new Bitmap(@"C:\path\to\image.jpg"), 34, -118)
    //        .Save(@"C:\path\to\geotagged.jpg", ImageFormat.Jpeg);
    // ======
    class ImageGeoTag
    {
        // These constants come from the CIPA DC-008 standard for EXIF 2.3
        const short ExifTypeByte = 1;
        const short ExifTypeAscii = 2;
        const short ExifTypeRational = 5;

        const int ExifTagGPSVersionID = 0x0000;
        const int ExifTagGPSLatitudeRef = 0x0001;
        const int ExifTagGPSLatitude = 0x0002;
        const int ExifTagGPSLongitudeRef = 0x0003;
        const int ExifTagGPSLongitude = 0x0004;

        public static Image Geotag( Image original, double lat, double lng )
        {
            char latHemisphere = 'N';
            if ( lat < 0 )
            {
                latHemisphere = 'S';
                lat = -lat;
            }
            char lngHemisphere = 'E';
            if ( lng < 0 )
            {
                lngHemisphere = 'W';
                lng = -lng;
            }

            MemoryStream ms = new MemoryStream();
            original.Save( ms, ImageFormat.Jpeg );
            ms.Seek( 0, SeekOrigin.Begin );

            Image img = Image.FromStream(ms);
            AddProperty( img, ExifTagGPSVersionID, ExifTypeByte, new byte[] { 2, 3, 0, 0 } );
            AddProperty( img, ExifTagGPSLatitudeRef, ExifTypeAscii, new byte[] { (byte) latHemisphere, 0 } );
            AddProperty( img, ExifTagGPSLatitude, ExifTypeRational, ConvertToRationalTriplet( lat ) );
            AddProperty( img, ExifTagGPSLongitudeRef, ExifTypeAscii, new byte[] { (byte) lngHemisphere, 0 } );
            AddProperty( img, ExifTagGPSLongitude, ExifTypeRational, ConvertToRationalTriplet( lng ) );

            return img;
        }

        static byte[] ConvertToRationalTriplet( double value )
        {
            int degrees = (int)Math.Floor(value);
            value = ( value - degrees ) * 60;
            int minutes = (int)Math.Floor(value);
            value = ( value - minutes ) * 60 * 100;
            int seconds = (int)Math.Round(value);
            byte[] bytes = new byte[3 * 2 * 4]; // Degrees, minutes, and seconds, each with a numerator and a denominator, each composed of 4 bytes
            int i = 0;
            Array.Copy( BitConverter.GetBytes( degrees ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( 1 ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( minutes ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( 1 ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( seconds ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( 100 ), 0, bytes, i, 4 );
            return bytes;
        }

        static void AddProperty( Image img, int id, short type, byte[] value )
        {
            PropertyItem pi = img.PropertyItems[0];
            pi.Id = id;
            pi.Type = type;
            pi.Len = value.Length;
            pi.Value = value;
            img.SetPropertyItem( pi );
        }

        public static double GetLatitude(Image img, double Latitude=0.0)
        {
            double lat = Latitude;

            foreach ( PropertyItem metaItem in img.PropertyItems )
            {
                if ( metaItem.Id == ExifTagGPSLatitude )
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lat = degreesNumerator / degreesDenominator +
                        minutesNumerator / minutesDenominator / 60f +
                        secondsNumerator / secondsDenominator / 3600f;
                }
            }
            return ( lat );
        }

        public static double GetLongitude( Image img, double Longitude=0.0)
        {
            double lng = Longitude;

            foreach ( PropertyItem metaItem in img.PropertyItems )
            {
                if ( metaItem.Id == ExifTagGPSLongitude )
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lng = degreesNumerator / degreesDenominator +
                        minutesNumerator / minutesDenominator / 60f +
                        secondsNumerator / secondsDenominator / 3600f;
                }
            }
            return ( lng );
        }
    }

    class GeoRegion
    {
        public double west;
        public double east;
        public double north;
        public double south;

        public GeoRegion( double w, double e, double n, double s )
        {
            west = w;
            east = e;
            north = n;
            south = s;
        }
    }

    class MarsWGS
    {

        string AppPath = System.AppDomain.CurrentDomain.BaseDirectory;


        //  Globals which should be set before calling this function:
        //
        //  int    polyCorners  =  how many corners the polygon has (no repeats)
        //  float  polyX[]      =  horizontal coordinates of corners
        //  float  polyY[]      =  vertical coordinates of corners
        //  float  x, y         =  point to be tested
        //
        //  (Globals are used in this example for purposes of speed.  Change as
        //  desired.)
        //
        //  The function will return YES if the point x,y is inside the polygon, or
        //  NO if it is not.  If the point is exactly on the edge of the polygon,
        //  then the function may return YES or NO.
        //
        //  Note that division by zero is avoided because the division is protected
        //  by the "if" clause which surrounds it.
        private List<double> polyX = new List<double>();
        private List<double> polyY = new List<double>();

        public Boolean pointInPolygon( double x, double y )
        {

            int polyCorners = polyX.Count;
            int i, j = polyCorners - 1;
            bool oddNodes = false;

            for ( i = 0; i < polyCorners; i++ )
            {
                if ( ( polyY[i] < y && polyY[j] >= y )
                 || ( polyY[j] < y && polyY[i] >= y )
                 && ( polyX[i] <= x || polyX[j] <= x ) )
                {
                    if ( polyX[i] + ( y - polyY[i] ) / ( polyY[j] - polyY[i] ) * ( polyX[j] - polyX[i] ) < x )
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            return oddNodes;
        }

        private void LoadPolygon()
        {
            string OffsetFile = string.Format("{0}{1}", AppPath, "china-mainland.poly");
            if ( System.IO.File.Exists( OffsetFile ) )
            {
                using ( StreamReader sr = new StreamReader( OffsetFile ) )
                {
                    string s = sr.ReadToEnd();

                    Match MP = Regex.Match(s, "(\\d\\.\\d{6,6}E\\+\\d{2,2})");

                    int i = 0;
                    while ( MP.Success )
                    {
                        //MessageBox.Show(MP.Value);
                        if ( i % 2 == 0 ) //第一列
                        {
                            polyX.Add( Convert.ToDouble( MP.Value ) );
                        }
                        else //第二列
                        {
                            polyY.Add( Convert.ToDouble( MP.Value ) );
                        }
                        i++;
                        MP = MP.NextMatch();
                    }
                    s = null;
                    sr.Close();
                }
            }
        }

        //China region - raw data
        private List<GeoRegion> chinaRegion = new List<GeoRegion>();
        //China excluded region - raw data
        private  List<GeoRegion> excludeRegion = new List<GeoRegion>();

        private Boolean isInRect( GeoRegion rect, double lon, double lat )
        {
            return rect.west <= lon && rect.east >= lon && rect.north >= lat && rect.south <= lat;
        }

        private Boolean isInChina( double lon, double lat, Boolean simple = false )
        {
            if ( simple )
            {
                for ( int i = 0; i < chinaRegion.Count; i++ )
                {
                    if ( isInRect( chinaRegion[i], lon, lat ) )
                    {
                        for ( int j = 0; j < excludeRegion.Count; j++ )
                        {
                            if ( isInRect( excludeRegion[j], lon, lat ) )
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return pointInPolygon( lon, lat );
            }

        }


        /// 最东端 东经135度2分30秒 黑龙江和乌苏里江交汇处
        /// 最西端 东经73度40分 帕米尔高原乌兹别里山口（乌恰县）
        /// 最南端 北纬3度52分 南沙群岛曾母暗沙
        /// 最北端 北纬53度33分 漠河以北黑龙江主航道（漠河)
        /// <summary>
        /// x是117左右，y是31左右
        /// </summary>
        /// <param name="xMars">中国地图纬度</param>
        /// <param name="yMars">中国地图经度</param>
        /// <param name="xWgs">GPS纬度</param>
        /// <param name="yWgs">GPS经度</param>
        public void Convert2WGS( double xMars, double yMars, out double xWgs, out double yWgs )
        {
            double xtry, ytry, dx, dy;

            xWgs = xMars;
            yWgs = yMars;

            if ( !isInChina( xMars, yMars ) ) return;

            xtry = xMars;
            ytry = yMars;
            Convert2Mars( xMars, yMars, out xtry, out ytry );
            dx = xtry - xMars;
            dy = ytry - yMars;

            xWgs = xMars - dx;
            yWgs = yMars - dy;
            return;
        }

        /// 最东端 东经135度2分30秒 黑龙江和乌苏里江交汇处
        /// 最西端 东经73度40分 帕米尔高原乌兹别里山口（乌恰县）
        /// 最南端 北纬3度52分 南沙群岛曾母暗沙
        /// 最北端 北纬53度33分 漠河以北黑龙江主航道（漠河)
        /// <summary>
        /// x是117左右，y是31左右
        /// </summary>
        /// <param name="xWgs">GPS纬度</param>
        /// <param name="yWgs">GPS经度</param>
        /// <param name="xMars">中国地图纬度</param>
        /// <param name="yMars">中国地图经度</param>
        public void Convert2Mars( double xWgs, double yWgs, out double xMars, out double yMars )
        {
            xMars = xWgs;
            yMars = yWgs;

            const double pi = 3.14159265358979324;

            //
            // Krasovsky 1940
            //
            // a = 6378245.0, 1/f = 298.3
            // b = a * (1 - f)
            // ee = (a^2 - b^2) / a^2;
            const double a = 6378245.0;
            const double ee = 0.00669342162296594323;

            //if ( xWgs < 72.004 || xWgs > 137.8347 )
            //    return;
            //if ( yWgs < 9.9984 || yWgs > 55.8271 )
            //    return;
            if ( !isInChina( xWgs, yWgs ) ) return;

            double x=0, y=0;
            x = xWgs - 105.0;
            y = yWgs - 35.0;

            double dLon =  300.0 + 1.0 * x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt( Math.Abs( x ) );
            dLon += ( 20.0 * Math.Sin( 6.0 * x * pi ) + 20.0 * Math.Sin( 2.0 * x * pi ) ) * 2.0 / 3.0;
            dLon += ( 20.0 * Math.Sin( x * pi ) + 40.0 * Math.Sin( x / 3.0 * pi ) ) * 2.0 / 3.0;
            dLon += ( 150.0 * Math.Sin( x / 12.0 * pi ) + 300.0 * Math.Sin( x / 30.0 * pi ) ) * 2.0 / 3.0;

            double dLat = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt( Math.Abs( x ) );
            dLat += ( 20.0 * Math.Sin( 6.0 * x * pi ) + 20.0 * Math.Sin( 2.0 * x * pi ) ) * 2.0 / 3.0;
            dLat += ( 20.0 * Math.Sin( y * pi ) + 40.0 * Math.Sin( y / 3.0 * pi ) ) * 2.0 / 3.0;
            dLat += ( 160.0 * Math.Sin( y / 12.0 * pi ) + 320.0 * Math.Sin( y * pi / 30.0 ) ) * 2.0 / 3.0;

            double radLat = yWgs / 180.0 * pi;
            double magic = Math.Sin( radLat );
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt( magic );
            dLon = ( dLon * 180.0 ) / ( a / sqrtMagic * Math.Cos( radLat ) * pi );
            dLat = ( dLat * 180.0 ) / ( ( a * ( 1 - ee ) ) / ( magic * sqrtMagic ) * pi );
            xMars = xWgs + dLon;
            yMars = yWgs + dLat;
        }

        public MarsWGS()
        {
            chinaRegion.Add( new GeoRegion( 79.446200, 49.220400, 96.330000, 42.889900 ) );
            chinaRegion.Add( new GeoRegion( 109.687200, 54.141500, 135.000200, 39.374200 ) );
            chinaRegion.Add( new GeoRegion( 73.124600, 42.889900, 124.143255, 29.529700 ) );
            chinaRegion.Add( new GeoRegion( 82.968400, 29.529700, 97.035200, 26.718600 ) );
            chinaRegion.Add( new GeoRegion( 97.025300, 29.529700, 124.367395, 20.414096 ) );
            chinaRegion.Add( new GeoRegion( 107.975793, 20.414096, 111.744104, 17.871542 ) );
            excludeRegion.Add( new GeoRegion( 119.921265, 25.398623, 122.497559, 21.785006 ) );
            excludeRegion.Add( new GeoRegion( 101.865200, 22.284000, 106.665000, 20.098800 ) );
            excludeRegion.Add( new GeoRegion( 106.452500, 21.542200, 108.051000, 20.487800 ) );
            excludeRegion.Add( new GeoRegion( 109.032300, 55.817500, 119.127000, 50.325700 ) );
            excludeRegion.Add( new GeoRegion( 127.456800, 55.817500, 137.022700, 49.557400 ) );
            excludeRegion.Add( new GeoRegion( 131.266200, 44.892200, 137.022700, 42.569200 ) );

            LoadPolygon();
        }
    }

}
