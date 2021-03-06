using System.Windows;
using System.Windows.Controls;
using BLL.Contracts;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page3 : Page
    {
        private readonly IKernel kernel;
        private readonly IAlgorithmService calculationService;


        public Page3(IKernel kernel, IAlgorithmService calculationService)
        {
            this.kernel = kernel;
            this.calculationService = calculationService;
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            calculationService.DrawTruthTable();
            this.NavigationService.Navigate(kernel.Get<Page4>());
        }

    }
}
