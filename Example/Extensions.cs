using MahApps.Metro.IconPacks;
using System.Windows.Media;

namespace Example
{
    public class Extensions
    {
        public static double Thickness { get; set; } = 0.25;
        public static DrawingImage MakeIcon(object value, object parameter = null)
        {
            GeometryDrawing geoDrawing = new GeometryDrawing();
            geoDrawing.Brush = parameter as Brush ?? Brushes.Black;
            geoDrawing.Pen = new Pen(geoDrawing.Brush, Thickness);
            if (value is PackIconFontAwesome)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconFontAwesomeKind>).Data);
            else if (value is PackIconMaterial)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconMaterialKind>).Data);
            else if (value is PackIconMaterialLight)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconEntypoKind>).Data);
            else if (value is PackIconModern)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconModernKind>).Data);
            else if (value is PackIconEntypo)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconEntypoKind>).Data);
            else if (value is PackIconOcticons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconControl<PackIconOcticonsKind>).Data);
            return new DrawingImage { Drawing = new DrawingGroup { Children = { geoDrawing }, Transform = new ScaleTransform(-1, 1) } };
        }

    }
}