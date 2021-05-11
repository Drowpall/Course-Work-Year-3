using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Services;
using BLL.Services.CalculateOperationResults;
using BLL.Services.FileOutput;
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
            container.Bind<ITruthTableRepository>().To<TruthTableRepository>().InSingletonScope();
        }

        private void ConfigureBLL()
        {
            container.Bind<IAlgorithmService>().To<AlgorithmService>().InSingletonScope();
            container.Bind<IPolynomialEvaluationService>().To<PolynomialEvaluationService>().InSingletonScope();

            container.Bind<IDimensionsService>().To<DimensionsService>().InSingletonScope();
            container.Bind<ITruthTableCalculator>().To<TruthTableCalculator>().InSingletonScope();
            container.Bind<IMatricesConstructor>().To<MatricesConctructor>().InSingletonScope();


            container.Bind<IOutputExtendedService>().To<FileOutputExtendedService>().InSingletonScope();
            container.Bind<IOutputReducedService>().To<FileOutputReducedService>().InSingletonScope();
            container.Bind<IOutputModuleService>().To<FileOutputModuleService>().InSingletonScope();

            container.Bind<IOutputMatricesService>().To<FileOutputMatricesService>().InSingletonScope();
            container.Bind<IOutputPolynomialsService>().To<FileOutputPolynomialsService>().InSingletonScope();


           

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
            AlgorithmService calculationService = new AlgorithmService(container.Get<IOperationRepository>(), 
                                                                           container.Get<IDigitCapacityRepository>(),
                                                                           container.Get<IOperandsNumberRepository>(),
                                                                           container.Get<IOperationModuleRepository>(),
                                                                           container.Get<ITruthTableCalculator>(),
                                                                           container.Get<IMatricesConstructor>(),
                                                                           container.Get<IPolynomialEvaluationService>(),
                                                                           container.Get<IDimensionsService>(),
                                                                           container.Get<IOutputExtendedService>(),
                                                                           container.Get<IOutputReducedService>(),
                                                                           container.Get<IOutputModuleService>(),
                                                                           container.Get<IOutputMatricesService>(),
                                                                           container.Get<IOutputPolynomialsService>()
                                                                           );

            Current.MainWindow = this.container.Get<MainWindow>();
        }
    }
}
