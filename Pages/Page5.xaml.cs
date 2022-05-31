using BLL.Contracts;
using Ninject;
using System.Windows;
using System.Windows.Controls;
using BLL.Services;

namespace Course_Work_v1
{
    public partial class Page5 : Page
    {
        private readonly IKernel kernel;
        private readonly IAlgorithmService calculationService;
        
        public Page5(IKernel kernel, IAlgorithmService calculationService)
        {
            this.kernel = kernel;
            this.calculationService = calculationService;
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateHdlButton_Click(object sender, RoutedEventArgs e)
        {
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.MinimalPolyHdl);
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.ShortestPolyHdl);
        }

        private void GenerateCppButton_Click(object sender, RoutedEventArgs e)
        {
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.MinimalPolyCpp);
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.ShortestPolyCpp);
        }

        private void GenerateTbButton_Click(object sender, RoutedEventArgs e)
        {
            calculationService.AlgorithmMain(AlgorithmService.AlgorithmOperation.TestBenchCpp);
        }
    }
}
