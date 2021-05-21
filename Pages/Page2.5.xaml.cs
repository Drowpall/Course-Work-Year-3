using System;
using System.Windows;
using System.Windows.Controls;
using DAL.Contracts;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page2_5 : Page
    {
        private readonly IKernel kernel;
        private readonly IOperationModuleRepository operationModuleRepository;
        public Page2_5(IKernel kernel, IOperationModuleRepository operationModuleRepository)
        {
            this.kernel = kernel;
            this.operationModuleRepository = operationModuleRepository;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(operationModule.Text, out int opModule) && opModule > 1)
            {
                operationModuleRepository.SetOperationModule(opModule);
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