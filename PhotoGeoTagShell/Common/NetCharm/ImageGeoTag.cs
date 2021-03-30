using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Fotofly;
using Fotofly.BitmapMetadataTools;

namespace NetCharm
{
    /// <summary>
    /// Usage:
    /// ======
    /// Geotag(new Bitmap(@"C:\path\to\image.jpg"), 34, -118)
    ///        .Save(@"C:\path\to\geotagged.jpg", ImageFormat.Jpeg);
    /// ======
    /// </summary>

    [Serializable]
    [ComVisible(true)]
    public struct PointD
    {
        public double X;
        public double Y;
    }

    class EXIF
    {
        /// <summary>
        /// These constants come from the CIPA DC-008 standard for EXIF 2.3
        /// </summary>

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

        // extension tags
        public const int PropertyTagExifCustomRendered = 0xA401;
        public const int PropertyTagExifExposureMode = 0xA402;
        public const int PropertyTagExifWhiteBalance = 0xA403;
        public const int PropertyTagExifDigitalZoomRatio = 0xA404;
        public const int PropertyTagExifFocalLengthIn35mmFilm = 0xA405;
        public const int PropertyTagExifSceneCaptureType = 0xA406;
        public const int PropertyTagExifGainControl = 0xA407;
        public const int PropertyTagExifContrast = 0xA408;
        public const int PropertyTagExifSaturation = 0xA409;
        public const int PropertyTagExifSharpness = 0xA40A;
        public const int PropertyTagExifSubjectDistanceRange = 0xA40C;

        // XP System Property
        public const int PropertyTagExifXPTitle = 0x9C9B; // 40091;
        public const int PropertyTagExifXPComment = 0x9C9C; // 40092;
        public const int PropertyTagExifXPAuthor = 0x9C9D; // 40093;
        public const int PropertyTagExifXPKeywords = 0x9C9E; // 40094;
        public const int PropertyTagExifXPSubject = 0x9C9F; // 40095;

        #endregion

