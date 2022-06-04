using System.Windows;
using System.Windows.Controls;
using DAL.Contracts;
using DAL.Models;
using Ninject;

namespace Course_Work_v1
{

    public partial class Page0 : Page
    {
        private readonly IKernel kernel;
        private readonly IOperationRepository operationRepository;
        private readonly IFileModeRepository fileModeRepository;
        private bool operationChecked;

        public Page0(IKernel kernel)
        {
            this.kernel = kernel;
            operationRepository = kernel.Get<IOperationRepository>();
            fileModeRepository = kernel.Get<IFileModeRepository>();
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!operationChecked)
            {
                MessageBox.Show($"Please, select an operation first.");
            }
            else
            {
                NavigationService?.Navigate(kernel.Get<Page1>());
            }
        }

        private void operation_sum2_Checked(object sender, RoutedEventArgs e)
        {
            operationChecked = true;
            operationRepository.SetOperation(Operation.Sum2);
        }

        private void operation_sum_Checked(object sender, RoutedEventArgs e)
        {
            operationChecked = true;
            operationRepository.SetOperation(Operation.Sum);
        }

        private void operation_mult_Checked(object sender, RoutedEventArgs e)
        {
            operationChecked = true;
            operationRepository.SetOperation(Operation.Mult);
        }

        private void operation_mult2_Checked(object sender, RoutedEventArgs e)
        {
            operationChecked = true;
            operationRepository.SetOperation(Operation.Mult2);
        }

        #region Checked

        private void truthtable_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.TruthTableMode = true;
        } 

        private void minimaltxt_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalTxtMode = true;
        } 

        private void complextxt_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexTxtMode = true;
        } 

        private void minimalv_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalVMode = true;
        } 

        private void shortestv_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ShortestVMode = true;
        } 

        private void complexv_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexVMode = true;
        } 

        private void minimalcpp_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalCppMode = true;
        } 

        private void shortestcpp_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ShortestCppMode = true;
        } 

        private void complexcpp_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexCppMode = true;
        } 

        private void testbench_checked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.TestBenchMode = true;
        }    

        #endregion Checked

        #region UnChecked

        private void truthtable_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.TruthTableMode = false;
        } 

        private void minimaltxt_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalTxtMode = false;
        } 

        private void complextxt_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexTxtMode = false;
        } 

        private void minimalv_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalVMode = false;
        } 

        private void shortestv_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ShortestVMode = false;
        } 

        private void complexv_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexVMode = false;
        } 

        private void minimalcpp_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.MinimalCppMode = false;
        } 

        private void shortestcpp_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ShortestCppMode = false;
        } 

        private void complexcpp_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.ComplexCppMode = false;
        } 

        private void testbench_unchecked(object sender, RoutedEventArgs e)
        {
            fileModeRepository.TestBenchMode = false;
        }       

        #endregion UnChecked
    }
}
