using SimpleCustomer.ViewModel;
using System;
using System.Linq;
using System.Windows;
namespace SimpleCustomer.View
{
    /// <summary>
    /// Interaction logic for StatsView.xaml
    /// </summary>
    public partial class StatsView : Window
    {
        public StatsView()
        {
            InitializeComponent();
        }

        // Populate the combobox with enum values
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboStatsType.ItemsSource = Enum.GetValues(typeof(StatsType)).Cast<StatsType>();
        }
        
    }
}
