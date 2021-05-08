using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using Course_Work_v1.BusinessLogic.Models;

namespace Course_Work_v1
{
    /// <summary>
    /// Interaction logic for Page0.xaml
    /// </summary>
    /// 

    public partial class Page0 : Page
    {
        public Page0()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void operation_sum2_Checked(object sender, RoutedEventArgs e)
        {
            Calculations.Operation = Operation.Sum2;
        }

        private void operation_sum_Checked(object sender, RoutedEventArgs e)
        {
            Calculations.Operation = Operation.Sum;
        }

        private void operation_mult_Checked(object sender, RoutedEventArgs e)
        {
            Calculations.Operation = Operation.Mult;
        }

        private void operation_mult2_Checked(object sender, RoutedEventArgs e)
        {
            Calculations.Operation = Operation.Mult2;
        }
    }
}
