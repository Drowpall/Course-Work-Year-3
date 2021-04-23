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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Data test: Operation: {Calculations.GetOperation_toString()}. \n" +
                $"Number of operands: {Calculations.GetOperandsNum_toString()}. \n" +
                $"Digit capacity: {Calculations.GetDigitCapacity_toString()}");
            Calculations.DrawTruthTable();
            this.NavigationService.Navigate(new Page4());
        }
    }
}
