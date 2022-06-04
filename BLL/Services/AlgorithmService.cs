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
            IFileModeRepository fileModeRepository,
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
            FileModeRepository = fileModeRepository;
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
        public IFileModeRepository FileModeRepository { get; set; }
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
            
            if (userParameters.DigitCapacity * userParameters.OperandsNumber >= 10)
            {
                if (IsComplexGenerated) return;
                
                using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplex))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsText(outputFile, ResultCols, userParameters,
                        Dimensions);
                    Thread.Sleep(500);
                    if (FileModeRepository.ComplexTxtMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplex);
                }

                using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplexHdl))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsHdl(outputFile, ResultCols, userParameters, Dimensions);
                    Thread.Sleep(500);
                    if (FileModeRepository.ComplexVMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplexHdl);
                }
                
                using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplexCpp))
                {
                    OutputPolynomialsService.OutputComplexPolynomialsC(outputFile, ResultCols, userParameters, Dimensions);
                    Thread.Sleep(500);
                    if (FileModeRepository.ComplexCppMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplexCpp);
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
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.TruthTableExtended))
                    {
                        OutputExtendedService.OutputExtendedTruthTable(outputFile, TruthTable, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.TruthTableMode) Process.Start(Globals.DocPath + Globals.TruthTableExtended);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ReducedTruthTable:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.TruthTableReduced))
                    {
                        OutputReducedService.OutputReducedTruthTable(outputFile, TruthTable, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.TruthTableMode) Process.Start(Globals.DocPath + Globals.TruthTableReduced);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ModuleTruthTable:
                {
                    if (userParameters.OperationModule == -1) return;
            
                    {
                        using (var outputFile = new StreamWriter(Globals.DocPath + Globals.TruthTableModule))
                        {
                            OutputModuleService.OutputModuleTruthTable(outputFile, TruthTable, userParameters, Dimensions);
                            Thread.Sleep(500);
                            if (FileModeRepository.TruthTableMode) Process.Start(Globals.DocPath + Globals.TruthTableModule);
                        }
                    }

                    break;
                }
                case AlgorithmOperation.ShortestPolynomials:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.ShortestPolynomials))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                        outputFile.WriteLine();
                        OutputPolynomialsService.OutputMinimalPolynomialsText(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.MinimalTxtMode) Process.Start(Globals.DocPath + Globals.ShortestPolynomials);
                    }
                    
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplex))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsText(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.ComplexTxtMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplex);
                    }

                    break;
                }
                case AlgorithmOperation.MinimalPolyHdl:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.MinimalPolynomialsHdl))
                    {
                        OutputPolynomialsService.OutputMinimalPolynomialsHdl(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.MinimalVMode) Process.Start(Globals.DocPath + Globals.MinimalPolynomialsHdl);
                    }

                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplexHdl))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsHdl(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.ComplexVMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplexHdl);
                    }
                    
                    break;
                }
                case AlgorithmOperation.ShortestPolyHdl:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.ShortestPolynomialsHdl))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsHdl(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.ShortestVMode) Process.Start(Globals.DocPath + Globals.ShortestPolynomialsHdl);
                    }
                    
                    break;
                }
                case AlgorithmOperation.MinimalPolyCpp:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.MinimalPolynomialsCpp))
                    {
                        OutputPolynomialsService.OutputMinimalPolynomialsC(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.MinimalCppMode) Process.Start(Globals.DocPath + Globals.MinimalPolynomialsCpp);
                    }
                    
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.PolynomialsComplexCpp))
                    {
                        OutputPolynomialsService.OutputComplexPolynomialsC(outputFile, ResultCols, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.ComplexCppMode) Process.Start(Globals.DocPath + Globals.PolynomialsComplexCpp);
                    }
                    
                    break;
                }                
                case AlgorithmOperation.ShortestPolyCpp:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.ShortestPolynomialsCpp))
                    {
                        OutputPolynomialsService.OutputShortestPolynomialsC(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.ShortestCppMode) Process.Start(Globals.DocPath + Globals.ShortestPolynomialsCpp);
                    }
                    
                    break;
                }
                case AlgorithmOperation.TestBenchCpp:
                {
                    using (var outputFile = File.CreateText(Globals.DocPath + Globals.TestBenchCpp))
                    {
                        OutputPolynomialsService.OutputTestBench(outputFile, Matrices, userParameters, Dimensions);
                        Thread.Sleep(500);
                        if (FileModeRepository.TestBenchMode) Process.Start(Globals.DocPath + Globals.TestBenchCpp);
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
