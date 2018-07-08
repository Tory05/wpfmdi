using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Example
{
    public partial class AccentStyleWindow : MetroWindow
    {
        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
            "Colors",
            typeof(List<KeyValuePair<string, Color>>),
            typeof(AccentStyleWindow),
            new PropertyMetadata(
                default(List<KeyValuePair<string, Color>>))
            );

        public List<KeyValuePair<string, Color>> Colors
        {
            get => (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty);

            set => SetValue(ColorsProperty, value);
        }

        public AccentStyleWindow()
        {
            InitializeComponent();

            DataContext = this;

            Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<string, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            ThemeManager.ChangeAppStyle(this, ThemeManager.DetectAppStyle(Application.Current).Item2, ThemeManager.DetectAppStyle(Application.Current).Item1);
        }

        private void ChangeWindowThemeButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(this, ThemeManager.DetectAppStyle(this).Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
        }

        private void ChangeWindowAccentButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent(((Button)sender).Content.ToString()), ThemeManager.DetectAppStyle(this).Item1);
        }

        private void ChangeAppThemeButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.DetectAppStyle(Application.Current).Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
        }

        private void ChangeAppAccentButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(((Button)sender).Content.ToString()), ThemeManager.DetectAppStyle(Application.Current).Item1);
        }

        private void CustomThemeAppButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.DetectAppStyle(Application.Current).Item2, ThemeManager.GetAppTheme("CustomTheme"));
        }

        private void CustomAccent1AppButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("CustomAccent1"), ThemeManager.DetectAppStyle(Application.Current).Item1);
        }

        private void CustomAccent2AppButtonClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("CustomAccent2"), ThemeManager.DetectAppStyle(Application.Current).Item1);
        }

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccentSelect.SelectedItem as Accent != null)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, AccentSelect.SelectedItem as Accent, theme.Item1);
                Application.Current.MainWindow.Activate();
            }
        }

        private void ColorsSelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((ColorsSelect.SelectedItem as KeyValuePair<string, Color>?).HasValue)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManagerHelper.CreateAppStyleBy((ColorsSelect.SelectedItem as KeyValuePair<string, Color>?).Value.Value, true);
                Application.Current.MainWindow.Activate();
            }
        }
    }
}
