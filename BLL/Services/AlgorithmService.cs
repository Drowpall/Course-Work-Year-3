using System;
using System.Collections.Generic;
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
        private IEnumerable<bool[]> ResultCols { get; set; }
        private bool IsComplexGenerated { get; set; }

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

            if (ResultCols == null)
            {
                ResultCols = TruthTableCalculator.CalculateTruthTableComplex(Dimensions, userParameters);
            }

            #region ComplexPolynomial
            
            if (userParameters.DigitCapacity * userParameters.OperandsNumber >= 10 && !IsComplexGenerated)
            {
                using (var outputFile = File.CreateText(Globals.PolynomialsComplex))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsText(outputFile, ResultCols, userParameters, Dimensions);
                    Thread.Sleep(500);
                    Process.Start(Globals.PolynomialsComplex);
                }
                
                using (var outputFile = File.CreateText(Globals.PolynomialsComplexHdl))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsHdl(outputFile, ResultCols, userParameters, Dimensions);
                    Thread.Sleep(500);
                    Process.Start(Globals.PolynomialsComplexHdl);
                }
                
                using (var outputFile = File.CreateText(Globals.PolynomialsComplexCpp))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsC(outputFile, ResultCols, userParameters, Dimensions);
                    Thread.Sleep(500);
                    Process.Start(Globals.PolynomialsComplexCpp);
                }

                IsComplexGenerated = true;
                
                return;
            }
            
            #endregion ComplexPolynomial
            
            
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
                    
                    using (var outputFile = File.CreateText(Globals.PolynomialsComplex))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsText(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.PolynomialsComplex);
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

                    using (var outputFile = File.CreateText(Globals.PolynomialsComplexHdl))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsHdl(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.PolynomialsComplexHdl);
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
                    
                    using (var outputFile = File.CreateText(Globals.PolynomialsComplexCpp))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsC(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        Process.Start(Globals.PolynomialsComplexCpp);
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
                    return;
                }
            }
        }

        public void CleanAlgorithm()
        {
            TruthTable = null;
            Dimensions = null;
            Matrices = null;
        }
    }
}
