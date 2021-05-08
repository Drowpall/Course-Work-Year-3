using System;
using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page2_5 : Page
    {
        private readonly IKernel kernel;
        public Page2_5(IKernel kernel)
        {
            this.kernel = kernel;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(operationModule.Text, out int _operationModule) && _operationModule > 1)
            {
                Calculations.OperationModule = _operationModule;
                this.NavigationService.Navigate(kernel.Get<Page3>());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {operationModule.Text}. Please specify an integer (>1) instead.");
                this.NavigationService.Navigate(kernel.Get<Page2_5>());
            }
        }
    }
}