using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ControlzEx.Theming;
using MahApps.Metro;

namespace Example
{
    public static class ThemeManagerHelper
    {
        public class AccentColorMenuData
        {
            public string Name { get; set; }

            public Brush BorderColorBrush { get; set; }

            public Brush ColorBrush { get; set; }

        }
        public class AppThemeMenuData : AccentColorMenuData
        {
        }
    }
}