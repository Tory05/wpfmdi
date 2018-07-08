using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro;

namespace Example
{
    public static class ThemeManagerHelper
    {
        public static void CreateAppStyleBy(Color color, bool changeImmediately = false)
        {
            // create a runtime accent resource dictionary

            var resourceDictionary = new ResourceDictionary
            {
                { "HighlightColor", color },
                { "AccentBaseColor", color },
                { "AccentColor", Color.FromArgb(204, color.R, color.G, color.B) },
                { "AccentColor2", Color.FromArgb(153, color.R, color.G, color.B) },
                { "AccentColor3", Color.FromArgb(102, color.R, color.G, color.B) },
                { "AccentColor4", Color.FromArgb(51, color.R, color.G, color.B) }
            };

            resourceDictionary.Add("HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["HighlightColor"]));
            resourceDictionary.Add("AccentBaseColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentBaseColor"]));
            resourceDictionary.Add("AccentColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("AccentColorBrush2", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("AccentColorBrush3", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("AccentColorBrush4", GetSolidColorBrush((Color)resourceDictionary["AccentColor4"]));

            resourceDictionary.Add("WindowTitleColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(
                new GradientStopCollection(new[]
                {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                }),
                // StartPoint="1.002,0.5" EndPoint="0.001,0.5"
                startPoint: new Point(1.002, 0.5), endPoint: new Point(0.001, 0.5)));

            resourceDictionary.Add("CheckmarkFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("RightArrowFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("IdealForegroundColor", IdealTextColor(color));
            resourceDictionary.Add("IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"], 0.4));
            resourceDictionary.Add("AccentSelectedColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            resourceDictionary.Add("MetroDataGrid.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            // applying theme to MahApps

            var fileName = Path.Combine(Path.GetTempPath(), string.Format("ApplicationAccent_{0}.xaml", color.ToString().Replace("#", string.Empty)));
            using (var writer = System.Xml.XmlWriter.Create(fileName, new System.Xml.XmlWriterSettings { Indent = true }))
            {
                System.Windows.Markup.XamlWriter.Save(resourceDictionary, writer);
                writer.Close();
            }

            resourceDictionary = new ResourceDictionary() { Source = new Uri(fileName, UriKind.Absolute) };

            ThemeManager.AddAccent(new Accent { Name = string.Format("ApplicationAccent_{0}.xaml", color.ToString().Replace("#", string.Empty)), Resources = resourceDictionary }.Name, new Accent { Name = string.Format("ApplicationAccent_{0}.xaml", color.ToString().Replace("#", string.Empty)), Resources = resourceDictionary }.Resources.Source);

            if (changeImmediately)
            {
                //var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));
                // detect current application theme
                Tuple<AppTheme, Accent> applicationTheme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, new Accent { Name = string.Format("ApplicationAccent_{0}.xaml", color.ToString().Replace("#", string.Empty)), Resources = resourceDictionary }, applicationTheme.Item1);
            }
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "color">The bg.</param>
        /// <returns></returns>
        private static Color IdealTextColor(Color color)
        {
            int bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            return (255 - bgDelta < 105) ? Colors.Black : Colors.White;
        }

        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }
    }
}