        private static bool touching = false;
        public static bool IsTouching
        {
            get { return touching; }
            set { touching = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string[] PhotoExts = { ".jpg", ".jpeg", ".tif", ".tiff", ".png" };

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="file"></param>
        /// <param name="touch"></param>
        public static void TouchPhoto(Image photo, string file, string touch = "")
        {
            FileInfo fi = new FileInfo( file );
            DateTime dt = fi.CreationTimeUtc.ToLocalTime();

            if (string.IsNullOrEmpty(touch))
            {
                try
                {
                    if (photo.PropertyIdList.Contains(EXIF.PropertyTagExifDTOrig) ||
                        photo.PropertyIdList.Contains(EXIF.PropertyTagExifDTDigitized) ||
                        photo.PropertyIdList.Contains(EXIF.PropertyTagDateTime))
                    {
                        PropertyItem dtOrigProp = photo.PropertyIdList.Contains( EXIF.PropertyTagExifDTOrig ) ? photo.GetPropertyItem( EXIF.PropertyTagExifDTOrig ) : null;
                        PropertyItem dtDigiProp = photo.PropertyIdList.Contains( EXIF.PropertyTagExifDTDigitized ) ? photo.GetPropertyItem( EXIF.PropertyTagExifDTDigitized ) : null;
                        PropertyItem dtExifProp = photo.PropertyIdList.Contains( EXIF.PropertyTagDateTime ) ? photo.GetPropertyItem( EXIF.PropertyTagDateTime ) : null;

                        PropertyItem DT = null;
                        if (dtOrigProp != null) DT = dtOrigProp;
                        else if (dtDigiProp != null) DT = dtDigiProp;
                        else if (dtExifProp != null) DT = dtExifProp;
                        else DT = null;

                        if (DT != null)
                        {
                            ASCIIEncoding encode = new ASCIIEncoding();
                            string dateText = encode.GetString( DT.Value, 0, DT.Len - 1 );

                            if (!string.IsNullOrEmpty(dateText))
                            {
                                if (!DateTime.TryParseExact(dateText, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                                {
                                    return;
                                }
                            }
                            else return;
                        }
                    }
                    if (photo.PropertyIdList.Contains(EXIF.PropertyTagImageTitle) ||
                        photo.PropertyIdList.Contains(EXIF.PropertyTagExifXPTitle))
                    {
                        PropertyItem title = photo.PropertyIdList.Contains( EXIF.PropertyTagImageTitle ) ? photo.GetPropertyItem( EXIF.PropertyTagImageTitle ) : null;
                        PropertyItem title_xp = photo.PropertyIdList.Contains( EXIF.PropertyTagExifXPTitle ) ? photo.GetPropertyItem( EXIF.PropertyTagExifXPTitle ) : null;

                        if (title != null) { var t_title = Encoding.UTF8.GetString(title.Value); }
                        if (title_xp != null) { var t_title_xp = Encoding.UTF32.GetString(title_xp.Value); }

                    }
                    else
                    {
                        AddProperty(photo, EXIF.PropertyTagImageTitle, 6, Encoding.UTF32.GetBytes(Path.GetFileName(file)));
                        AddProperty(photo, EXIF.PropertyTagExifXPTitle, 6, Encoding.UTF32.GetBytes(Path.GetFileName(file)));
                    }
                }
                catch { }
            }
            else
            {
                if (!DateTime.TryParse(touch, out dt)) return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="touch"></param>
        public static void TouchPhoto(string photo, string touch = "")
        {
            try
            {
                FileInfo fi = new FileInfo( photo );
                DateTime dt = fi.CreationTimeUtc.ToLocalTime();
                bool is_png = fi.Extension.Equals(".png", StringComparison.CurrentCultureIgnoreCase);
                string name = Path.GetFileNameWithoutExtension(fi.Name);
                if (Microsoft.WindowsAPICodePack.Shell.ShellObject.IsPlatformSupported)
                {
                    var sh = Microsoft.WindowsAPICodePack.Shell.ShellFile.FromFilePath(photo);
                    if (sh.Properties.System.Photo.DateTaken.Value == null) sh.Properties.System.Photo.DateTaken.Value = dt;
                    if (!is_png)
                    {
                        if (sh.Properties.System.Title.Value == null) sh.Properties.System.Title.Value = name;
                        if (sh.Properties.System.Subject.Value == null) sh.Properties.System.Subject.Value = name;
                        if (sh.Properties.System.DateAcquired.Value == null) sh.Properties.System.DateAcquired.Value = dt;
                        if (sh.Properties.System.FileDescription.Value == null) sh.Properties.System.FileDescription.Value = name;
                        SetImageTitle_WPF(photo, Path.GetFileNameWithoutExtension(fi.Name), dt);
                    }
                }
                fi.LastAccessTimeUtc = dt.ToUniversalTime();
                fi.LastWriteTimeUtc = dt.ToUniversalTime();
                fi.CreationTimeUtc = dt.ToUniversalTime();
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="touch"></param>
        /// <param name="option"></param>
        public static void TouchPhoto(string folder, string touch = "", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            IEnumerable<FileInfo> fileinfos  = di.EnumerateFiles().Where( f => PhotoExts.Contains( f.Extension, StringComparer.CurrentCultureIgnoreCase ) );
            foreach (FileInfo file in fileinfos)
            {
                TouchPhoto($"{file.DirectoryName}{Path.DirectorySeparatorChar}{file.Name}", touch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static Image Geotag(Image original, double lat, double lng)
        {
            char latHemisphere = 'N';
            if (lat < 0)
            {
                latHemisphere = 'S';
                lat = -lat;
            }
            char lngHemisphere = 'E';
            if (lng < 0)
            {
                lngHemisphere = 'W';
                lng = -lng;
            }

            MemoryStream ms = new MemoryStream();
            original.Save(ms, ImageFormat.Jpeg);
            ms.Seek(0, SeekOrigin.Begin);

            Image img = Image.FromStream(ms);
            AddProperty(img, PropertyTagGpsVer, PropertyTagTypeByte, new byte[] { 2, 3, 0, 0 });
            AddProperty(img, PropertyTagGpsLatitudeRef, PropertyTagTypeASCII, new byte[] { (byte)latHemisphere, 0 });
            AddProperty(img, PropertyTagGpsLatitude, PropertyTagTypeRational, ConvertToRationalTriplet(lat));
            AddProperty(img, PropertyTagGpsLongitudeRef, PropertyTagTypeASCII, new byte[] { (byte)lngHemisphere, 0 });
            AddProperty(img, PropertyTagGpsLongitude, PropertyTagTypeRational, ConvertToRationalTriplet(lng));

            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static byte[] ConvertToRationalTriplet(double value)
        {
            int factor = 10000000;
            int degrees = (int)Math.Floor(value);
            value = (value - degrees) * 60;
            int minutes = (int)Math.Floor(value);
            value = (value - minutes) * 60 * factor;
            int seconds = (int)Math.Round(value);
            byte[] bytes = new byte[3 * 2 * 4]; // Degrees, minutes, and seconds, each with a numerator and a denominator, each composed of 4 bytes
            int i = 0;
            Array.Copy(BitConverter.GetBytes(degrees), 0, bytes, i, 4); i += 4;
            Array.Copy(BitConverter.GetBytes(1), 0, bytes, i, 4); i += 4;
            Array.Copy(BitConverter.GetBytes(minutes), 0, bytes, i, 4); i += 4;
            Array.Copy(BitConverter.GetBytes(1), 0, bytes, i, 4); i += 4;
            Array.Copy(BitConverter.GetBytes(seconds), 0, bytes, i, 4); i += 4;
            Array.Copy(BitConverter.GetBytes(factor), 0, bytes, i, 4);
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        internal static void AddProperty(Image img, int id, short type, byte[] value)
        {
            PropertyItem pi = img.PropertyItems[0];
            pi.Id = id;
            pi.Type = type;
            pi.Len = value.Length;
            pi.Value = value;
            img.SetPropertyItem(pi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Latitude"></param>
        /// <returns></returns>
        public static double GetLatitude(Image img, double Latitude = double.NaN)
        {
            double lat = Latitude;

            foreach (PropertyItem metaItem in img.PropertyItems)
            {
                if (metaItem.Id == PropertyTagGpsLatitude)
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lat = (double)degreesNumerator / (double)degreesDenominator +
                        (double)minutesNumerator / (double)minutesDenominator / 60f +
                        (double)secondsNumerator / (double)secondsDenominator / 3600f;
                }
            }
            return (lat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public static double GetLongitude(Image img, double Longitude = double.NaN)
        {
            double lng = Longitude;

            foreach (PropertyItem metaItem in img.PropertyItems)
            {
                if (metaItem.Id == PropertyTagGpsLongitude)
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lng = (double)degreesNumerator / (double)degreesDenominator +
                       (double)minutesNumerator / (double)minutesDenominator / 60f +
                       (double)secondsNumerator / (double)secondsDenominator / 3600f;
                }
            }
            return (lng);
        }

        /// <summary>
        /// GetLatitudeRef
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Ref"></param>
        /// <returns></returns>
        public static char GetLatitudeRef(Image img, char Ref = 'N')
        {
            char latRef = Ref;

            foreach (PropertyItem metaItem in img.PropertyItems)
            {
                if (metaItem.Id == PropertyTagGpsLatitudeRef)
                {
                    latRef = BitConverter.ToChar(metaItem.Value, 0);
                }
            }

            return (latRef);
        }

        /// <summary>
        /// GetLongitudeRef
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Ref"></param>
        /// <returns></returns>
        public static char GetLongitudeRef(Image img, char Ref = 'E')
        {
            char lngRef = Ref;

            foreach (PropertyItem metaItem in img.PropertyItems)
            {
                if (metaItem.Id == PropertyTagGpsLongitudeRef)
                {
                    lngRef = BitConverter.ToChar(metaItem.Value, 0);
                }
            }

            return (lngRef);
        }

        /// <summary>
        /// Get GeoTag Lat&Lng
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public static PointF GetLatLng(Image img, double Latitude = double.NaN, double Longitude = double.NaN)
        {
            double lat = Latitude;
            double lng = Longitude;

            foreach (PropertyItem metaItem in img.PropertyItems)
            {
                if (metaItem.Id == PropertyTagGpsLatitude)
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lat = (double)degreesNumerator / (double)degreesDenominator +
                        (double)minutesNumerator / (double)minutesDenominator / 60f +
                        (double)secondsNumerator / (double)secondsDenominator / 3600f;
                }
                else if (metaItem.Id == PropertyTagGpsLongitude)
                {
                    uint degreesNumerator   = BitConverter.ToUInt32(metaItem.Value, 0);
                    uint degreesDenominator = BitConverter.ToUInt32(metaItem.Value, 4);
                    uint minutesNumerator   = BitConverter.ToUInt32(metaItem.Value, 8);
                    uint minutesDenominator = BitConverter.ToUInt32(metaItem.Value, 12);
                    uint secondsNumerator   = BitConverter.ToUInt32(metaItem.Value, 16);
                    uint secondsDenominator = BitConverter.ToUInt32(metaItem.Value, 20);

                    lng = (double)degreesNumerator / (double)degreesDenominator +
                       (double)minutesNumerator / (double)minutesDenominator / 60f +
                        (double)secondsNumerator / (double)secondsDenominator / 3600f;
                }
            }
            return (new PointF((float)lat, (float)lng));
        }

        /// <summary>
        /// GetEncoder
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="image"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime SetImageGeoTag_WPF(double lat, double lng, string image, DateTime dt)
        {
            #region Using Fotofly library ( WIC wrapper )
            using (WpfFileManager wpfFileManager = new WpfFileManager(image, true))
            {
                var metadata = wpfFileManager.BitmapMetadata;
                if (metadata != null)
                {
                    HashSet<string> keywords = new HashSet<string>();
                    HashSet<string> authors = new HashSet<string>();
                    HashSet<string> titles = new HashSet<string>();
                    HashSet<string> copyrights = new HashSet<string>();
                    HashSet<string> comments = new HashSet<string>();

                    #region Get DateTaken
                    string dtmeta = String.Empty;
                    var dtexif = metadata.GetQuery(META.TagExifDateTime);
                    if (metadata.DateTaken != null)
                    {
                        dtmeta = metadata.DateTaken;
                        if (!string.IsNullOrEmpty(dtmeta) && !DateTime.TryParse(dtmeta, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                        {
                        }
                    }
                    else if (dtexif != null)
                    {
                        dtmeta = dtexif as string;
                        if (!string.IsNullOrEmpty(dtmeta) && !DateTime.TryParseExact(dtmeta, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                        {
                        }
                    }
                    #endregion

                    #region Get Keywords
                    if (metadata.Keywords != null)
                    {
                        foreach (string keyword in metadata.Keywords)
                        {
                            keywords.Add(keyword.Trim());
                        }
                    }
                    var iptckeywords = metadata.GetQuery( META.TagIptcKeywords );
                    if (iptckeywords != null)
                    {
                        if (iptckeywords as string[] == null)
                        {
                            keywords.Add((iptckeywords as string).Trim());
                        }
                        else
                        {
                            foreach (string keyword in (iptckeywords as string[]))
                            {
                                keywords.Add(keyword.Trim());
                            }
                        }
                    }
                    BitmapMetadata xmpsubjects = metadata.GetQuery( META.TagXmpSubject ) as BitmapMetadata;
                    if (xmpsubjects != null)
                    {
                        foreach (string query in xmpsubjects.ToList())
                        {
                            string keyword = xmpsubjects.GetQuery( query ) as string;
                            keywords.Add(keyword.Trim());
                        }
                    }
                    var xpkeywords = metadata.GetQuery( META.TagExifXPKeywords );
                    if (xpkeywords != null)
                    {
                        string xpkeywords_str = Encoding.Unicode.GetString( (byte[]) xpkeywords ).Trim( new char[] { ' ', '\0' } );
                        foreach (string key in xpkeywords_str.Split(';'))
                        {
                            keywords.Add(key.Trim());
                        }
                    }
                    #endregion

                    #region Get Authors
                    var artist = metadata.GetQuery( META.TagExifArtist );
                    if (artist != null)
                    {
                        foreach (string art in (artist as string).Split(';'))
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    if (wpfFileManager.BitmapMetadata.Author != null)
                    {
                        foreach (string art in wpfFileManager.BitmapMetadata.Author)
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    var xpauthor = metadata.GetQuery( META.TagExifXPAuthor );
                    if (xpauthor != null)
                    {
                        string xpauthor_str = Encoding.Unicode.GetString( (byte[]) xpauthor ).Trim( new char[] { ' ', '\0' } );
                        //metadata.SetQuery( META.TagIptcByline, xpauthor_str );
                        foreach (string art in xpauthor_str.Split(';'))
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    #endregion

                    #region Get Title

                    #endregion

                    #region Get Copyright

                    #endregion

                    #region Get Comments

                    #endregion

                    if (metadata.IsFrozen)
                    {
                        //metadata = metadata.Clone();
                    }

                    #region Set GPS Info
                    char latHemisphere = 'N';
                    if (lat < 0)
                    {
                        latHemisphere = 'S';
                        lat = -lat;
                    }
                    char lngHemisphere = 'E';
                    if (lng < 0)
                    {
                        lngHemisphere = 'W';
                        lng = -lng;
                    }
                    GpsCoordinate glat = new GpsCoordinate(GpsCoordinate.LatOrLons.Latitude, lat);
                    GpsCoordinate glng = new GpsCoordinate(GpsCoordinate.LatOrLons.Longitude, lng);

                    //ulong factor = 10000000;
                    ulong[] ulat = new ulong[3] {
                        Convert.ToUInt64( glat.Degrees ) + 0x0000000100000000L,
                        Convert.ToUInt64( glat.Minutes ) + 0x0000000100000000L,
                        Convert.ToUInt64( glat.Seconds ) + 0x0000000100000000L
                        //Convert.ToUInt64( (( glat.Numeric - glat.Degrees ) * 60 - glat.Minutes ) * 60 * factor ) + 0x0098968000000000L
                    };
                    ulong[] ulng = new ulong[3] {
                        Convert.ToUInt64( glng.Degrees ) + 0x0000000100000000L,
                        Convert.ToUInt64( glng.Minutes ) + 0x0000000100000000L,
                        Convert.ToUInt64( glng.Seconds ) + 0x0000000100000000L
                        //Convert.ToUInt64( (( glng.Numeric - glng.Degrees ) * 60 - glng.Minutes ) * 60 * factor ) + 0x0098968000000000L
                    };

                    metadata.SetQuery(META.TagExifGpsLatitudeRef, latHemisphere);
                    metadata.SetQuery(META.TagExifGpsLatitude, ulat);
                    metadata.SetQuery(META.TagExifGpsLongitudeRef, lngHemisphere);
                    metadata.SetQuery(META.TagExifGpsLongitude, ulng);
                    #endregion

                    #region Set Title & Subject & Comment
                    var xptitle = metadata.GetQuery( META.TagExifXPTitle );
                    if (xptitle != null)
                    {
                        string xptitle_str = Encoding.Unicode.GetString( (byte[]) xptitle ).Trim(new char[] { ' ', '\0' } );
                        metadata.SetQuery(META.TagIptcBylineTitle, xptitle_str);
                    }
                    var xpcomment = metadata.GetQuery( META.TagExifXPComment );
                    if (xpcomment != null)
                    {
                        string xpcomment_str = Encoding.Unicode.GetString( (byte[]) xpcomment ).Trim( new char[] { ' ', '\0' } );
                        metadata.SetQuery(META.TagIptcCaption, xpcomment_str);
                    }
                    var xpsubject = metadata.GetQuery( META.TagExifXPSubject );
                    if (xpsubject != null)
                    {
                        string xpsubject_str = Encoding.Unicode.GetString( (byte[]) xpsubject ).Trim( new char[] { ' ', '\0' } );
                        metadata.SetQuery(META.TagIptcHeadline, xpsubject_str);
                    }
                    var xpcopyright = metadata.GetQuery( META.TagExifCopyright );
                    if (xpcopyright != null && !string.IsNullOrEmpty((xpcopyright as string).Trim()))
                    {
                        string xpcopyright_str = (xpcopyright as string).Trim();
                        metadata.SetQuery(META.TagIptcCopyrightNotice, xpcopyright_str);
                    }
                    #endregion

                    #region Set Keywords & Authors
                    ulong idx = 0;
                    foreach (string keyword in keywords)
                    {
                        string query = $"{META.TagXmpSubject}/{{ulong={idx}}}";
                        metadata.SetQuery(query, keyword);
                        idx++;
                    }
                    if (keywords.Count > 0)
                    {
                        metadata.SetQuery(META.TagIptcKeywords, keywords.ToArray());
                    }

                    if (authors.Count > 0)
                    {
                        metadata.SetQuery(META.TagIptcByline, string.Join(";", authors));
                    }
                    #endregion

                    #region Set Image.Datetime to Taken datetime
                    if (!string.IsNullOrEmpty(metadata.DateTaken))
                    {
                        //metadata.SetQuery( META.TagExifDateTime, metadata.DateTaken );
                        metadata.SetQuery(META.TagExifDateTime, metadata.DateTaken.Replace('/', ':').Replace('-', ':').Replace(',', ':').Replace('.', ':'));
                    }
                    #endregion
                    wpfFileManager.WriteMetadata();
                }
            }
            #endregion

            return (dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="image"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime SetImageGeoTag_GDI(double lat, double lng, string image, DateTime dt)
        {
            using (FileStream fs = new FileStream(image, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {

                fs.Seek(0, SeekOrigin.Begin);
                Image photo = Image.FromStream( fs, true, true );
                photo = EXIF.Geotag(photo, lat, lng);

                fs.Close();

                try
                {
                    if (photo.PropertyIdList.Contains(EXIF.PropertyTagExifDTOrig))
                    {
                        PropertyItem DTOrig = photo.GetPropertyItem(EXIF.PropertyTagExifDTOrig);

                        ASCIIEncoding enc = new ASCIIEncoding();
                        string dateTakenText = enc.GetString( DTOrig.Value, 0, DTOrig.Len - 1 );

                        if (!string.IsNullOrEmpty(dateTakenText))
                        {
                            if (!DateTime.TryParseExact(dateTakenText, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                            {
                            }
                        }
                    }
                }
                catch { }

                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                photo.Save(image, jpgEncoder, myEncoderParameters);
                photo.Dispose();
            }
            return (dt);
        }

        public static void SetImageTitle_WPF(string image, string title, DateTime dt, string subject = "", string copyright = "", string author = "", string comment = "", string keyword_list = "")
        {
            #region Using Fotofly library ( WIC wrapper )
            using (WpfFileManager wpfFileManager = new WpfFileManager(image, true))
            {
                var metadata = wpfFileManager.BitmapMetadata;
                if (metadata != null)
                {
                    HashSet<string> keywords = new HashSet<string>();
                    HashSet<string> authors = new HashSet<string>();
                    HashSet<string> titles = new HashSet<string>();
                    HashSet<string> copyrights = new HashSet<string>();
                    HashSet<string> comments = new HashSet<string>();

                    #region Get DateTaken
                    string dtmeta = string.Empty;
                    var dtexif = metadata.GetQuery(META.TagExifDateTime);
                    if (metadata.DateTaken != null)
                    {
                        dtmeta = metadata.DateTaken;
                        if (!string.IsNullOrEmpty(dtmeta) && !DateTime.TryParse(dtmeta, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                        {
                        }
                    }
                    else if (dtexif != null)
                    {
                        dtmeta = dtexif as string;
                        if (!string.IsNullOrEmpty(dtmeta) && !DateTime.TryParseExact(dtmeta, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                        {
                        }
                    }
                    //var meta_date = Encoding.Unicode.GetBytes(dt.ToString("yyyy:MM:dd HH:mm:ss"));
                    var meta_date = dt.ToString("yyyy:MM:dd HH:mm:ss.fff");
                    if (metadata.DateTaken == null) metadata.DateTaken = dt.ToString("yyyy/MM/dd HH:mm:ss.fff");
                    if (metadata.GetQuery(META.TagIptcDateCreated) == null) metadata.SetQuery(META.TagIptcDateCreated, meta_date);
                    if (metadata.GetQuery(META.TagExifDateTime) == null) metadata.SetQuery(META.TagExifDateTime, meta_date);
                    if (metadata.GetQuery(META.TagExifDTOrig) == null) metadata.SetQuery(META.TagExifDTOrig, meta_date);
                    //if (metadata.GetQuery(META.TagExifDTOrigSS) == null) metadata.SetQuery(META.TagExifDTOrigSS, meta_date);
                    if (metadata.GetQuery(META.TagExifDTDigitized) == null) metadata.SetQuery(META.TagExifDTDigitized, meta_date);
                    //if (metadata.GetQuery(META.TagExifDTDigSS) == null) metadata.SetQuery(META.TagExifDTDigSS, meta_date);
                    if (metadata.GetQuery(META.TagExifDTSubsec) == null) metadata.SetQuery(META.TagExifDTSubsec, meta_date);
                    if (metadata.GetQuery(META.TagXmpCreateDate) == null) metadata.SetQuery(META.TagXmpCreateDate, meta_date);
                    if (metadata.GetQuery(META.TagXmpDateAcquired) == null) metadata.SetQuery(META.TagXmpDateAcquired, meta_date);
                    //if (metadata.GetQuery(META.TagTiffDateAcquired) == null) metadata.SetQuery(META.TagTiffDateAcquired, meta_date);
                    #endregion

                    #region Get Keywords
                    if (metadata.Keywords != null)
                    {
                        foreach (string keyword in metadata.Keywords)
                        {
                            keywords.Add(keyword.Trim());
                        }
                    }
                    var iptckeywords = metadata.GetQuery(META.TagIptcKeywords);
                    if (iptckeywords != null)
                    {
                        if (iptckeywords as string[] == null)
                        {
                            keywords.Add((iptckeywords as string).Trim());
                        }
                        else
                        {
                            foreach (string keyword in (iptckeywords as string[]))
                            {
                                keywords.Add(keyword.Trim());
                            }
                        }
                    }
                    BitmapMetadata xmpsubjects = metadata.GetQuery(META.TagXmpSubject) as BitmapMetadata;
                    if (xmpsubjects != null)
                    {
                        foreach (string query in xmpsubjects.ToList())
                        {
                            string keyword = xmpsubjects.GetQuery( query ) as string;
                            keywords.Add(keyword.Trim());
                        }
                    }
                    var xpkeywords = metadata.GetQuery(META.TagExifXPKeywords);
                    if (xpkeywords != null)
                    {
                        string xpkeywords_str = Encoding.Unicode.GetString( (byte[]) xpkeywords ).Trim( new char[] { ' ', '\0' } );
                        foreach (string key in xpkeywords_str.Split(';'))
                        {
                            keywords.Add(key.Trim());
                        }
                    }
                    #endregion

                    #region Get Authors
                    var artist = metadata.GetQuery( META.TagExifArtist );
                    if (artist != null)
                    {
                        foreach (string art in (artist as string).Split(';'))
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    if (wpfFileManager.BitmapMetadata.Author != null)
                    {
                        foreach (string art in wpfFileManager.BitmapMetadata.Author)
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    var xpauthor = metadata.GetQuery( META.TagExifXPAuthor );
                    if (xpauthor != null)
                    {
                        string xpauthor_str = Encoding.Unicode.GetString( (byte[]) xpauthor ).Trim( new char[] { ' ', '\0' } );
                        //metadata.SetQuery( META.TagIptcByline, xpauthor_str );
                        foreach (string art in xpauthor_str.Split(';'))
                        {
                            authors.Add(art.Trim(new char[] { ' ', '\0' }));
                        }
                    }
                    #endregion

                    #region Get Title

                    #endregion

                    #region Get Copyright

                    #endregion

                    #region Get Comments

                    #endregion

                    if (metadata.IsFrozen)
                    {
                        //metadata = metadata.Clone();
                    }

                    #region Set Title & Subject & Comment
                    var xptitle = metadata.GetQuery(META.TagExifXPTitle);
                    if (xptitle != null)
                    {
                        string xptitle_str = Encoding.Unicode.GetString( (byte[]) xptitle ).Trim(new char[] { ' ', '\0' } );
                        if (string.IsNullOrEmpty(xptitle_str) || Regex.IsMatch(xptitle_str, @"\?{2,}", RegexOptions.IgnoreCase)) xptitle_str = title;
                        metadata.SetQuery(META.TagIptcBylineTitle, Encoding.Unicode.GetBytes(xptitle_str));
                    }
                    else if (!string.IsNullOrEmpty(title))
                    {
                        metadata.Title = title;
                        metadata.SetQuery(META.TagExifXPTitle, Encoding.Unicode.GetBytes(title));
                        metadata.SetQuery(META.TagIptcBylineTitle, Encoding.Unicode.GetBytes(title));
                        //var meta_title = metadata.GetQuery(META.TagXmpTitleRoot) as BitmapMetadata;
                        //meta_title.SetQuery("/x-default", Encoding.Unicode.GetBytes(title));
                        //metadata.SetQuery(META.TagXmpTitle, Encoding.Unicode.GetBytes(title));
                    }
                    var xpcomment = metadata.GetQuery(META.TagExifXPComment);
                    if (xpcomment != null)
                    {
                        string xpcomment_str = Encoding.Unicode.GetString( (byte[]) xpcomment ).Trim( new char[] { ' ', '\0' } );
                        if (string.IsNullOrEmpty(xpcomment_str) || Regex.IsMatch(xpcomment_str, @"\?{2,}", RegexOptions.IgnoreCase)) xpcomment_str = comment;
                        metadata.SetQuery(META.TagIptcCaption, xpcomment_str);
                    }
                    else if (!string.IsNullOrEmpty(comment))
                    {
                        metadata.Comment = comment;
                        metadata.SetQuery(META.TagExifXPComment, Encoding.Unicode.GetBytes(comment));
                        metadata.SetQuery(META.TagIptcCaption, comment);
                    }
                    var xpsubject = metadata.GetQuery(META.TagExifXPSubject);
                    if (xpsubject != null)
                    {
                        string xpsubject_str = Encoding.Unicode.GetString( (byte[]) xpsubject ).Trim( new char[] { ' ', '\0' } );
                        if (string.IsNullOrEmpty(xpsubject_str) || Regex.IsMatch(xpsubject_str, @"\?{2,}", RegexOptions.IgnoreCase)) xpsubject_str = subject;
                        metadata.SetQuery(META.TagIptcHeadline, xpsubject_str);
                    }
                    else if (!string.IsNullOrEmpty(subject))
                    {
                        metadata.Subject = subject;
                        metadata.SetQuery(META.TagExifXPSubject, Encoding.Unicode.GetBytes(subject));
                        metadata.SetQuery(META.TagIptcHeadline, subject);
                        metadata.SetQuery(META.TagXmpDescription, comment);
                    }
                    var xpcopyright = metadata.GetQuery(META.TagExifCopyright);
                    if (xpcopyright != null && !string.IsNullOrEmpty((xpcopyright as string).Trim()))
                    {
                        string xpcopyright_str = (xpcopyright as string).Trim();
                        if (string.IsNullOrEmpty(xpcopyright_str) || Regex.IsMatch(xpcopyright_str, @"\?{2,}", RegexOptions.IgnoreCase)) xpcopyright_str = copyright;
                        metadata.SetQuery(META.TagIptcCopyrightNotice, xpcopyright_str);
                    }
                    else if (!string.IsNullOrEmpty(copyright))
                    {
                        metadata.Copyright = copyright;
                        metadata.SetQuery(META.TagExifCopyright, Encoding.Unicode.GetBytes(copyright));
                        metadata.SetQuery(META.TagIptcCopyrightNotice, copyright);
                    }
                    #endregion

                    #region Set Keywords & Authors
                    ulong idx = 0;
                    foreach (string keyword in keywords)
                    {
                        string query = $"{META.TagXmpSubject}/{{ulong={idx}}}";
                        metadata.SetQuery(query, keyword);
                        idx++;
                    }
                    if (keywords.Count > 0)
                    {
                        metadata.SetQuery(META.TagIptcKeywords, keywords.ToArray());
                    }
                    else if (!string.IsNullOrEmpty(keyword_list))
                    {
                        var keys = keyword_list.Split(new char[] { ';', '/', '\\', }).Select(k => k.Trim());
                        if (keys.Count() > 0) metadata.SetQuery(META.TagIptcKeywords, keys.ToArray());
                    }

                    if (authors.Count > 0)
                    {
                        metadata.SetQuery(META.TagIptcByline, string.Join(";", authors));
                    }
                    else if (!string.IsNullOrEmpty(author))
                    {
                        var aus = author.Split(new char[] { ';', '/', '\\', }).Select(k => k.Trim());
                        if (aus.Count() > 0) metadata.SetQuery(META.TagIptcByline, aus.ToArray());
                    }
                    #endregion

                    #region Set Image.Datetime to Taken datetime
                    if (!string.IsNullOrEmpty(metadata.DateTaken))
                    {
                        //metadata.SetQuery( META.TagExifDateTime, metadata.DateTaken );
                        //metadata.SetQuery(META.TagExifDateTime, metadata.DateTaken.Replace('/', ':').Replace('-', ':').Replace(',', ':').Replace('.', ':'));
                    }
                    #endregion
                    wpfFileManager.WriteMetadata();
                }
            }
            #endregion
        }
    }

    class META
    {
        #region Query Path
        ///
        /// all used the bitmapmetadata.getquery("[INSERT BELOW]")... warning data type that is returned is not always the same
        /// "they" could have made it much easier...I would still have some hair left in if they did !!
        ///
        // IPTC Tags
        public const string TagIptcByline = "/app13/irb/8bimiptc/iptc/{str=By-line}";
        public const string TagIptcBylineTitle = "/app13/irb/8bimiptc/iptc/{str=By-line Title}";
        public const string TagIptcCaption = "/app13/irb/8bimiptc/iptc/{str=Caption}";
        public const string TagIptcCity = "/app13/irb/8bimiptc/iptc/{str=City}";
        public const string TagIptcCopyrightNotice = "/app13/irb/8bimiptc/iptc/{str=Copyright Notice}";
        public const string TagIptcCountryPrimaryLocationName = "/app13/irb/8bimiptc/iptc/{str=Country/Primary Location Name}";
        public const string TagIptcCredit = "/app13/irb/8bimiptc/iptc/{str=Credit}";
        public const string TagIptcDateCreated = "/app13/irb/8bimiptc/iptc/{str=Date Created}";
        public const string TagIptcDescription = "/app13/irb/8bimiptc/iptc/{str=Description}";
        public const string TagIptcHeadline = "/app13/irb/8bimiptc/iptc/{str=Headline}";
        public const string TagIptcKeywords = "/app13/irb/8bimiptc/iptc/{str=Keywords}";
        public const string TagIptcObjectName = "/app13/irb/8bimiptc/iptc/{str=Object Name}";
        public const string TagIptcOriginalTransmissionReference = "/app13/irb/8bimiptc/iptc/{str=Original Transmission Reference}";
        public const string TagIptcRecordVersion = "/app13/irb/8bimiptc/iptc/{str=Record Version}";
        public const string TagIptcSource = "/app13/irb/8bimiptc/iptc/{str=Source}";
        public const string TagIptcState = "/app13/irb/8bimiptc/iptc/{str=Province/State}";
        public const string TagIptcSpecialInstructions = "/app13/irb/8bimiptc/iptc/{str=Special Instructions}";
        public const string TagIptcWriterEditor = "/app13/irb/8bimiptc/iptc/{str=Writer/Editor}";

        // EXIF Tags
        public const string TagExifExposureTime = "/app1/ifd/exif/{ushort=33434}";
        public const string TagExifFNumber = "/app1/ifd/exif/{ushort=33437}";
        public const string TagExifExposureProg = "/app1/ifd/exif/{ushort=34850}";
        public const string TagExifISOSpeed = "/app1/ifd/exif/{ushort=34855}";
        public const string TagExifDTOrig = "/app1/ifd/exif/{ushort=36867}";
        public const string TagExifDTDigitized = "/app1/ifd/exif/{ushort=36868}";
        public const string TagExifShutterSpeed = "/app1/ifd/exif/{ushort=37377}";
        public const string TagExifAperture = "/app1/ifd/exif/{ushort=37378}";
        public const string TagExifExposureBias = "/app1/ifd/exif/{ushort=37380}";
        public const string TagExifMaxAperture = "/app1/ifd/exif/{ushort=37381}";
        public const string TagExifMeteringMode = "/app1/ifd/exif/{ushort=37383}";
        public const string TagExifLightSource = "/app1/ifd/exif/{ushort=37384}";
        public const string TagExifFlash = "/app1/ifd/exif/{ushort=37385}";
        public const string TagExifFocalLength = "/app1/ifd/exif/{ushort=37386}";
        public const string TagExifUserComment = "/app1/ifd/exif/{ushort=37510}";
        public const string TagExifDTSubsec = "/app1/ifd/exif/{ushort=37520}";
        public const string TagExifDTOrigSS = "/app1/ifd/exif/{ushort=37521}";
        public const string TagExifDTDigSS = "/app1/ifd/exif/{ushort=37522}";
        public const string TagExifColorSpace = "/app1/ifd/exif/{ushort=40961}";
        public const string TagExifPixXDim = "/app1/ifd/exif/{ushort=40962}";
        public const string TagExifPixYDim = "/app1/ifd/exif/{ushort=40963}";
        public const string TagExifSensingMethod = "/app1/ifd/exif/{ushort=41495}";
        public const string TagExifFileSource = "/app1/ifd/exif/{ushort=41728}";
        public const string TagExifSceneType = "/app1/ifd/exif/{ushort=41729}";
        public const string TagExifCfaPattern = "/app1/ifd/exif/{ushort=41730}";
        public const string TagExifCustomRendered = "/app1/ifd/exif/{ushort=41985}";
        public const string TagExifExposureMode = "/app1/ifd/exif/{ushort=41986}";
        public const string TagExifWhiteBalance = "/app1/ifd/exif/{ushort=41987}";
        public const string TagExifDigitalZoomRatio = "/app1/ifd/exif/{ushort=41988}";
        public const string TagExifFocalLengthIn35mmFilm = "/app1/ifd/exif/{ushort=41989}";
        public const string TagExifSceneCaptureType = "/app1/ifd/exif/{ushort=41990}";
        public const string TagExifGainControl = "/app1/ifd/exif/{ushort=41991}";
        public const string TagExifContrast = "/app1/ifd/exif/{ushort=41992}";
        public const string TagExifSaturation = "/app1/ifd/exif/{ushort=41993}";
        public const string TagExifSharpness = "/app1/ifd/exif/{ushort=41994}";
        public const string TagExifSubjectDistanceRange = "/app1/ifd/exif/{ushort=41996}";

        // Exif others
        public const string TagExifTitle = "/app1/ifd/{ushort=270}";
        public const string TagExifDateTime = "/app1/ifd/{ushort=306}";
        public const string TagExifArtist = "/app1/ifd/{ushort=315}";
        public const string TagExifCopyright = "/app1/ifd/{ushort=33432}";

        // GPS Info
        public const string TagExifGpsLatitudeRef = "/app1/ifd/Gps/subifd:{uint=1}";
        public const string TagExifGpsLatitude = "/app1/ifd/Gps/subifd:{uint=2}";
        public const string TagExifGpsLongitudeRef = "/app1/ifd/Gps/subifd:{uint=3}";
        public const string TagExifGpsLongitude = "/app1/ifd/Gps/subifd:{uint=4}";
        public const string TagExifGpsAltitudeRef = "/app1/ifd/Gps/subifd:{uint=5}";
        public const string TagExifGpsAltitude = "/app1/ifd/Gps/subifd:{uint=6}";
        public const string TagExifGpsTimeStamp = "/app1/ifd/Gps/subifd:{uint=7}";
        public const string TagExifGpsSatellites = "/app1/ifd/Gps/subifd:{uint=8}";
        public const string TagExifGpsStatus = "/app1/ifd/Gps/subifd:{uint=9}";
        public const string TagExifGpsMeasureMode = "/app1/ifd/Gps/subifd:{uint=10}";
        public const string TagExifGpsDop = "/app1/ifd/Gps/subifd:{uint=11}";
        public const string TagExifGpsSpeedRef = "/app1/ifd/Gps/subifd:{uint=12}";
        public const string TagExifGpsSpeed = "/app1/ifd/Gps/subifd:{uint=13}";
        public const string TagExifGpsTrackRef = "/app1/ifd/Gps/subifd:{uint=14}";
        public const string TagExifGpsTrack = "/app1/ifd/Gps/subifd:{uint=15}";
        public const string TagExifGpsImgDirectionRef = "/app1/ifd/Gps/subifd:{uint=16}";
        public const string TagExifGpsImgDirection = "/app1/ifd/Gps/subifd:{uint=17}";
        public const string TagExifGpsMapDatum = "/app1/ifd/Gps/subifd:{uint=18}";
        public const string TagExifGpsDestLatitudeRef = "/app1/ifd/Gps/subifd:{uint=19}";
        public const string TagExifGpsDestLatitude = "/app1/ifd/Gps/subifd:{uint=20}";
        public const string TagExifGpsDestLongitudeRef = "/app1/ifd/Gps/subifd:{uint=21}";
        public const string TagExifGpsDestLongitude = "/app1/ifd/Gps/subifd:{uint=22}";
        public const string TagExifGpsDestBearingRef = "/app1/ifd/Gps/subifd:{uint=23}";
        public const string TagExifGpsDestBearing = "/app1/ifd/Gps/subifd:{uint=24}";
        public const string TagExifGpsDestDistanceRef = "/app1/ifd/Gps/subifd:{uint=25}";
        public const string TagExifGpsDestDistance = "/app1/ifd/Gps/subifd:{uint=26}";
        public const string TagExifGpsProcessingMethod = "/app1/ifd/Gps/subifd:{uint=27}";
        public const string TagExifGpsAreaInformation = "/app1/ifd/Gps/subifd:{uint=28}";
        public const string TagExifGpsDateStamp = "/app1/ifd/Gps/subifd:{uint=29}";
        public const string TagExifGpsDifferential = "/app1/ifd/Gps/subifd:{uint=30}";


        // alt query string
        public const string TagIptcKeywordsAlt = "/app13/{ushort=0}/{ulonglong=61857348781060}/iptc/{str=Keywords}";

        // XP System Property
        public const string TagExifImageDescription = "/app1/ifd/{ushort=270}";
        public const string TagExifXPTitle = "/app1/ifd/exif:{ushort=40091}";
        public const string TagExifXPComment = "/app1/ifd/exif:{ushort=40092}";
        public const string TagExifXPAuthor = "/app1/ifd/exif:{ushort=40093}";
        public const string TagExifXPKeywords = "/app1/ifd/exif:{ushort=40094}";
        public const string TagExifXPSubject = "/app1/ifd/exif:{ushort=40095}";
        
        // TIFF Paths
        public const string TagTiffDateAcquired = "/ifd/xmp/MicrosoftPhoto:DateAcquired";

        // XMP Query Path
        public const string TagXmpTitleRoot = "/xmp/dc:title";
        public const string TagXmpTitle = "/xmp/dc:title/x-default";
        public const string TagXmpSubject = "/xmp/dc:subject";
        public const string TagXmpDescription = "/xmp/dc:description";
        public const string TagXmpCreateDate = "/xmp/xmp:CreateDate";
        public const string TagXmpDateAcquired = "/xmp/MicrosoftPhoto:DateAcquired";
        public const string TagXmpCopyrights = "/xmp/dc:rights";
        public const string TagXmpPaddingSchema = "/xmp/PaddingSchema:padding";

        // Extentions for metadata padding
        // Queries for the EXIF, IFD & XMP Padding
        public const string paddingExif = "/app1/ifd/exif/PaddingSchema:Padding";
        public const string paddingIfd = "/app1/ifd/PaddingSchema:Padding";
        public const string paddingXmp = "/xmp/PaddingSchema:Padding";    // Queries for the EXIF, IFD & XMP Padding
        public const string paddingIptc = "/app13/irb/8bimiptc/iptc/PaddingSchema:Padding";
        public const string padding8bimiptc = "/app13/irb/8bimiptc/PaddingSchema:Padding";
        public const string paddinIrb = "/app13/irb//PaddingSchema:Padding";
        public const string paddingApp13 = "/app13/PaddingSchema:Padding";
        #endregion

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
        public GeoRegion(double w, double e, double n, double s)
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
        string AppPath = AppDomain.CurrentDomain.BaseDirectory;

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
        public Boolean pointInPolygon(double x, double y)
        {

            int polyCorners = polyX.Count;
            int i, j = polyCorners - 1;
            bool oddNodes = false;

            for (i = 0; i < polyCorners; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y)
                 || (polyY[j] < y && polyY[i] >= y)
                 && (polyX[i] <= x || polyX[j] <= x))
                {
                    if (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x)
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
            if (System.IO.File.Exists(OffsetFile))
            {
                using (StreamReader sr = new StreamReader(OffsetFile))
                {
                    string s = sr.ReadToEnd();

                    Match MP = Regex.Match(s, "(\\d\\.\\d{6,6}E\\+\\d{2,2})");

                    int i = 0;
                    while (MP.Success)
                    {
                        //MessageBox.Show(MP.Value);
                        if (i % 2 == 0) //第一列
                        {
                            polyX.Add(Convert.ToDouble(MP.Value));
                        }
                        else //第二列
                        {
                            polyY.Add(Convert.ToDouble(MP.Value));
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
        private Boolean isInRect(GeoRegion rect, double lon, double lat)
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
        private Boolean isInChina(double lon, double lat, Boolean simple = false)
        {
            if (simple)
            {
                for (int i = 0; i < chinaRegion.Count; i++)
                {
                    if (isInRect(chinaRegion[i], lon, lat))
                    {
                        for (int j = 0; j < excludeRegion.Count; j++)
                        {
                            if (isInRect(excludeRegion[j], lon, lat))
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
                return pointInPolygon(lon, lat);
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
        public void Convert2WGS(double xMars, double yMars, out double xWgs, out double yWgs)
        {
            double xtry, ytry, dx, dy;

            xWgs = xMars;
            yWgs = yMars;

            if (!isInChina(xMars, yMars)) return;

            xtry = xMars;
            ytry = yMars;
            Convert2Mars(xMars, yMars, out xtry, out ytry);
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
        public void Convert2Mars(double xWgs, double yWgs, out double xMars, out double yMars)
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
            if (!isInChina(xWgs, yWgs)) return;

            double x=0, y=0;
            x = xWgs - 105.0;
            y = yWgs - 35.0;

            double dLon =  300.0 + 1.0 * x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt( Math.Abs( x ) );
            dLon += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLon += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            dLon += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;

            double dLat = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt( Math.Abs( x ) );
            dLat += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLat += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            dLat += (160.0 * Math.Sin(y / 12.0 * pi) + 320.0 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;

            double radLat = yWgs / 180.0 * pi;
            double magic = Math.Sin( radLat );
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt( magic );
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            xMars = xWgs + dLon;
            yMars = yWgs + dLat;
        }

        /// <summary>
        /// 
        /// </summary>
        public MarsWGS()
        {
            chinaRegion.Add(new GeoRegion(79.446200, 49.220400, 96.330000, 42.889900));
            chinaRegion.Add(new GeoRegion(109.687200, 54.141500, 135.000200, 39.374200));
            chinaRegion.Add(new GeoRegion(73.124600, 42.889900, 124.143255, 29.529700));
            chinaRegion.Add(new GeoRegion(82.968400, 29.529700, 97.035200, 26.718600));
            chinaRegion.Add(new GeoRegion(97.025300, 29.529700, 124.367395, 20.414096));
            chinaRegion.Add(new GeoRegion(107.975793, 20.414096, 111.744104, 17.871542));
            excludeRegion.Add(new GeoRegion(119.921265, 25.398623, 122.497559, 21.785006));
            excludeRegion.Add(new GeoRegion(101.865200, 22.284000, 106.665000, 20.098800));
            excludeRegion.Add(new GeoRegion(106.452500, 21.542200, 108.051000, 20.487800));
            excludeRegion.Add(new GeoRegion(109.032300, 55.817500, 119.127000, 50.325700));
            excludeRegion.Add(new GeoRegion(127.456800, 55.817500, 137.022700, 49.557400));
            excludeRegion.Add(new GeoRegion(131.266200, 44.892200, 137.022700, 42.569200));

            LoadPolygon();
        }
    }

    class GPX
    {
        public class META
        {
            struct AUTHOR
            {
                struct EMAIL
                {
                    string id { get; set; }
                    string domain { get; set; }
                }
                string name { get; set; }
                EMAIL email { get; set; }
            }

            struct COPYRIGHT
            {
                string year { get; set; }
                string license { get; set; }
            }

            struct LINK
            {
                string href { get; set; }
                string text { get; set; }
            }

            string desc { get; set; }
            string author { get; set; }
            COPYRIGHT copyright { get; set; }
            List<string> keywords = new List<string>();
            LINK link { get; set; }
            DateTime time { get; set; }
            RectangleF bounds { get; set; }
        }

        public class WAYPOINT
        {
            double latitude { get; set; }
            double longitude { get; set; }
            double altitude { get; set; }

            DateTime time { get; set; }
            string name { get; set; }
        }

        public class TRACKPOINT
        {
            double latitude { get; set; }
            double longitude { get; set; }
            double altitude { get; set; }

            DateTime time { get; set; }
        }

        public class TRACK
        {
            string name { get; set; }
            List<List<TRACKPOINT>> segments = new List<List<TRACKPOINT>>();
        }

        public META metadata { get; set; }
        public List<WAYPOINT> waypoints = new List<WAYPOINT>();
        public List<TRACK> tracks = new List<TRACK>();

        public void load(string gpxfile)
        {

        }
    }
}
