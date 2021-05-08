using System;
using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using Course_Work_v1.BusinessLogic.Models;

namespace Course_Work_v1
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(lengthOfOperands.Text, out int _lengthOfOperands) && _lengthOfOperands > 0)
            {
                Calculations.DigitCapacity = _lengthOfOperands;
                if(Calculations.Operation == Operation.Sum2 || Calculations.Operation == Operation.Mult2)
                {
                    this.NavigationService.Navigate(new Page2_5());
                }
                else
                {
                    this.NavigationService.Navigate(new Page3());
                }
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {lengthOfOperands.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(new Page2());
            }
        }
    }
}
