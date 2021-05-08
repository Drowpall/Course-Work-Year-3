using System;
using System.Collections.Generic;
using System.IO;
using BLL.Contracts;
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
        public static IOutputService OutputService { get; set; }


        private static Operation Operation => OperationRepository.GetOperation();
        private static int OperandsNumber => OperandsNumberRepository.GetOperandsNumber();
        private static int DigitCapacity => DigitCapacityRepository.GetDigitCapacity();
        private static int OperationModule => OperationModuleRepository.GetOperationModule();


        //private static void CalculateModuleLines(int[,] op_values, bool[] module_lines)
        //{
        //    for (int i = 0; i < DimensionRows; i++)
        //    {
        //        module_lines[i] = true;
        //        for (int j = 0; j < OperandsNumber; j++)
        //        {
        //            if (op_values[i, j] >= OperationModule)
        //            {
        //                module_lines[i] = false;
        //            }
        //        }
        //    }
        //}

        #region WriteFileNoSpaces

        //private static void WriteFile_TableValues_ModuleOperation(StreamWriter outputFile, bool[,] var_values, bool[,] res_values, bool[] module_lines)
        //{
        //    for (int i = 0; i < DimensionRows; i++)
        //    {
        //        if (module_lines[i])
        //        {
        //            for (int j = 0; j < DimensionVariablesColumns; j++)
        //            {
        //                outputFile.Write(Convert.ToInt32(var_values[i, j]).ToString());
        //            }

        //            outputFile.Write(" ");

        //            for (int j = 0; j < DimensionResultColumns; j++)
        //            {
        //                outputFile.Write(Convert.ToInt32(res_values[i, j]).ToString());
        //            }

        //            outputFile.WriteLine();
        //        }
        //    }
        //    outputFile.WriteLine($".e");
        //}

        #endregion

        #region Main
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

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTable0)))
            {
                OutputService.OutputTruthTable0(outputFile, truthTable, userParameters, dimensions);
            }

            using (StreamWriter outputFile = File.CreateText(Path.Combine(Globals.docPath, Globals.TruthTable1)))
            {
                OutputService.OutputTruthTable1(outputFile, truthTable, dimensions);
            }

            //bool[] module_lines = new bool[DimensionRows];

            //if (Operation == Operation.Mult2 || Operation == Operation.Sum2)
            //{
            //    using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable2.txt")))
            //    {
            //        WriteFile_UserVariables(outputFile);
            //        WriteFile_TableValues_ModuleOperation(outputFile, var_values, res_values, module_lines);
            //    }
            //}

        }
        #endregion
    }
}