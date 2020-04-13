using System.Windows;
using System.Windows.Controls;
using WPF.MDI;

namespace Example.Controls
{
    /// <summary>
    /// Interaction logic for ExampleControl.xaml
    /// </summary>
    public partial class ExampleControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleControl"/> class.
        /// </summary>
        MdiChild MDI;
        public ExampleControl(MdiChild _mdi)
        {
            InitializeComponent();
            MDI = _mdi;
            Width = double.NaN;
            Height = double.NaN;
            MDI.Closing += ExampleControl_Unloaded;
        }

        private void ExampleControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ((ClosingEventArgs)e).Cancel = true; // <- works in my project, fails here... 
            if (MessageBox.Show("Close?", "Close", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MDI.Close();
            }
        }
    }
}
