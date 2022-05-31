using System;
using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Models;
using DAL.Contracts;
using DAL.Models;
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

        private Dimensions Dimensions { get; set; }
        private TruthTable TruthTable { get; set; }
        private Matrices Matrices { get; set; }

        public enum AlgorithmOperation
        {
            ExtendedTruthTable,
            ReducedTruthTable,
            ModuleTruthTable,
            ShortestPolynomials,
            MinimalPolyHdl,
            ShortestPolyHdl,            
            MinimalPolyCpp,
            ShortestPolyCpp,
            TestBenchCpp
        }
        public void AlgorithmMain(AlgorithmOperation selectOutputFormat)
        {
            var userParameters = new UserParameters()
            {
                DigitCapacity = DigitCapacity,
                OperandsNumber = OperandsNumber,
                OperationModule = OperationModule,
                Operation = Operation
            };

            if (Dimensions == null)
            {
                Dimensions = DimensionsService.GetDimensions(userParameters);
            }

            if (TruthTable == null)
            {
                TruthTable = TruthTableCalculator.CalculateTruthTable(Dimensions, userParameters);
            }

            if (Matrices == null)
            {
                Matrices = MatricesConstructor.CalculateMatrices(TruthTable, Dimensions, userParameters);
                PolynomialEvaluationService.EvaluatePolynomialShortest(TruthTable, Matrices);
                PolynomialEvaluationService.EvaluatePolynomialMinimal(Matrices, userParameters);
            }
            
            switch (selectOutputFormat)
            {
                case AlgorithmOperation.ExtendedTruthTable:
                {
                    using (var outputFile = File.CreateText(Globals.TruthTableExtended))
                    {
                        OutputExtendedService.OutputExtendedTruthTable(outputFile, TruthTable, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.TruthTableExtended);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ReducedTruthTable:
                {
                    using (var outputFile = File.CreateText(Globals.TruthTableReduced))
                    {
                        OutputReducedService.OutputReducedTruthTable(outputFile, TruthTable, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.TruthTableReduced);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ModuleTruthTable:
                {
                    if (userParameters.OperationModule == -1) return;
            
                    {
                        using (var outputFile = new StreamWriter(Globals.TruthTableModule))
                        {
                            OutputModuleService.OutputModuleTruthTable(outputFile, TruthTable, userParameters, Dimensions);
                            Thread.Sleep(500);
                            Process.Start(Globals.TruthTableModule);
                        }
                    }

                    break;
                }
                case AlgorithmOperation.ShortestPolynomials:
                {
                    using (var outputFile = File.CreateText(Globals.ShortestPolynomials))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                        outputFile.WriteLine();
                        OutputPolynomialsService.OutputMinimalPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.ShortestPolynomials);
                    }

                    break;
                }
                case AlgorithmOperation.MinimalPolyHdl:
                {
                    using (var outputFile = File.CreateText(Globals.MinimalPolynomialsHdl))
                    {
                        OutputPolynomialsService.OutputMinimalPolynomialsHdl(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.MinimalPolynomialsHdl);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ShortestPolyHdl:
                {
                    using (var outputFile = File.CreateText(Globals.ShortestPolynomialsHdl))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsHdl(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.ShortestPolynomialsHdl);
                    }
                    
                    break;
                }
                case AlgorithmOperation.MinimalPolyCpp:
                {
                    using (var outputFile = File.CreateText(Globals.MinimalPolynomialsCpp))
                    {
                        OutputPolynomialsService.OutputMinimalPolynomialsC(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.MinimalPolynomialsCpp);
                    }
                    
                    break;
                }                
                case AlgorithmOperation.ShortestPolyCpp:
                {
                    using (var outputFile = File.CreateText(Globals.ShortestPolynomialsCpp))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsC(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.ShortestPolynomialsCpp);
                    }
                    
                    break;
                }
                case AlgorithmOperation.TestBenchCpp:
                {
                    using (var outputFile = File.CreateText(Globals.TestBenchCpp))
                    {
                        OutputPolynomialsService.OutputTestBench(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.TestBenchCpp);
                    }
                    
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(selectOutputFormat), selectOutputFormat, null);
                }
            }
        }
        
        
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

            /*if (userParameters.OperandsNumber * userParameters.DigitCapacity >= 12)
            {
                using (var outputFile = File.CreateText(Globals.DNFpresentation))
                {
                    OutputExtendedService.OutputDNF(outputFile, truthTable, userParameters);
                }
                
                Thread.Sleep(500);
                Process.Start(Globals.DNFpresentation);

                return;
            }*/
            
            using (var outputFile = File.CreateText(Globals.TruthTableExtended))
            {
                OutputExtendedService.OutputExtendedTruthTable(outputFile, truthTable, userParameters, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.TruthTableExtended);
            }
            
            using (var outputFile = File.CreateText(Globals.TruthTableReduced))
            {
                OutputReducedService.OutputReducedTruthTable(outputFile, truthTable, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.TruthTableReduced);
            }

            if (userParameters.OperationModule == -1) return;
            
            {
                using (var outputFile = new StreamWriter(Globals.TruthTableModule))
                {
                    OutputModuleService.OutputModuleTruthTable(outputFile, truthTable, userParameters, dimensions);
                    Thread.Sleep(500);
                    Process.Start(Globals.TruthTableModule);
                }
            }
        }

        

        public void CalculatePolynomials(string selectFileFormat)
        {
            var userParameters = new UserParameters()
            {
                DigitCapacity = DigitCapacity,
                OperandsNumber = OperandsNumber,
                OperationModule = OperationModule,
                Operation = Operation
            };

            if (Dimensions == null)
            {
                Dimensions = DimensionsService.GetDimensions(userParameters);
            }

            if (TruthTable == null)
            {
                TruthTable = TruthTableCalculator.CalculateTruthTable(Dimensions, userParameters);
            }

            if (Matrices == null)
            {
                Matrices = MatricesConstructor.CalculateMatrices(TruthTable, Dimensions, userParameters);
            }
            

            PolynomialEvaluationService.EvaluatePolynomialMinimal(Matrices, userParameters);

            
            

            using (var outputFile = File.CreateText(Globals.Matrices))
            {
                OutputMatricesService.OutputMatrices(outputFile, Matrices);
                Thread.Sleep(500);
                Process.Start(Globals.Matrices);
            }

            //using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.ShortestPolynomials)))
            using (var outputFile = File.CreateText(Globals.ShortestPolynomials))
            {
               // OutputPolynomialsService.OutputShortestPolynomialsVectors(outputFile, matrices);
                OutputPolynomialsService.OutputShortestPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                // OutputPolynomialsService.OutputMinimalPolynomialsVectors(outputFile, matrices);
                outputFile.WriteLine();
                OutputPolynomialsService.OutputMinimalPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.ShortestPolynomials);
            }
        }

        public void GenerateHdl(int selectFileFormat)
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
            PolynomialEvaluationService.EvaluatePolynomialMinimal(matrices, userParameters);

            using (var outputFile = File.CreateText(Globals.MinimalPolynomialsHdl))
            {
                OutputPolynomialsService.OutputMinimalPolynomialsHdl(outputFile, matrices, userParameters, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.MinimalPolynomialsHdl);
            }
            
            using (var outputFile = File.CreateText(Globals.ShortestPolynomialsHdl))
            {
                OutputPolynomialsService.OutputShortestPolynomialsHdl(outputFile, matrices, userParameters, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.ShortestPolynomialsHdl);
            }
            
            using (var outputFile = File.CreateText(Globals.MinimalPolynomialsCpp))
            {
                OutputPolynomialsService.OutputMinimalPolynomialsC(outputFile, matrices, userParameters, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.MinimalPolynomialsCpp);
            }
            
            using (var outputFile = File.CreateText(Globals.ShortestPolynomialsCpp))
            {
                OutputPolynomialsService.OutputShortestPolynomialsC(outputFile, matrices, userParameters, dimensions);
                Thread.Sleep(500);
                Process.Start(Globals.ShortestPolynomialsCpp);
            }
        }
    }
}
