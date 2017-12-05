
namespace GMap.NET.MapProviders
{
    using System;

    /// <summary>
    /// GoogleCnMap provider
    /// </summary>
    public class GoogleCnMapProvider : GoogleMapProviderBase
    {
        public static readonly GoogleCnMapProvider Instance;

        GoogleCnMapProvider()
        {
            RefererUrl = string.Format("http://www.google.cn/");
        }

        static GoogleCnMapProvider()
        {
            Instance = new GoogleCnMapProvider();
        }

        public string Version = "m@298";

        #region GMapProvider Members

        readonly Guid id = new Guid("E6AAE0D9-DA18-4DA8-BCE5-D752E114616F");
        public override Guid Id
        {
            get
            {
                return id;
            }
        }

        readonly string name = "GoogleCnMap";
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

            return string.Format(UrlFormat, UrlFormatServer, GetServerNum(pos, 1), UrlFormatRequest, Version, ChinaLanguage, pos.X, sec1, pos.Y, zoom, sec2, ServerChina);
        }

        static readonly string ChinaLanguage = "zh-CN";
        static readonly string UrlFormatServer = "mt";
        static readonly string UrlFormatRequest = "vt";
        static readonly string UrlFormat = "http://{0}{1}.{10}/{2}/lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}";
    }
}