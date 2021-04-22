using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Course_Work_v1
{
    /// <summary>
    /// Interaction logic for Page0.xaml
    /// </summary>
    /// 
    public enum Operation
    {
        Sum,
        Sum2,
        Mult,
        Mult2
    }
    public partial class Page0 : Page
    {
        private Operation operation;
        public Page0()
        {
            InitializeComponent();
        }

        public Operation getOperation()
        {
            return operation;
        }

        private void operation_sum_Checked(object sender, RoutedEventArgs e)
        {
            operation = Operation.Sum;
        }

        private void operation_mult_Checked(object sender, RoutedEventArgs e)
        {
            operation = Operation.Mult;
        }

        private void operation_sum2_Checked(object sender, RoutedEventArgs e)
        {
            operation = Operation.Sum2;
        }

        private void operation_mult2_Checked(object sender, RoutedEventArgs e)
        {
            operation = Operation.Mult2;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }
    }
}
