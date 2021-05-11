using System.Windows;
using System.Windows.Controls;
using Course_Work_v1.BusinessLogic;
using DAL.Models;
using Ninject;

namespace Course_Work_v1
{
    public partial class Page3 : Page
    {
        private readonly IKernel kernel;
        public Page3(IKernel kernel)
        {
            this.kernel = kernel;
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Calculations.DrawTruthTable();
            this.NavigationService.Navigate(kernel.Get<Page4>());
        }

    }
}
