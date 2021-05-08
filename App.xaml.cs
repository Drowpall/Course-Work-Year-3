using BLL.Contracts;
using BLL.Services;
using BLL.Services.CalculateOperationResults;
using Course_Work_v1.BusinessLogic;
using DAL.Contracts;
using DAL.Services;
using Ninject;
using System.Windows;

namespace Course_Work_v1
{
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.container = new StandardKernel();
            ConfigureDAL();
            ConfigureBLL();
            ConfigurePresentation();

            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureDAL()
        {
            container.Bind<IOperationRepository, IDigitCapacityRepository, IOperandsNumberRepository, IOperationModuleRepository>().To<Repository>().InSingletonScope();
        }

        private void ConfigureBLL()
        {
            container.Bind<IDimensionsService>().To<DimensionsService>().InSingletonScope();
            container.Bind<ITruthTableCalculator>().To<TruthTableCalculator>().InSingletonScope();
            container.Bind<IOperationResultsCalculator>().To<CalculateMultOperationResult>().InSingletonScope();
            container.Bind<IOperationResultsCalculator>().To<CalculateMult2OperationResult>().InSingletonScope();
            container.Bind<IOperationResultsCalculator>().To<CalculateSumOperationResult>().InSingletonScope();
            container.Bind<IOperationResultsCalculator>().To<CalculateSum2OperationResuls>().InSingletonScope();
        }

        private void ConfigurePresentation()
        {
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
            Calculations.DigitCapacityRepository = container.Get<IDigitCapacityRepository>();
            Calculations.OperandsNumberRepository = container.Get<IOperandsNumberRepository>();
            Calculations.OperationModuleRepository = container.Get<IOperationModuleRepository>();
            Calculations.DimensionsService = container.Get<IDimensionsService>();
            Calculations.TruthTableCalculator = container.Get<ITruthTableCalculator>();

            Current.MainWindow = this.container.Get<MainWindow>();
        }
    }
}
