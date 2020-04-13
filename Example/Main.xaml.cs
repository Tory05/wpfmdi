using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Example.Controls;
using WPF.MDI;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System.Windows.Media;
using System.Windows.Data;
using System.Collections.Specialized;

namespace Example
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            InitializeComponent();
            _original_title = Title;
            Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows(o, e);
            Container.MdiChildTitleChanged += Container_MdiChildTitleChanged;

            Container.Children.Add(new MdiChild
            {
                Title = "Empty Window Using Code",
                Icon = Extensions.MakeIcon(new PackIconOcticons { Kind = PackIconOcticonsKind.GitCompare })
            });
            MdiChild mdi = new MdiChild
            {
                Title = "Window Using Code",
                Width = 714,
                Height = 734,
                Position = new Point(300, 80),
                Icon = Extensions.MakeIcon(new PackIconMaterial { Kind = PackIconMaterialKind.Airplane},Brushes.White)
            };
            mdi.Activated += Window_Activated;
            mdi.Deactivated += Window_Deactivated;
            mdi.Content = new ExampleControl(mdi);

            Container.Children.Add(mdi);
        }

        #region Mdi-like title

        string _original_title;

        void Container_MdiChildTitleChanged(object sender, RoutedEventArgs e)
        {
            Title = Container.ActiveMdiChild != null && Container.ActiveMdiChild.WindowState == WindowState.Maximized
                ? _original_title + " - [" + Container.ActiveMdiChild.Title + "]"
                : _original_title;
        }

        #endregion

        #region Theme Menu Events

        /// <summary>
        /// Handles the Click event of the Generic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Generic_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = true;
            Luna.IsChecked = false;
            Aero.IsChecked = false;
            Metro.IsChecked = false;

            Container.Theme = ThemeType.Generic;
        }

        /// <summary>
        /// Handles the Click event of the Luna control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Luna_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = true;
            Aero.IsChecked = false;
            Metro.IsChecked = false;

            Container.Theme = ThemeType.Luna;
        }

        /// <summary>
        /// Handles the Click event of the Aero control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Aero_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = false;
            Aero.IsChecked = true;
            Metro.IsChecked = false;

            Container.Theme = ThemeType.Aero;
        }
        /// <summary>
		/// Handles the Click event of the Metro control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Metro_Click(object sender, RoutedEventArgs e)
        {
            Generic.IsChecked = false;
            Luna.IsChecked = false;
            Aero.IsChecked = false;
            Metro.IsChecked = true;

            Container.Theme = ThemeType.Metro;
        }

        #endregion

        #region Menu Events

        int ooo = 1;

        /// <summary>
        /// Handles the Click event of the 'Normal window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild { Content = new Label { Content = "Normal window" }, Title = "Window " + ooo++ });
        }

        /// <summary>
        /// Handles the Click event of the 'Fixed window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddFixedWindow_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild { Content = new Label { Content = "Fixed width window" }, Title = "Window " + ooo++, Resizable = false });
        }

        /// <summary>
        /// Handles the Click event of the 'Scroll window' menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddScrollWindow_Click(object sender, RoutedEventArgs e)
        {
            StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
            sp.Children.Add(new TextBlock { Text = "Window with scroll", Margin = new Thickness(5) });
            sp.Children.Add(new ComboBox { Margin = new Thickness(20), Width = 300 });
            ScrollViewer sv = new ScrollViewer { Content = sp, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };

            Container.Children.Add(new MdiChild { Content = sv, Title = "Window " + ooo++ });
        }

        /// <summary>
        /// Refresh windows list
        /// </summary>
        public void Menu_RefreshWindows(object sender, NotifyCollectionChangedEventArgs Args)
        {
            switch (Args.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    WindowsChildMenu.Children.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < Args.NewItems.Count; i++)
                    {
                        MdiChild mdi_ = Args.NewItems[i] as MdiChild;
                        DrawingImage drawing = ((DrawingImage)mdi_.Icon).Clone();
                        ((GeometryDrawing)((DrawingGroup)drawing.Drawing).Children[0]).Brush = Brushes.Black;
                        MenuItem mi = new MenuItem { Icon = new Image { Source = drawing, Height = 16, Width = 16 }, Tag = mdi_ };

                        Binding b = new Binding
                        {
                            Source = mdi_,
                            Path = new PropertyPath(MdiChild.TitleProperty),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        BindingOperations.SetBinding(mi, HeaderedItemsControl.HeaderProperty, b);
                        //mi.SetResourceReference(MenuItem.HeaderProperty, mdi_.Title);
                        mi.Click += (o, e) => mdi_.Focus();

                        WindowsChildMenu.Children.Insert(0, mi);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (int l = 0; l < Args.OldItems.Count; l++)
                    {
                        int index = -1;
                        for (int i = 0; i < WindowsChildMenu.Children.Count; i++)
                        {
                            if (((MenuItem)WindowsChildMenu.Children[i]).Tag == Args.OldItems[l])
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index != -1)
                        {
                            WindowsChildMenu.Children.RemoveAt(index);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace: // Hmmm
                    break;
                case NotifyCollectionChangedAction.Move: // Hmmm
                    break;
                default:
                    break;
            }
        }
        private void M_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Tag)
            {
                case "Cascade":
                    Container.MdiLayout = MdiLayout.Cascade;
                    break;
                case "Horizontal":
                    Container.MdiLayout = MdiLayout.TileHorizontal;
                    break;
                case "Vertical":
                    Container.MdiLayout = MdiLayout.TileVertical;
                    break;
                case "Close all":
                    Container.Children.Clear();
                    break;
            }
        }


        #endregion

        #region Content Button Events

        /// <summary>
        /// Handles the Click event of the DisableMinimize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DisableMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window1.MinimizeBox = false;
        }

        /// <summary>
        /// Handles the Click event of the EnableMinimize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EnableMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window1.MinimizeBox = true;
        }

        /// <summary>
        /// Handles the Click event of the DisableMaximize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DisableMaximize_Click(object sender, RoutedEventArgs e)
        {
            Window1.MaximizeBox = false;
        }

        /// <summary>
        /// Handles the Click event of the EnableMaximize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EnableMaximize_Click(object sender, RoutedEventArgs e)
        {
            Window1.MaximizeBox = true;
        }

        /// <summary>
        /// Handles the Click event of the DisableClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void DisableClose_Click(object sender, RoutedEventArgs e)
        {
            Window1.CloseBox = false;
        }

        /// <summary>
        /// Handles the Click event of the EnableClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void EnableClose_Click(object sender, RoutedEventArgs e)
        {
            Window1.CloseBox = true;
        }

        /// <summary>
        /// Handles the Click event of the ShowIcon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ShowIcon_Click(object sender, RoutedEventArgs e)
        {
            Window1.ShowIcon = true;
        }

        /// <summary>
        /// Handles the Click event of the HideIcon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void HideIcon_Click(object sender, RoutedEventArgs e)
        {
            Window1.ShowIcon = false;
        }
        #endregion

        private MetroWindow accentThemeTestWindow;
        private void ChangeAppStyleButtonClick(object sender, RoutedEventArgs e)
        {
            if (accentThemeTestWindow != null)
            {
                accentThemeTestWindow.Activate();
                return;
            }

            accentThemeTestWindow = new AccentStyleWindow();
            accentThemeTestWindow.Owner = this;
            accentThemeTestWindow.Closed += (o, args) => accentThemeTestWindow = null;
            accentThemeTestWindow.Left = Left + ActualWidth / 2.0;
            accentThemeTestWindow.Top = Top + ActualHeight / 2.0;
            accentThemeTestWindow.Show();
        }
        //Activated/Deactivated from Stefano Gottardo
        private void Window_Activated(object sender, RoutedEventArgs e)
        {
            activetxt.Text = Container.ActiveMdiChild.Title;
            Container.ActiveMdiChild.Icon = Extensions.MakeIcon(new PackIconMaterial { Kind = PackIconMaterialKind.Adobe }, Brushes.Wheat);
        }

        private void Window_Deactivated(object sender, RoutedEventArgs e)
        {
            Container.ActiveMdiChild.Icon = Extensions.MakeIcon(new PackIconMaterial { Kind = PackIconMaterialKind.AlertDecagram }, Brushes.Red);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Window1.Icon = Extensions.MakeIcon(new PackIconMaterial { Kind = PackIconMaterialKind.Brush });
        }
    }
}