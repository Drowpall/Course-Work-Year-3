using System;
using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using DAL.Contracts;
using DAL.Models;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page2 : Page
    {
        private readonly IOperationRepository operationRepository;
        private readonly IKernel kernel;
        public Page2(IKernel kernel)
        {
            this.kernel = kernel;
            this.operationRepository = kernel.Get<IOperationRepository>();
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(lengthOfOperands.Text, out int _lengthOfOperands) && _lengthOfOperands > 0)
            {
                var operation = operationRepository.GetOperation();
                Calculations.DigitCapacity = _lengthOfOperands;
                if(operation == Operation.Sum2 || operation == Operation.Mult2)
                {
                    this.NavigationService.Navigate(kernel.Get<Page2_5>());
                }
                else
                {
                    this.NavigationService.Navigate(kernel.Get<Page3>());
                }
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {lengthOfOperands.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(kernel.Get<Page2>());
            }
        }
    }
}
