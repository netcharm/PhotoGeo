using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GMap.NET.WindowsForms.ToolTips
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System;
    using System.Runtime.Serialization;

#if !PocketPC
    /// <summary>
    /// GMap.NET marker
    /// </summary>
    [Serializable]
    public class GMapImageToolTip : GMapToolTip, ISerializable
    {
        public float Radius = 10f;
        //private float Radius = 10f;

        public Image tooltipImage;
        public Image Image
        {
            get { return ( tooltipImage ); }
            set { tooltipImage = value; }
        }

        public GMapImageToolTip( GMapMarker marker )
           : base( marker )
        {
            Stroke = new Pen( Color.FromArgb( 140, Color.Navy ) );
            Stroke.Width = 3;
#if !PocketPC
            this.Stroke.LineJoin = LineJoin.Round;
            this.Stroke.StartCap = LineCap.RoundAnchor;
#endif
            Offset.X = 0;
            Offset.Y = 0;

            Fill = Brushes.Yellow;
        }

        public override void OnRender( Graphics g )
        {
            Size st = g.MeasureString(Marker.ToolTipText, Font).ToSize();
            Size si = new Size(0, 0);
            int padFactor_X = 0;
            int padFactor_Y = 0;
            if (tooltipImage != null)
            {
                si = tooltipImage.Size;
                padFactor_Y = 1;
            }    
            
            int x = Marker.ToolTipPosition.X;
            int y = Marker.ToolTipPosition.Y - st.Height;
            int w = st.Width > si.Width ? st.Width : si.Width;
            int h = st.Height + si.Height;
            w = w + ( 2 + padFactor_X ) * TextPadding.Width;
            h = h + ( 1 + padFactor_Y ) * TextPadding.Height;
            Rectangle rect = new Rectangle( x, y, w, h );
            //rect.Offset( Offset.X, Offset.Y );
            rect.Offset( (int) ( -w / 2f ) + Offset.X , - h - (int)Radius - 4 + Offset.Y);

            using ( GraphicsPath objGP = new GraphicsPath() )
            {
                float cX = rect.X + rect.Width  / 2f;
                float cY = rect.Y + rect.Height / 2f;
                float rW = rect.Width;
                float rH = rect.Height;
                float rT = rect.Y;
                float rB = rect.Y + rect.Height;
                float rL = rect.X;
                float rR = rect.X + rect.Width;

                objGP.AddLine( cX + Radius / 2, rB, cX, rB + Radius );
                objGP.AddLine( cX, rB + Radius, cX - Radius / 2, rB );

                objGP.AddLine( cX - Radius / 2, rB, rL + Radius, rB );
                objGP.AddArc( rL, rB - ( Radius * 2 ), Radius * 2, Radius * 2, 90, 90 );
                objGP.AddLine( rL, rB - ( Radius * 2 ), rL, rT + Radius );
                objGP.AddArc( rL, rT, Radius * 2, Radius * 2, 180, 90 );
                objGP.AddLine( rL + Radius, rT, rR - ( Radius * 2 ), rT );
                objGP.AddArc( rR - ( Radius * 2 ), rT, Radius * 2, Radius * 2, 270, 90 );
                objGP.AddLine( rR, rT + Radius, rR, rB - ( Radius * 2 ) );
                objGP.AddArc( rR - ( Radius * 2 ), rB - ( Radius * 2 ), Radius * 2, Radius * 2, 0, 90 ); // Corner

                objGP.CloseFigure();

                g.FillPath( Fill, objGP );
                g.DrawPath( Stroke, objGP );
            }
            if(tooltipImage != null)
            {
                g.DrawImage( tooltipImage, rect.X + ( rect.Width - si.Width ) / 2f, rect.Y + Radius );
            }

#if !PocketPC
            float tX = rect.X + rect.Width  / 2f;
            float tY = rect.Y + si.Height + padFactor_Y * TextPadding.Height + 4;
            float tL = st.Width > si.Width ? rect.X + Radius / 2 : tX - si.Width / 2f+ Radius;
            RectangleF sr = new RectangleF(
                tL,
                tY,
                st.Width + TextPadding.Width,
                st.Height
                );
            g.DrawString( Marker.ToolTipText, Font, Foreground, sr, Format );
            //g.DrawString( Marker.ToolTipText, Font, Foreground, rect.X + ( rect.Width + st.Width ) / 2f, rect.Y + si.Height + padFactor_Y * TextPadding.Height + 4, Format );
            //g.DrawString( Marker.ToolTipText, Font, Foreground, rect.X + Radius, rect.Y + si.Height + padFactor_Y * TextPadding.Height, Format );
#else
            g.DrawString(ToolTipText, ToolTipFont, TooltipForeground, rect.X, rect.Y + si.Height + padFactor_Y * TextPadding.Height, ToolTipFormat);
#endif
        }

        #region ISerializable Members

        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( "Radius", this.Radius );

            base.GetObjectData( info, context );
        }

        protected GMapImageToolTip( SerializationInfo info, StreamingContext context )
           : base( info, context )
        {
            this.Radius = Extensions.GetStruct<float>( info, "Radius", 10f );
        }

        #endregion
    }

    /// <summary>
    /// GMap.NET marker
    /// </summary>
    [Serializable]
    public class GMapHTMLToolTip : GMapToolTip, ISerializable
    {
        public float Radius = 10f;

        public GMapHTMLToolTip( GMapMarker marker )
           : base( marker )
        {
            Stroke = new Pen( Color.FromArgb( 140, Color.Navy ) );
            Stroke.Width = 3;
#if !PocketPC
            this.Stroke.LineJoin = LineJoin.Round;
            this.Stroke.StartCap = LineCap.RoundAnchor;
#endif

            Fill = Brushes.Yellow;
        }

        public override void OnRender( Graphics g )
        {
            Size st = g.MeasureString(Marker.ToolTipText, Font).ToSize();
            Rectangle rect = new Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - st.Height, st.Width + TextPadding.Width, st.Height + TextPadding.Height);
            rect.Offset( Offset.X, Offset.Y );

            using ( GraphicsPath objGP = new GraphicsPath() )
            {
                objGP.AddLine( rect.X + 2 * Radius, rect.Y + rect.Height, rect.X + Radius, rect.Y + rect.Height + Radius );
                objGP.AddLine( rect.X + Radius, rect.Y + rect.Height + Radius, rect.X + Radius, rect.Y + rect.Height );

                objGP.AddArc( rect.X, rect.Y + rect.Height - ( Radius * 2 ), Radius * 2, Radius * 2, 90, 90 );
                objGP.AddLine( rect.X, rect.Y + rect.Height - ( Radius * 2 ), rect.X, rect.Y + Radius );
                objGP.AddArc( rect.X, rect.Y, Radius * 2, Radius * 2, 180, 90 );
                objGP.AddLine( rect.X + Radius, rect.Y, rect.X + rect.Width - ( Radius * 2 ), rect.Y );
                objGP.AddArc( rect.X + rect.Width - ( Radius * 2 ), rect.Y, Radius * 2, Radius * 2, 270, 90 );
                objGP.AddLine( rect.X + rect.Width, rect.Y + Radius, rect.X + rect.Width, rect.Y + rect.Height - ( Radius * 2 ) );
                objGP.AddArc( rect.X + rect.Width - ( Radius * 2 ), rect.Y + rect.Height - ( Radius * 2 ), Radius * 2, Radius * 2, 0, 90 ); // Corner

                objGP.CloseFigure();

                g.FillPath( Fill, objGP );
                g.DrawPath( Stroke, objGP );
            }

#if !PocketPC
            g.DrawString( Marker.ToolTipText, Font, Foreground, rect, Format );
#else
            g.DrawString(ToolTipText, ToolTipFont, TooltipForeground, rect, ToolTipFormat);
#endif
        }

        #region ISerializable Members

        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( "Radius", this.Radius );

            base.GetObjectData( info, context );
        }

        protected GMapHTMLToolTip( SerializationInfo info, StreamingContext context )
           : base( info, context )
        {
            this.Radius = Extensions.GetStruct<float>( info, "Radius", 10f );
        }

        #endregion
    }


#endif
}
