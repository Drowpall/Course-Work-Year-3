using System;
using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;

namespace Course_Work_v1
{
    /// <summary>
    /// Interaction logic for Page5.xaml
    /// </summary>
    public partial class Page2_5 : Page
    {
        public Page2_5()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(operationModule.Text, out int _operationModule) && _operationModule > 1)
            {
                Calculations.OperationModule = _operationModule;
                this.NavigationService.Navigate(new Page3());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {operationModule.Text}. Please specify an integer (>1) instead.");
                this.NavigationService.Navigate(new Page2_5());
            }
        }
    }
}