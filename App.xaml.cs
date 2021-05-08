using Course_Work_v1.BusinessLogic;
using DAL.Contracts;
using DAL.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Course_Work_v1
{
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            container.Bind<IOperationRepository>().To<OperationRepository>().InSingletonScope();
            container.Bind<Page0>().ToSelf().InTransientScope();
            container.Bind<Page1>().ToSelf().InTransientScope();
            container.Bind<Page2>().ToSelf().InTransientScope();
            container.Bind<Page2_5>().ToSelf().InTransientScope();
            container.Bind<Page3>().ToSelf().InTransientScope();
            container.Bind<Page4>().ToSelf().InTransientScope();
            container.Bind<Page5>().ToSelf().InTransientScope();
        }

        private void ComposeObjects()
        {
            Calculations.OperationRepository = container.Get<IOperationRepository>();
            Current.MainWindow = this.container.Get<MainWindow>();
        }
    }
}
