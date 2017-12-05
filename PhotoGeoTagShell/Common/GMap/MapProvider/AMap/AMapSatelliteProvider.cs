using System;

namespace GMap.NET.MapProviders.AMap
{
    public class AMapSatelliteProvider : AMapProviderBase
    {
        public static readonly AMapSatelliteProvider Instance;

        readonly Guid id = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BC");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMapSatellite";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapSatelliteProvider()
        {
            Instance = new AMapSatelliteProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {

            //http://webst04.is.autonavi.com/appmaptile?x=23&y=12&z=5&lang=zh_cn&size=1&scale=1&style=8
            long num = (pos.X + pos.Y) % 4L + 1L;
            string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            Console.WriteLine("url:" + url);
            return url;
        }

        //static readonly string UrlFormat = "http://webst04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=6";
        //static readonly string UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=6&x={1}&y={2}&z={3}";
        static readonly string UrlFormat = "http://wprd0{0}.is.autonavi.com/appmaptile?style=6&lang=zh_cn&size=1&scale=1&x={1}&y={2}&z={3}";
    }
}
