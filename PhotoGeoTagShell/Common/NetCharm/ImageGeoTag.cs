using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCharm
{
    /// <summary>
    /// Usage:
    /// ======
    /// Geotag(new Bitmap(@"C:\path\to\image.jpg"), 34, -118)
    ///        .Save(@"C:\path\to\geotagged.jpg", ImageFormat.Jpeg);
    /// ======
    /// </summary>
    class ImageGeoTag
    {
        /// <summary>
        /// These constants come from the CIPA DC-008 standard for EXIF 2.3
        /// </summary>
        const short ExifTypeByte = 1;
        const short ExifTypeAscii = 2;
        const short ExifTypeRational = 5;


        #region EXIF Data Type
        /// <summary> 
        /// Specifies that the format is 4 bits per pixel, indexed.
        /// </summary>
        public const short PixelFormat4bppIndexed = 0;

        /// <summary> 
        /// Specifies that the value data member is an array of bytes.
        /// </summary>
        public const short PropertyTagTypeByte = 1;

        /// <summary> 
        /// Specifies that the value data member is a null-terminated ASCII string. If you set the type data member of a PropertyItem object to PropertyTagTypeASCII, you should set the length data member to the length of the string including the NULL terminator. For example, the string HELLO would have a length of 6.
        /// </summary>
        public const short PropertyTagTypeASCII = 2;

        /// <summary> 
        /// Specifies that the value data member is an array of unsigned short (16-bit) integers.
        /// </summary>
        public const short PropertyTagTypeShort = 3;

        /// <summary> 
        /// Specifies that the value data member is an array of unsigned long (32-bit) integers.
        /// </summary>
        public const short PropertyTagTypeLong = 4;

        /// <summary> 
        /// Specifies that the value data member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
        /// </summary>
        public const short PropertyTagTypeRational = 5;

        /// <summary> 
        /// Specifies that the value data member is an array of bytes that can hold values of any data type. 
        /// </summary>
        public const short PropertyTagTypeUndefined = 7;

        /// <summary> 
        /// Specifies that the value data member is an array of signed long (32-bit) integers.
        /// </summary>
        public const short PropertyTagTypeSLONG = 9;

        /// <summary> 
        /// Specifies that the value data member is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
        /// </summary>
        public const short PropertyTagTypeSRational = 10;
        #endregion

        #region EXIF Tag ID
        /// <summary>
        /// Exit Property Tag ID Define
        /// </summary>
        public const int PropertyTagGpsVer = 0x0000;
        public const int PropertyTagGpsLatitudeRef = 0x0001;
        public const int PropertyTagGpsLatitude = 0x0002;
        public const int PropertyTagGpsLongitudeRef = 0x0003;
        public const int PropertyTagGpsLongitude = 0x0004;
        public const int PropertyTagGpsAltitudeRef = 0x0005;
        public const int PropertyTagGpsAltitude = 0x0006;
        public const int PropertyTagGpsGpsTime = 0x0007;
        public const int PropertyTagGpsGpsSatellites = 0x0008;
        public const int PropertyTagGpsGpsStatus = 0x0009;
        public const int PropertyTagGpsGpsMeasureMode = 0x000A;
        public const int PropertyTagGpsGpsDop = 0x000B;
        public const int PropertyTagGpsSpeedRef = 0x000C;
        public const int PropertyTagGpsSpeed = 0x000D;
        public const int PropertyTagGpsTrackRef = 0x000E;
        public const int PropertyTagGpsTrack = 0x000F;
        public const int PropertyTagGpsImgDirRef = 0x0010;
        public const int PropertyTagGpsImgDir = 0x0011;
        public const int PropertyTagGpsMapDatum = 0x0012;
        public const int PropertyTagGpsDestLatRef = 0x0013;
        public const int PropertyTagGpsDestLat = 0x0014;
        public const int PropertyTagGpsDestLongRef = 0x0015;
        public const int PropertyTagGpsDestLong = 0x0016;
        public const int PropertyTagGpsDestBearRef = 0x0017;
        public const int PropertyTagGpsDestBear = 0x0018;
        public const int PropertyTagGpsDestDistRef = 0x0019;
        public const int PropertyTagGpsDestDist = 0x001A;
        public const int PropertyTagNewSubfileType = 0x00FE;
        public const int PropertyTagSubfileType = 0x00FF;
        public const int PropertyTagImageWidth = 0x0100;
        public const int PropertyTagImageHeight = 0x0101;
        public const int PropertyTagBitsPerSample = 0x0102;
        public const int PropertyTagCompression = 0x0103;
        public const int PropertyTagPhotometricInterp = 0x0106;
        public const int PropertyTagThreshHolding = 0x0107;
        public const int PropertyTagCellWidth = 0x0108;
        public const int PropertyTagCellHeight = 0x0109;
        public const int PropertyTagFillOrder = 0x010A;
        public const int PropertyTagDocumentName = 0x010D;
        public const int PropertyTagImageDescription = 0x010E;
        public const int PropertyTagEquipMake = 0x010F;
        public const int PropertyTagEquipModel = 0x0110;
        public const int PropertyTagStripOffsets = 0x0111;
        public const int PropertyTagOrientation = 0x0112;
        public const int PropertyTagSamplesPerPixel = 0x0115;
        public const int PropertyTagRowsPerStrip = 0x0116;
        public const int PropertyTagStripBytesCount = 0x0117;
        public const int PropertyTagMinSampleValue = 0x0118;
        public const int PropertyTagMaxSampleValue = 0x0119;
        public const int PropertyTagXResolution = 0x011A;
        public const int PropertyTagYResolution = 0x011B;
        public const int PropertyTagPlanarConfig = 0x011C;
        public const int PropertyTagPageName = 0x011D;
        public const int PropertyTagXPosition = 0x011E;
        public const int PropertyTagYPosition = 0x011F;
        public const int PropertyTagFreeOffset = 0x0120;
        public const int PropertyTagFreeByteCounts = 0x0121;
        public const int PropertyTagGrayResponseUnit = 0x0122;
        public const int PropertyTagGrayResponseCurve = 0x0123;
        public const int PropertyTagT4Option = 0x0124;
        public const int PropertyTagT6Option = 0x0125;
        public const int PropertyTagResolutionUnit = 0x0128;
        public const int PropertyTagPageNumber = 0x0129;
        public const int PropertyTagTransferFunction = 0x012D;
        public const int PropertyTagSoftwareUsed = 0x0131;
        public const int PropertyTagDateTime = 0x0132;
        public const int PropertyTagArtist = 0x013B;
        public const int PropertyTagHostComputer = 0x013C;
        public const int PropertyTagPredictor = 0x013D;
        public const int PropertyTagWhitePoint = 0x013E;
        public const int PropertyTagPrimaryChromaticities = 0x013F;
        public const int PropertyTagColorMap = 0x0140;
        public const int PropertyTagHalftoneHints = 0x0141;
        public const int PropertyTagTileWidth = 0x0142;
        public const int PropertyTagTileLength = 0x0143;
        public const int PropertyTagTileOffset = 0x0144;
        public const int PropertyTagTileByteCounts = 0x0145;
        public const int PropertyTagInkSet = 0x014C;
        public const int PropertyTagInkNames = 0x014D;
        public const int PropertyTagNumberOfInks = 0x014E;
        public const int PropertyTagDotRange = 0x0150;
        public const int PropertyTagTargetPrinter = 0x0151;
        public const int PropertyTagExtraSamples = 0x0152;
        public const int PropertyTagSampleFormat = 0x0153;
        public const int PropertyTagSMinSampleValue = 0x0154;
        public const int PropertyTagSMaxSampleValue = 0x0155;
        public const int PropertyTagTransferRange = 0x0156;
        public const int PropertyTagJPEGProc = 0x0200;
        public const int PropertyTagJPEGInterFormat = 0x0201;
        public const int PropertyTagJPEGInterLength = 0x0202;
        public const int PropertyTagJPEGRestartInterval = 0x0203;
        public const int PropertyTagJPEGLosslessPredictors = 0x0205;
        public const int PropertyTagJPEGPointTransforms = 0x0206;
        public const int PropertyTagJPEGQTables = 0x0207;
        public const int PropertyTagJPEGDCTables = 0x0208;
        public const int PropertyTagJPEGACTables = 0x0209;
        public const int PropertyTagYCbCrCoefficients = 0x0211;
        public const int PropertyTagYCbCrSubsampling = 0x0212;
        public const int PropertyTagYCbCrPositioning = 0x0213;
        public const int PropertyTagREFBlackWhite = 0x0214;
        public const int PropertyTagGamma = 0x0301;
        public const int PropertyTagICCProfileDescriptor = 0x0302;
        public const int PropertyTagSRGBRenderingIntent = 0x0303;
        public const int PropertyTagImageTitle = 0x0320;
        public const int PropertyTagResolutionXUnit = 0x5001;
        public const int PropertyTagResolutionYUnit = 0x5002;
        public const int PropertyTagResolutionXLengthUnit = 0x5003;
        public const int PropertyTagResolutionYLengthUnit = 0x5004;
        public const int PropertyTagPrintFlags = 0x5005;
        public const int PropertyTagPrintFlagsVersion = 0x5006;
        public const int PropertyTagPrintFlagsCrop = 0x5007;
        public const int PropertyTagPrintFlagsBleedWidth = 0x5008;
        public const int PropertyTagPrintFlagsBleedWidthScale = 0x5009;
        public const int PropertyTagHalftoneLPI = 0x500A;
        public const int PropertyTagHalftoneLPIUnit = 0x500B;
        public const int PropertyTagHalftoneDegree = 0x500C;
        public const int PropertyTagHalftoneShape = 0x500D;
        public const int PropertyTagHalftoneMisc = 0x500E;
        public const int PropertyTagHalftoneScreen = 0x500F;
        public const int PropertyTagJPEGQuality = 0x5010;
        public const int PropertyTagGridSize = 0x5011;
        public const int PropertyTagThumbnailFormat = 0x5012;
        public const int PropertyTagThumbnailWidth = 0x5013;
        public const int PropertyTagThumbnailHeight = 0x5014;
        public const int PropertyTagThumbnailColorDepth = 0x5015;
        public const int PropertyTagThumbnailPlanes = 0x5016;
        public const int PropertyTagThumbnailRawBytes = 0x5017;
        public const int PropertyTagThumbnailSize = 0x5018;
        public const int PropertyTagThumbnailCompressedSize = 0x5019;
        public const int PropertyTagColorTransferFunction = 0x501A;
        public const int PropertyTagThumbnailData = 0x501B;
        public const int PropertyTagThumbnailImageWidth = 0x5020;
        public const int PropertyTagThumbnailImageHeight = 0x5021;
        public const int PropertyTagThumbnailBitsPerSample = 0x5022;
        public const int PropertyTagThumbnailCompression = 0x5023;
        public const int PropertyTagThumbnailPhotometricInterp = 0x5024;
        public const int PropertyTagThumbnailImageDescription = 0x5025;
        public const int PropertyTagThumbnailEquipMake = 0x5026;
        public const int PropertyTagThumbnailEquipModel = 0x5027;
        public const int PropertyTagThumbnailStripOffsets = 0x5028;
        public const int PropertyTagThumbnailOrientation = 0x5029;
        public const int PropertyTagThumbnailSamplesPerPixel = 0x502A;
        public const int PropertyTagThumbnailRowsPerStrip = 0x502B;
        public const int PropertyTagThumbnailStripBytesCount = 0x502C;
        public const int PropertyTagThumbnailResolutionX = 0x502D;
        public const int PropertyTagThumbnailResolutionY = 0x502E;
        public const int PropertyTagThumbnailPlanarConfig = 0x502F;
        public const int PropertyTagThumbnailResolutionUnit = 0x5030;
        public const int PropertyTagThumbnailTransferFunction = 0x5031;
        public const int PropertyTagThumbnailSoftwareUsed = 0x5032;
        public const int PropertyTagThumbnailDateTime = 0x5033;
        public const int PropertyTagThumbnailArtist = 0x5034;
        public const int PropertyTagThumbnailWhitePoint = 0x5035;
        public const int PropertyTagThumbnailPrimaryChromaticities = 0x5036;
        public const int PropertyTagThumbnailYCbCrCoefficients = 0x5037;
        public const int PropertyTagThumbnailYCbCrSubsampling = 0x5038;
        public const int PropertyTagThumbnailYCbCrPositioning = 0x5039;
        public const int PropertyTagThumbnailRefBlackWhite = 0x503A;
        public const int PropertyTagThumbnailCopyRight = 0x503B;
        public const int PropertyTagLuminanceTable = 0x5090;
        public const int PropertyTagChrominanceTable = 0x5091;
        public const int PropertyTagFrameDelay = 0x5100;
        public const int PropertyTagLoopCount = 0x5101;
        public const int PropertyTagGlobalPalette = 0x5102;
        public const int PropertyTagIndexBackground = 0x5103;
        public const int PropertyTagIndexTransparent = 0x5104;
        public const int PropertyTagPixelUnit = 0x5110;
        public const int PropertyTagPixelPerUnitX = 0x5111;
        public const int PropertyTagPixelPerUnitY = 0x5112;
        public const int PropertyTagPaletteHistogram = 0x5113;
        public const int PropertyTagCopyright = 0x8298;
        public const int PropertyTagExifExposureTime = 0x829A;
        public const int PropertyTagExifFNumber = 0x829D;
        public const int PropertyTagExifIFD = 0x8769;
        public const int PropertyTagICCProfile = 0x8773;
        public const int PropertyTagExifExposureProg = 0x8822;
        public const int PropertyTagExifSpectralSense = 0x8824;
        public const int PropertyTagGpsIFD = 0x8825;
        public const int PropertyTagExifISOSpeed = 0x8827;
        public const int PropertyTagExifOECF = 0x8828;
        public const int PropertyTagExifVer = 0x9000;
        public const int PropertyTagExifDTOrig = 0x9003;
        public const int PropertyTagExifDTDigitized = 0x9004;
        public const int PropertyTagExifCompConfig = 0x9101;
        public const int PropertyTagExifCompBPP = 0x9102;
        public const int PropertyTagExifShutterSpeed = 0x9201;
        public const int PropertyTagExifAperture = 0x9202;
        public const int PropertyTagExifBrightness = 0x9203;
        public const int PropertyTagExifExposureBias = 0x9204;
        public const int PropertyTagExifMaxAperture = 0x9205;
        public const int PropertyTagExifSubjectDist = 0x9206;
        public const int PropertyTagExifMeteringMode = 0x9207;
        public const int PropertyTagExifLightSource = 0x9208;
        public const int PropertyTagExifFlash = 0x9209;
        public const int PropertyTagExifFocalLength = 0x920A;
        public const int PropertyTagExifMakerNote = 0x927C;
        public const int PropertyTagExifUserComment = 0x9286;
        public const int PropertyTagExifDTSubsec = 0x9290;
        public const int PropertyTagExifDTOrigSS = 0x9291;
        public const int PropertyTagExifDTDigSS = 0x9292;
        public const int PropertyTagExifFPXVer = 0xA000;
        public const int PropertyTagExifColorSpace = 0xA001;
        public const int PropertyTagExifPixXDim = 0xA002;
        public const int PropertyTagExifPixYDim = 0xA003;
        public const int PropertyTagExifRelatedWav = 0xA004;
        public const int PropertyTagExifInterop = 0xA005;
        public const int PropertyTagExifFlashEnergy = 0xA20B;
        public const int PropertyTagExifSpatialFR = 0xA20C;
        public const int PropertyTagExifFocalXRes = 0xA20E;
        public const int PropertyTagExifFocalYRes = 0xA20F;
        public const int PropertyTagExifFocalResUnit = 0xA210;
        public const int PropertyTagExifSubjectLoc = 0xA214;
        public const int PropertyTagExifExposureIndex = 0xA215;
        public const int PropertyTagExifSensingMethod = 0xA217;
        public const int PropertyTagExifFileSource = 0xA300;
        public const int PropertyTagExifSceneType = 0xA301;
        public const int PropertyTagExifCfaPattern = 0xA302;
        #endregion

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="file"></param>
        /// <param name="touch"></param>
        public static void TouchPhoto( Image photo, string file, string touch = "" )
        {
            FileInfo fi = new FileInfo( file );
            DateTime dt = DateTime.Now;

            if ( string.IsNullOrEmpty( touch ))
            {
                try
                {
                    //PropertyItem DTDigital = photo.GetPropertyItem(PropertyTagExifDTDigitized);
                    PropertyItem DTOrig = photo.GetPropertyItem(PropertyTagExifDTOrig);

                    ASCIIEncoding enc = new ASCIIEncoding();
                    string dateTakenText = enc.GetString( DTOrig.Value, 0, DTOrig.Len - 1 );

                    if ( !string.IsNullOrEmpty( dateTakenText ) )
                    {
                        if ( !DateTime.TryParseExact( dateTakenText, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt ) )
                        {
                            //dt = DateTime.ParseExact( dateTakenText, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None );
                            return;
                        }
                    }
                    else return;
                }
                catch { }
            }
            else
            {
                dt = DateTime.Parse( touch );
            }
            fi.LastAccessTimeUtc = dt.ToUniversalTime();
            fi.LastWriteTimeUtc = dt.ToUniversalTime();
            fi.CreationTimeUtc = dt.ToUniversalTime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="touch"></param>
        public static void TouchPhoto(string photo, string touch="" )
        {
            using ( FileStream fs = new FileStream( photo, FileMode.Open, FileAccess.Read ) )
            {
                Image img = Image.FromStream( fs, true, true );
                fs.Close();
                TouchPhoto( img, photo, touch );
                img.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
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
            AddProperty( img, PropertyTagGpsVer, ExifTypeByte, new byte[] { 2, 3, 0, 0 } );
            AddProperty( img, PropertyTagGpsLatitudeRef, ExifTypeAscii, new byte[] { (byte) latHemisphere, 0 } );
            AddProperty( img, PropertyTagGpsLatitude, ExifTypeRational, ConvertToRationalTriplet( lat ) );
            AddProperty( img, PropertyTagGpsLongitudeRef, ExifTypeAscii, new byte[] { (byte) lngHemisphere, 0 } );
            AddProperty( img, PropertyTagGpsLongitude, ExifTypeRational, ConvertToRationalTriplet( lng ) );

            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static byte[] ConvertToRationalTriplet( double value )
        {
            int factor = 1000000;
            int degrees = (int)Math.Floor(value);
            value = ( value - degrees ) * 60;
            int minutes = (int)Math.Floor(value);
            value = ( value - minutes ) * 60 * factor;
            int seconds = (int)Math.Round(value);
            byte[] bytes = new byte[3 * 2 * 4]; // Degrees, minutes, and seconds, each with a numerator and a denominator, each composed of 4 bytes
            int i = 0;
            Array.Copy( BitConverter.GetBytes( degrees ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( 1 ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( minutes ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( 1 ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( seconds ), 0, bytes, i, 4 ); i += 4;
            Array.Copy( BitConverter.GetBytes( factor ), 0, bytes, i, 4 );
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        static void AddProperty( Image img, int id, short type, byte[] value )
        {
            PropertyItem pi = img.PropertyItems[0];
            pi.Id = id;
            pi.Type = type;
            pi.Len = value.Length;
            pi.Value = value;
            img.SetPropertyItem( pi );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Latitude"></param>
        /// <returns></returns>
        public static double GetLatitude(Image img, double Latitude=0.0)
        {
            double lat = Double.NaN;

            foreach ( PropertyItem metaItem in img.PropertyItems )
            {
                if ( metaItem.Id == PropertyTagGpsLatitude )
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public static double GetLongitude( Image img, double Longitude=0.0)
        {
            double lng = double.NaN;

            foreach ( PropertyItem metaItem in img.PropertyItems )
            {
                if ( metaItem.Id == PropertyTagGpsLongitude )
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="e"></param>
        /// <param name="n"></param>
        /// <param name="s"></param>
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

        /// <summary>
        /// 
        /// </summary>
        string AppPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///  Globals which should be set before calling this function:
        ///
        /// int    polyCorners  =  how many corners the polygon has (no repeats)
        /// float  polyX[]      =  horizontal coordinates of corners
        /// float  polyY[]      =  vertical coordinates of corners
        /// float  x, y         =  point to be tested
        ///
        /// (Globals are used in this example for purposes of speed.  Change as
        /// desired.)
        ///
        /// The function will return YES if the point x,y is inside the polygon, or
        /// NO if it is not.  If the point is exactly on the edge of the polygon,
        /// then the function may return YES or NO.
        ///
        /// Note that division by zero is avoided because the division is protected
        /// by the "if" clause which surrounds it.
        /// </summary>
        private List<double> polyX = new List<double>();
        private List<double> polyY = new List<double>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// China region - raw data
        /// </summary>
        private List<GeoRegion> chinaRegion = new List<GeoRegion>();

        /// <summary>
        /// China excluded region - raw data
        /// </summary>
        private  List<GeoRegion> excludeRegion = new List<GeoRegion>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        private Boolean isInRect( GeoRegion rect, double lon, double lat )
        {
            return rect.west <= lon && rect.east >= lon && rect.north >= lat && rect.south <= lat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="simple"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
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
