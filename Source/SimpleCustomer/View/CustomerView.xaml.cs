using SimpleCustomer.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCustomer.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
        }

        // Populate the comboboxes with enum values
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboBoxCategory.ItemsSource = Enum.GetValues(typeof(Category)).Cast<Category>();
            this.comboBoxGender.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
        }
        
    }
}
