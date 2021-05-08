using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using DAL.Contracts;
using DAL.Models;
using Ninject;

namespace Course_Work_v1
{

    public partial class Page0 : Page
    {
        private readonly IKernel kernel;
        private readonly IOperationRepository operationRepository;

        public Page0(IKernel kernel)
        {
            this.kernel = kernel;
            operationRepository = kernel.Get<IOperationRepository>();
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(kernel.Get<Page1>());
        }

        private void operation_sum2_Checked(object sender, RoutedEventArgs e)
        {
            operationRepository.SetOperation(Operation.Sum2);
        }

        private void operation_sum_Checked(object sender, RoutedEventArgs e)
        {
            operationRepository.SetOperation(Operation.Sum);
        }

        private void operation_mult_Checked(object sender, RoutedEventArgs e)
        {
            operationRepository.SetOperation(Operation.Mult);
        }

        private void operation_mult2_Checked(object sender, RoutedEventArgs e)
        {
            operationRepository.SetOperation(Operation.Mult2);
        }
    }
}
