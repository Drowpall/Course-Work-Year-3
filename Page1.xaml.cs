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
using Course_Work_v1.BusinessLogic;

namespace Course_Work_v1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(numberOfOperands.Text, out int _numberOfOperands) && _numberOfOperands > 0)
            {
                Calculations.OperandsNumber = _numberOfOperands;
                this.NavigationService.Navigate(new Page2());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {numberOfOperands.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(new Page1());
            }
        }
    }
}
