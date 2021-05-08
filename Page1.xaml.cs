using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Course_Work_v1.BusinessLogic;
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
            if (Int32.TryParse(numberOfOperands.Text, out int operandsNum) && operandsNum > 0)
            {
                operandsNumberRepository.SetOperandsNumber(operandsNum);
                this.NavigationService.Navigate(kernel.Get<Page2>());
            }
            else
            {
                MessageBox.Show($"Invalid input. Given input: {numberOfOperands.Text}. Please specify an integer instead.");
                this.NavigationService.Navigate(kernel.Get<Page1>());
            }
        }
    }
}
