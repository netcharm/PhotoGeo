
namespace GMap.NET.MapProviders
{
    using System;

    /// <summary>
    /// GoogleCnSatelliteMap provider
    /// </summary>
    public class GoogleCnSatelliteMapProvider : GoogleMapProviderBase
    {
        public static readonly GoogleCnSatelliteMapProvider Instance;

        GoogleCnSatelliteMapProvider()
        {
            RefererUrl = string.Format("http://www.google.cn/");
        }

        static GoogleCnSatelliteMapProvider()
        {
            Instance = new GoogleCnSatelliteMapProvider();
        }

        public string Version = "s@170";

        #region GMapProvider Members

        readonly Guid id = new Guid("83A19850-722D-4361-8E24-CCED2720E2B5");
        public override Guid Id
        {
            get
            {
                return id;
            }
        }

        readonly string name = "GoogleCnSatelliteMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        #endregion

        internal void GetSecureWords(GPoint pos, out string sec1, out string sec2)
        {
            sec1 = string.Empty; // after &x=...
            sec2 = string.Empty; // after &zoom=...
            int seclen = (int)((pos.X * 3) + pos.Y) % 8;
            sec2 = SecureWord.Substring(0, seclen);
            if (pos.Y >= 10000 && pos.Y < 100000)
            {
                sec1 = Sec1;
            }
        }
        static readonly string Sec1 = "&s=";


        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            string sec1 = string.Empty; // after &x=...
            string sec2 = string.Empty; // after &zoom=...
            GetSecureWords(pos, out sec1, out sec2);

            return string.Format(UrlFormat, UrlFormatServer, GetServerNum(pos, 1), UrlFormatRequest, Version, pos.X, sec1, pos.Y, zoom, sec2, ServerChina);
        }

        static readonly string UrlFormatServer = "mt";
        static readonly string UrlFormatRequest = "vt";
        static readonly string UrlFormat = "http://{0}{1}.{9}/{2}/lyrs={3}&gl=cn&x={4}{5}&y={6}&z={7}&s={8}";
    }
}