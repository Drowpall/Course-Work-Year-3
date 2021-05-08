using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using Course_Work_v1.BusinessLogic.Models;

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
            if(Calculations.Operation == Operation.Sum2 || Calculations.Operation == Operation.Mult2)
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
            MessageBox.Show($"Data test: Operation: {Calculations.Operation}. \n" +
            $"Number of operands: {Calculations.OperandsNumber}. \n" +
            $"Digit capacity: {Calculations.DigitCapacity} \n");
        }

        private void DisplayParametersMod()
        {
            MessageBox.Show($"Data test: Operation: {Calculations.Operation}. \n" +
            $"Number of operands: {Calculations.OperandsNumber}. \n" +
            $"Digit capacity: {Calculations.DigitCapacity} \n" +
            $"Operation module: {Calculations.OperationModule} \n");
        }

    }
}
