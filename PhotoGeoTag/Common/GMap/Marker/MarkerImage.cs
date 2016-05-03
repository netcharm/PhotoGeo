using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMap.NET.WindowsForms.Markers
{
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class GMapMarkerImage : GMapMarker
    {
        private Image tooltipImage;
        public Image Image
        {
            get { return ( tooltipImage ); }
            set { tooltipImage = value; }
        }


        public GMapMarkerImage( PointLatLng p )
            : base( p )
        {
            //TTBorder.Width = 2;
            //TTBorder.LineJoin = LineJoin.Round;
            //TTBorder.StartCap = LineCap.RoundAnchor;
            //TTFormat.Alignment = StringAlignment.Center;
            //TTFormat.LineAlignment = StringAlignment.Center;
        }

        public override void OnRender( Graphics g )
        {
            g.DrawImage( Image, Convert.ToInt32( LocalPosition.X - Size.Width / 2 ), Convert.ToInt32( LocalPosition.Y - Size.Height / 2 ), Size.Width, Size.Height );
        }
    }

    public class GMapMarkerHTML : GMapMarker
    {
        private Image tooltipImage;
        public Image Image
        {
            get { return ( tooltipImage ); }
            set { tooltipImage = value; }
        }
        private WebBrowser browser;

        public GMapMarkerHTML( PointLatLng p )
            : base( p )
        {
            browser = new WebBrowser();
            browser.Size = new Size( 240, 240 );
        }

        public override void OnRender( Graphics g )
        {

            g.DrawImage( tooltipImage, Convert.ToInt32( LocalPosition.X - Size.Width / 2 ), Convert.ToInt32( LocalPosition.Y - Size.Height / 2 ), Size.Width, Size.Height );
        }
    }

}
