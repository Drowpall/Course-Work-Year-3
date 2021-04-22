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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private int _lengthOfOperands;
        public Page2()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(lengthOfOperands.Text, out _lengthOfOperands) && _lengthOfOperands > 0)
            {
                if(_lengthOfOperands > 0)
                    this.NavigationService.Navigate(new Page3());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {lengthOfOperands.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(new Page2());
            }

            //MessageBox.Show($"Something went wrong... Given input: {lengthOfOperands.Text}. Please specify an accurate integer instead.");
            //this.NavigationService.Navigate(new Page1());
        }
    }
}
