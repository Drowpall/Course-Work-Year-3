using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Models;
using Course_Work_v1;
using DAL.Contracts;
using DAL.Models;
using System.IO;

namespace BLL.Services
{
    public class AlgorithmService : IAlgorithmService
    {
        public AlgorithmService
        (
            IOperationRepository operationRepository, 
            IDigitCapacityRepository digitCapacityRepository, 
            IOperandsNumberRepository operandsNumberRepository, 
            IOperationModuleRepository operationModuleRepository, 
            ITruthTableCalculator truthTableCalculator, 
            IMatricesConstructor matricesConstructor,
            IPolynomialEvaluationService polynomialEvaluationService,
            IDimensionsService dimensionsService, 
            IOutputExtendedService outputExtendedService, 
            IOutputReducedService outputReducedService, 
            IOutputModuleService outputModuleService,
            IOutputMatricesService outputMatricesService,
            IOutputPolynomialsService outputPolynomialsService
        )
        {
            OperationRepository = operationRepository;
            DigitCapacityRepository = digitCapacityRepository;
            OperandsNumberRepository = operandsNumberRepository;
            OperationModuleRepository = operationModuleRepository;
            TruthTableCalculator = truthTableCalculator;
            MatricesConstructor = matricesConstructor;
            PolynomialEvaluationService = polynomialEvaluationService;
            DimensionsService = dimensionsService;
            OutputExtendedService = outputExtendedService;
            OutputReducedService = outputReducedService;
            OutputModuleService = outputModuleService;
            OutputMatricesService = outputMatricesService;
            OutputPolynomialsService = outputPolynomialsService;
        }

        public IOperationRepository OperationRepository { get; set; }
        public IDigitCapacityRepository DigitCapacityRepository { get; set; }
        public IOperandsNumberRepository OperandsNumberRepository { get; set; }
        public IOperationModuleRepository OperationModuleRepository { get; set; }
        public ITruthTableCalculator TruthTableCalculator { get; set; }
        public IMatricesConstructor MatricesConstructor { get; set; }
        public IPolynomialEvaluationService PolynomialEvaluationService { get; set; }
        public IDimensionsService DimensionsService { get; set; }

        public IOutputExtendedService OutputExtendedService { get; set; }
        public IOutputReducedService OutputReducedService { get; set; }
        public IOutputModuleService OutputModuleService { get; set; }

        public IOutputMatricesService OutputMatricesService { get; set; }
        public IOutputPolynomialsService OutputPolynomialsService { get; set; }


        private Operation Operation => OperationRepository.GetOperation();
        private int OperandsNumber => OperandsNumberRepository.GetOperandsNumber();
        private int DigitCapacity => DigitCapacityRepository.GetDigitCapacity();
        private int OperationModule => OperationModuleRepository.GetOperationModule();

        public void DrawTruthTable()
        {
            var userParameters = new UserParameters()
            {
                DigitCapacity = DigitCapacity,
                OperandsNumber = OperandsNumber,
                OperationModule = OperationModule,
                Operation = Operation
            };

            var dimensions = DimensionsService.GetDimensions(userParameters);

            var truthTable = TruthTableCalculator.CalculateTruthTable(dimensions, userParameters);

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTableExtended)))
            {
                OutputExtendedService.OutputExtendedTruthTable(outputFile, truthTable, userParameters, dimensions);
            }

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTableReduced)))
            {
                OutputReducedService.OutputReducedTruthTable(outputFile, truthTable, dimensions);
            }

            if (userParameters.OperationModule != -1)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, Globals.TruthTableModule)))
                {
                    OutputModuleService.OutputModuleTruthTable(outputFile, truthTable, userParameters, dimensions);
                }
            }
        }

        public void CalculatePolynomials()
        {
            var userParameters = new UserParameters()
            {
                DigitCapacity = DigitCapacity,
                OperandsNumber = OperandsNumber,
                OperationModule = OperationModule,
                Operation = Operation
            };

            var dimensions = DimensionsService.GetDimensions(userParameters);

            var truthTable = TruthTableCalculator.CalculateTruthTable(dimensions, userParameters);

            var matrices = MatricesConstructor.CalculateMatrices(truthTable, dimensions, userParameters);

            PolynomialEvaluationService.EvaluatePolynomial(truthTable, matrices);

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.Matrices)))
            {
                OutputMatricesService.OutputMatrices(outputFile, matrices);
            }

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.ShortestPolynomials)))
            {
                OutputPolynomialsService.OutputShortestPolynomials(outputFile, matrices);
                OutputPolynomialsService.OutputShortestPolynomialsText(outputFile, matrices, userParameters, dimensions);
            }

        }
    }
}
