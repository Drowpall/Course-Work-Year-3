using System;
using System.Collections.Generic;
using System.IO;
using BLL.Contracts;
using BLL.Models;
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

        #region Properties

        private static Operation Operation => OperationRepository.GetOperation();
        private static int OperandsNumber => OperandsNumberRepository.GetOperandsNumber();
        private static int DigitCapacity => DigitCapacityRepository.GetDigitCapacity();
        private static int OperationModule => OperationModuleRepository.GetOperationModule();

        private static int IterationSize { get; set; }

        public static int DimensionRows { get; set; }

        public static int DimensionResultColumns { get; set; }

        private static int DimensionVariablesColumns { get; set; }

        #endregion
        #region Calculations

        private static void CalculateModuleLines(int[,] op_values, bool[] module_lines)
        {
            for (int i = 0; i < DimensionRows; i++)
            {
                module_lines[i] = true;
                for (int j = 0; j < OperandsNumber; j++)
                {
                    if (op_values[i, j] >= OperationModule)
                    {
                        module_lines[i] = false;
                    }
                }
            }
        }
        #endregion
        #region FillArrays
        private static void FillList_ResNames(List<string> res)
        {
            for (int i = 0; i < IterationSize; i++)
            {
                res.Add($"S{i}");
            }
        }
        private static void FillList_VarNames(List<string> vars)
        {
            for (int i = 0; i < DigitCapacity * OperandsNumber; i++)
            {
                vars.Add($"X{i}");
            }
        }
        #endregion
        #region WriteFileDefault
        private static void WriteFile_TruthTable_VarNames(StreamWriter outputFile, List<string> vars)
        {
            for (int i = 0; i < DigitCapacity * OperandsNumber; i++)
            {
                if ((i + 1) % DigitCapacity == 0)
                {
                    outputFile.Write(vars[i] + "   ");
                }
                else
                {
                    outputFile.Write(vars[i] + "  ");
                }
            }
        }
        private static void WriteFile_TruthTable_ResNames(StreamWriter outputFile, List<string> res)
        {
            for (int i = 0; i < IterationSize; i++)
            {
                outputFile.Write(res[i] + "   ");
            }
            outputFile.WriteLine();
        }
        private static void WriteFile_TruthTable_Values(StreamWriter outputFile, TruthTable truthTable)
        {
            for (int i = 0; i < DimensionRows; i++)
            {
                for (int j = 0; j < DimensionVariablesColumns; j++)
                {
                    outputFile.Write((Convert.ToInt32(truthTable.VariableValues[i, j])).ToString());
                    outputFile.Write("   ");

                    if ((j + 1) % DigitCapacity == 0)
                    {
                        outputFile.Write(" ");
                    }

                    if (j > 9)
                    {
                        outputFile.Write(" ");
                    }

                }

                for (int j = 0; j < DimensionResultColumns; j++)
                {
                    outputFile.Write((Convert.ToInt32(truthTable.ResultValues[i, j])).ToString());
                    outputFile.Write("    ");

                    if (j > 9)
                    {
                        outputFile.Write(" ");
                    }
                }

                outputFile.WriteLine();
            }
        }
        #endregion
        #region WriteFileNoSpaces
        private static void WriteFile_UserVariables(StreamWriter outputFile)
        {
            outputFile.WriteLine($".i {DimensionVariablesColumns}");
            outputFile.WriteLine($".o {DimensionResultColumns}");
            outputFile.WriteLine($".p {DimensionRows}");
        }
        private static void WriteFile_TableValues(StreamWriter outputFile, TruthTable truthTable)
        {
            for (int i = 0; i < DimensionRows; i++)
            {
                for (int j = 0; j < DimensionVariablesColumns; j++)
                {
                    outputFile.Write(Convert.ToInt32(truthTable.VariableValues[i, j]).ToString());
                }

                outputFile.Write(" ");

                for (int j = 0; j < DimensionResultColumns; j++)
                {
                    outputFile.Write(Convert.ToInt32(truthTable.ResultValues[i, j]).ToString());
                }

                outputFile.WriteLine();
            }
            outputFile.WriteLine($".e");
        }
        private static void WriteFile_TableValues_ModuleOperation(StreamWriter outputFile, bool[,] var_values, bool[,] res_values, bool[] module_lines)
        {
            for (int i = 0; i < DimensionRows; i++)
            {
                if (module_lines[i])
                {
                    for (int j = 0; j < DimensionVariablesColumns; j++)
                    {
                        outputFile.Write(Convert.ToInt32(var_values[i, j]).ToString());
                    }

                    outputFile.Write(" ");

                    for (int j = 0; j < DimensionResultColumns; j++)
                    {
                        outputFile.Write(Convert.ToInt32(res_values[i, j]).ToString());
                    }

                    outputFile.WriteLine();
                }
            }
            outputFile.WriteLine($".e");
        }

        #endregion

        #region Main
        internal static void DrawTruthTable()
        {
            var dimensions = DimensionsService.GetDimensions();
            IterationSize = dimensions.IterationSize;
            DimensionRows = dimensions.DimensionRows;
            DimensionResultColumns = dimensions.DimensionResultColumns;
            DimensionVariablesColumns = dimensions.DimensionVariablesColumns;

            var userParameters = new UserParameters()
            {
                DigitCapacity = DigitCapacity,
                OperandsNumber = OperandsNumber,
                OperationModule = OperationModule,
                Operation = Operation
            };
            var truthTable = TruthTableCalculator.CalculateTruthTable(dimensions, userParameters);

            List<string> vars = new List<string>();
            List<string> res = new List<string>();

            //bool[] module_lines = new bool[DimensionRows];

            FillList_VarNames(vars);
            FillList_ResNames(res);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable.txt")))
            {
                WriteFile_TruthTable_VarNames(outputFile, vars);
                WriteFile_TruthTable_ResNames(outputFile, res);
                WriteFile_TruthTable_Values(outputFile, truthTable);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable1.txt")))
            {
                WriteFile_UserVariables(outputFile);
                WriteFile_TableValues(outputFile, truthTable);
            }

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