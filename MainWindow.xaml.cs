using DAL.Contracts;
using Ninject;
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

namespace Course_Work_v1
{
    public partial class MainWindow : Window
    {
        private readonly IKernel kernel;

        public MainWindow(IKernel kernel)
        {
            this.kernel = kernel;
            InitializeComponent();
            Main.NavigationService.Navigate(kernel.Get<Page0>());
        }

        private void MyWindow_Loaded(object sender, RoutedEvent e)
        {
            Main.NavigationService.Navigate(kernel.Get<Page0>());
        }
    }
}
