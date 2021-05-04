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
            if(Calculations.GetOperation() == Operation.Sum2 || Calculations.GetOperation() == Operation.Mult2)
            {
                DisplayParametersMod();
            }
            else
            {
                DisplayParameters();
            }

            Calculations.DrawTruthTable();
            this.NavigationService.Navigate(new Page4());
        }

        private void DisplayParameters()
        {
            MessageBox.Show($"Data test: Operation: {Calculations.Operation_toString()}. \n" +
            $"Number of operands: {Calculations.OperandsNum_toString()}. \n" +
            $"Digit capacity: {Calculations.DigitCapacity_toString()} \n");
        }

        private void DisplayParametersMod()
        {
            MessageBox.Show($"Data test: Operation: {Calculations.Operation_toString()}. \n" +
            $"Number of operands: {Calculations.OperandsNum_toString()}. \n" +
            $"Digit capacity: {Calculations.DigitCapacity_toString()} \n" +
            $"Operation module: {Calculations.OperationModule_toString()} \n");
        }

    }
}
