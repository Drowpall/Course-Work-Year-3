using System;
using System.Windows;
using System.Windows.Controls;
using DAL.Contracts;
using DAL.Models;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page2 : Page
    {
        private readonly IKernel kernel;

        private readonly IOperationRepository operationRepository;

        private readonly IDigitCapacityRepository digitCapacityRepository;
        
        public Page2(IKernel kernel, IOperationRepository operationRepository, IDigitCapacityRepository digitCapacityRepository)
        {
            this.kernel = kernel;
            this.operationRepository = operationRepository;
            this.digitCapacityRepository = digitCapacityRepository;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(digitCapacity.Text, out int digitCap) && digitCap > 0)
            {
                var operation = operationRepository.GetOperation();
                digitCapacityRepository.SetDigitCapacity(digitCap);

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
                MessageBox.Show($"Invalid input. Given input: {digitCapacity.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(kernel.Get<Page2>());
            }
        }
    }
}
