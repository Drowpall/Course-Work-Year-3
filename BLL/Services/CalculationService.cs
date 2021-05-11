using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Models;
using Course_Work_v1;
using DAL.Contracts;
using DAL.Models;
using System.IO;

namespace BLL.Services
{
    public class CalculationService : ICalculationService
    {
        public CalculationService
        (
            IOperationRepository operationRepository, 
            IDigitCapacityRepository digitCapacityRepository, 
            IOperandsNumberRepository operandsNumberRepository, 
            IOperationModuleRepository operationModuleRepository, 
            ITruthTableCalculator truthTableCalculator, 
            IDimensionsService dimensionsService, 
            IOutputExtendedService outputExtendedService, 
            IOutputReducedService outputReducedService, 
            IOutputModuleService outputModuleService
        )
        {
            OperationRepository = operationRepository;
            DigitCapacityRepository = digitCapacityRepository;
            OperandsNumberRepository = operandsNumberRepository;
            OperationModuleRepository = operationModuleRepository;
            TruthTableCalculator = truthTableCalculator;
            DimensionsService = dimensionsService;
            OutputExtendedService = outputExtendedService;
            OutputReducedService = outputReducedService;
            OutputModuleService = outputModuleService;
        }

        public IOperationRepository OperationRepository { get; set; }
        public IDigitCapacityRepository DigitCapacityRepository { get; set; }
        public IOperandsNumberRepository OperandsNumberRepository { get; set; }
        public IOperationModuleRepository OperationModuleRepository { get; set; }
        public ITruthTableCalculator TruthTableCalculator { get; set; }
        public IDimensionsService DimensionsService { get; set; }

        public IOutputExtendedService OutputExtendedService { get; set; }
        public IOutputReducedService OutputReducedService { get; set; }
        public IOutputModuleService OutputModuleService { get; set; }


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
    }
}
