using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Models;
using Course_Work_v1;
using DAL.Contracts;
using DAL.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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

           // using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTableExtended)))
            using (StreamWriter outputFile = File.CreateText(Globals.TruthTableExtended))
            {
                OutputExtendedService.OutputExtendedTruthTable(outputFile, truthTable, userParameters, dimensions);
                //Thread.Sleep(500);
                //Process.Start(Globals.TruthTableExtended);
            }

            //using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTableReduced)))
            using (StreamWriter outputFile = File.CreateText(Globals.TruthTableReduced))
            {
                OutputReducedService.OutputReducedTruthTable(outputFile, truthTable, dimensions);
                //Thread.Sleep(500);
                //Process.Start(Globals.TruthTableReduced);
            }

            if (userParameters.OperationModule != -1)
            {
                //using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, Globals.TruthTableModule)))
                using (StreamWriter outputFile = new StreamWriter(Globals.TruthTableModule))
                {
                    OutputModuleService.OutputModuleTruthTable(outputFile, truthTable, userParameters, dimensions);
                    //Thread.Sleep(500);
                    //Process.Start(Globals.TruthTableModule);
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

            PolynomialEvaluationService.EvaluatePolynomialShortest(truthTable, matrices);
            PolynomialEvaluationService.EvaluatePolynomialMinimal(truthTable, matrices, userParameters);

            //using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.Matrices)))
            using (StreamWriter outputFile = File.CreateText(Globals.Matrices))
            {
                OutputMatricesService.OutputMatrices(outputFile, matrices);
                //Thread.Sleep(500);
                //Process.Start(Globals.Matrices);
            }

            //using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.ShortestPolynomials)))
            using (StreamWriter outputFile = File.CreateText(Globals.ShortestPolynomials))
            {
               // OutputPolynomialsService.OutputShortestPolynomialsVectors(outputFile, matrices);
                OutputPolynomialsService.OutputShortestPolynomialsText(outputFile, matrices, userParameters, dimensions);
                // OutputPolynomialsService.OutputMinimalPolynomialsVectors(outputFile, matrices);
                outputFile.WriteLine();
                OutputPolynomialsService.OutputMinimalPolynomialsText(outputFile, matrices, userParameters, dimensions);
                //Thread.Sleep(500);
                //Process.Start(Globals.ShortestPolynomials);
            }

        }
    }
}
