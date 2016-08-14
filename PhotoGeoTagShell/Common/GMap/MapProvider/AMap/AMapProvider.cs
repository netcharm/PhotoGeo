using System;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders.AMap
{
    public abstract class AMapProviderBase : GMapProvider
    {
        public AMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://www.amap.com/";
            Copyright = string.Format("©{0} 高德 Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);    
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }

    public class AMapProvider : AMapProviderBase
    {
        public static readonly AMapProvider Instance;
   
        readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE88");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapProvider()
        {
            Instance = new AMapProvider();
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {

            //http://webrd04.is.autonavi.com/appmaptile?x=5&y=2&z=3&lang=zh_cn&size=1&scale=1&style=7
            long num = (pos.X + pos.Y) % 4L + 1L;
            string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            Console.WriteLine("url:" + url);
            return url;
        }

        //static readonly string UrlFormat = "http://webrd04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=7";
        //static readonly string UrlFormat = "http://webrd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={1}&y={2}&z={3}";
        static readonly string UrlFormat = "http://wprd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={1}&y={2}&z={3}";
    }

    public class AMapHybirdProvider : AMapProviderBase
    {
        public static readonly AMapHybirdProvider Instance;

        readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE87");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMapHybird";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapHybirdProvider()
        {
            Instance = new AMapHybirdProvider();
        }

        public override PureImage GetTileImage( GPoint pos, int zoom )
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp( url );
        }

        string MakeTileImageUrl( GPoint pos, int zoom, string language )
        {

            long num = (pos.X + pos.Y) % 4L + 1L;
            string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            Console.WriteLine( "url:" + url );
            return url;
        }

        //static readonly string UrlFormat = "http://webst04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=8";
        //static readonly string UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=8&x={1}&y={2}&z={3}";
        static readonly string UrlFormat = "http://wprd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&style=8&x={1}&y={2}&z={3}&scl=1";
    }
}
