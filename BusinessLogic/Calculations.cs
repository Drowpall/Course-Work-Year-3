using System;
using System.Collections.Generic;
using System.IO;
using BLL.Contracts;
using BLL.Contracts.IOutput;
using BLL.Models;
using BLL.Services;
using DAL.Contracts;
using DAL.Models;

namespace Course_Work_v1.BusinessLogic
{
    public static class Calculations
    {
        public static IOperationRepository OperationRepository { get; set; }
        public static IDigitCapacityRepository DigitCapacityRepository { get; set; }
        public static IOperandsNumberRepository OperandsNumberRepository { get; set; }
        public static IOperationModuleRepository OperationModuleRepository { get; set; }
        public static ITruthTableCalculator TruthTableCalculator { get; set; }
        public static IDimensionsService DimensionsService { get; set; }

        public static IOutputExtendedService OutputExtendedService { get; set; }
        public static IOutputReducedService OutputReducedService { get; set; }
        public static IOutputModuleService OutputModuleService { get; set; }


        private static Operation Operation => OperationRepository.GetOperation();
        private static int OperandsNumber => OperandsNumberRepository.GetOperandsNumber();
        private static int DigitCapacity => DigitCapacityRepository.GetDigitCapacity();
        private static int OperationModule => OperationModuleRepository.GetOperationModule();

        internal static void DrawTruthTable()
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

            if(userParameters.OperationModule != -1)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, Globals.TruthTableModule)))
                {
                    OutputModuleService.OutputModuleTruthTable(outputFile, truthTable, userParameters, dimensions);
                }
            }

        }
    }
}