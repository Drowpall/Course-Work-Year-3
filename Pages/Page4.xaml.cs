using System;
using BLL.Contracts;
using Ninject;
using System.Windows;
using System.Windows.Controls;
using BLL.Services;

namespace Course_Work_v1
{
    public partial class Page4 : Page
    {
        private readonly IKernel kernel;
        private readonly IAlgorithmService calculationService;
        public Page4(IKernel kernel, IAlgorithmService calculationService)
        {
            this.kernel = kernel;
            this.calculationService = calculationService;
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.ShortestPolynomials);
            }
            catch(OutOfMemoryException)
            {
                MessageBox.Show("Out of RAM. Please select fewer operands or smaller digit capacity.");
                calculationService.CleanAlgorithm();
                NavigationService?.Navigate(kernel.Get<Page0>());
                return;
            }

            NavigationService?.Navigate(kernel.Get<Page5>());
        }
    }
}
