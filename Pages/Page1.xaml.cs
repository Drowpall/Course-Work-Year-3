using System;
using System.Windows;
using System.Windows.Controls;
using DAL.Contracts;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page1 : Page
    {
        private readonly IKernel kernel;
        private readonly IOperandsNumberRepository operandsNumberRepository;
        public Page1(IKernel kernel, IOperandsNumberRepository operandsNumberRepository)
        {
            this.kernel = kernel;
            this.operandsNumberRepository = operandsNumberRepository;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numberOfOperands.Text, out var operandsNum) && operandsNum > 0)
            {
                operandsNumberRepository.SetOperandsNumber(operandsNum);
                NavigationService?.Navigate(kernel.Get<Page2>());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {numberOfOperands.Text}. Please specify an integer instead.");
                NavigationService?.Navigate(kernel.Get<Page1>());
            }
        }
    }
}
