using MahApps.Metro.IconPacks;
using System.Windows.Media;

namespace Example
{
    public class Extensions
    {
        public static double Thickness { get; set; } = 0.25;
        public static DrawingImage MakeIcon(object value, object parameter = null)
        {

            GeometryDrawing geoDrawing = new GeometryDrawing
            {
                Brush = parameter as Brush ?? Brushes.Black
            };
            geoDrawing.Pen = new Pen(geoDrawing.Brush, Thickness);
            if (value is PackIconFontAwesome)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconFontAwesome).Data);
            else if (value is PackIconMaterial)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconMaterial).Data);
            else if (value is PackIconMaterialLight)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconMaterialLight).Data);
            else if (value is PackIconModern)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconModern).Data);
            else if (value is PackIconEntypo)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconEntypo).Data);
            else if (value is PackIconOcticons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconOcticons).Data);
            else if (value is PackIconBoxIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconBoxIcons).Data);
            else if (value is PackIconEvaIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconEvaIcons).Data);
            else if (value is PackIconFeatherIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconFeatherIcons).Data);
            else if (value is PackIconIonicons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconIonicons).Data);
            else if (value is PackIconJamIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconJamIcons).Data);
            else if (value is PackIconMaterialDesign)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconMaterialDesign).Data);
            else if (value is PackIconMicrons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconMicrons).Data);
            else if (value is PackIconPicolIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconPicolIcons).Data);
            else if (value is PackIconRPGAwesome)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconRPGAwesome).Data);
            else if (value is PackIconSimpleIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconSimpleIcons).Data);
            else if (value is PackIconTypicons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconTypicons).Data);
            else if (value is PackIconUnicons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconUnicons).Data);
            else if (value is PackIconWeatherIcons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconWeatherIcons).Data);
            else if (value is PackIconZondicons)
                geoDrawing.Geometry = Geometry.Parse((value as PackIconZondicons).Data);
            return new DrawingImage { Drawing = new DrawingGroup { Children = { geoDrawing }, Transform = new ScaleTransform(-1, 1) } };
        }

    }
}