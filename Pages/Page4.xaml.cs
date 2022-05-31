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
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.ShortestPolynomials);
            this.NavigationService.Navigate(kernel.Get<Page5>());
        }
    }
}
