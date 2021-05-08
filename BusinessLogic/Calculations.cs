using System;
using System.Collections.Generic;
using System.IO;
using DAL.Contracts;
using DAL.Models;

namespace Course_Work_v1.BusinessLogic
{

    public static class Calculations
    {
        public static IOperationRepository OperationRepository { get; set; }

        #region Properties

        private static Operation Operation => OperationRepository.GetOperation();

        public static int OperandsNumber { get; set; }

        public static int DigitCapacity { get; set; }

        public static int OperationModule { get; set; }

        private static int IterationSize { get; set; }

        public static int DimensionRows { get; set; }

        public static int DimensionResultColumns { get; set; }

        private static int DimensionVariablesColumns { get; set; }
        
        #endregion

        #region Getters Logic

        public static void GetIterationSize()
        {
            switch (Operation)
            {
                case Operation.Sum:
                    IterationSize = Convert.ToInt32(Math.Ceiling(Math.Log((Math.Pow(2, DigitCapacity) - 1) * OperandsNumber, 2)));
                    if(DigitCapacity == 1)
                    {
                        IterationSize += 1;
                    }
                    break;
                case Operation.Sum2:
                    IterationSize = Convert.ToInt32(Math.Ceiling(Math.Log(OperationModule, 2)));
                    break;
                case Operation.Mult:
                    IterationSize = Convert.ToInt32(Math.Ceiling(Math.Log(Math.Pow((Math.Pow(2, DigitCapacity) - 1), OperandsNumber), 2)));
                    break;
                case Operation.Mult2:
                    IterationSize = Convert.ToInt32(Math.Ceiling(Math.Log(OperationModule, 2)));
                    break;
                default:
                    IterationSize = -1;
                    break;
            }
        }

        private static void GetTableDimensions()
        {
            GetIterationSize();

            DimensionRows = (int)Math.Pow(2, OperandsNumber * DigitCapacity);

            DimensionVariablesColumns = DigitCapacity * OperandsNumber;

            DimensionResultColumns = IterationSize;
        }

        private static bool GetRightNthBit(int val, int n)
        {
            return ((val & (1 << (n - 1))) >> (n - 1)) != 0;
        }

        #endregion

        #region Calculations

        private static void CalculateOperationValues(ref int[,] op_values, ref bool[,] var_values)
        {
            int value_counter;
            for (int i = 0; i < DimensionRows; i++)
            {
                value_counter = 0;

                for (int j = 0; j < DimensionVariablesColumns; j++)
                {
                    if (var_values[i, j] == true)
                    {
                        op_values[i, value_counter / DigitCapacity] += Convert.ToInt32(Math.Pow(2, (DigitCapacity - 1) - j % DigitCapacity));
                    }
                    value_counter++;
                }
            }
        }
        private static void CalculateOperationResults(ref int[,] op_values, ref int[] op_results)
        {
            switch (Operation)
            {
                case Operation.Sum:
                    for (int k = 0; k < DimensionRows; k++)
                    {
                        for (int m = 0; m < OperandsNumber; m++)
                        {
                            op_results[k] += op_values[k, m];
                        }
                    }
                    break;
                case Operation.Sum2:
                    for (int k = 0; k < DimensionRows; k++)
                    {
                        for (int m = 0; m < OperandsNumber; m++)
                        {
                            op_results[k] += op_values[k, m];
                        }
                        op_results[k] = op_results[k] % OperationModule;
                    }
                    break;
                case Operation.Mult:
                    for (int k = 0; k < DimensionRows; k++)
                    {
                        op_results[k] = 1;

                        for (int m = 0; m < OperandsNumber; m++)
                        {
                            op_results[k] *= op_values[k, m];
                        }
                    }
                    break;
                case Operation.Mult2:
                    for (int k = 0; k < DimensionRows; k++)
                    {
                        op_results[k] = 1;

                        for (int m = 0; m < OperandsNumber; m++)
                        {
                            op_results[k] *= op_values[k, m];
                        }
                        op_results[k] = op_results[k] % OperationModule;
                    }
                    break;
                default:
                    break;
            }
        }
        private static void CalculateModuleLines(ref int[,] op_values, ref bool[] module_lines)
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
        private static void FillList_ResNames(ref List<string> res)
        {
            for (int i = 0; i < IterationSize; i++)
            {
                res.Add($"S{i}");
            }
        }
        private static void FillList_VarNames(ref List<string> vars)
        {
            for (int i = 0; i < DigitCapacity * OperandsNumber; i++)
            {
                vars.Add($"X{i}");
            }
        }
        private static void FillMatrix_VarValues(ref bool[,] var_values)
        {
            int row_value = 0;
            for (int i = 0; i < DimensionRows; i++)
            {
                for (int j = 0; j < DimensionVariablesColumns; j++)
                {
                    var_values[i, j] = GetRightNthBit(row_value, DimensionVariablesColumns - j);
                }
                row_value++;
            }
        }
        private static void FillMatrix_ResValues(ref bool[,] res_values, ref bool[,] var_values, ref bool[] module_lines)
        {
            int[,] op_values = new int[DimensionRows, OperandsNumber];
            CalculateOperationValues(ref op_values, ref var_values);
            int[] op_results = new int[DimensionRows];
            CalculateOperationResults(ref op_values, ref op_results);

            if (Operation == Operation.Mult2 || Operation == Operation.Sum2)
                CalculateModuleLines(ref op_values, ref module_lines);

            for (int i = 0; i < DimensionRows; i++)
            {
                for (int j = 0; j < DimensionResultColumns; j++)
                {
                    res_values[i, j] = GetRightNthBit(op_results[i], DimensionResultColumns - j);
                }
            }

            ReedMullerExpansion.SetTruthTableResValues(res_values);
        }
        #endregion
        #region WriteFileDefault
        private static void WriteFile_TruthTable_VarNames(StreamWriter outputFile, ref List<string> vars)
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
        private static void WriteFile_TruthTable_ResNames(StreamWriter outputFile, ref List<string> res)
        {
            for (int i = 0; i < IterationSize; i++)
            {
                outputFile.Write(res[i] + "   ");
            }
            outputFile.WriteLine();
        }
        private static void WriteFile_TruthTable_Values(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values)
        {
            for (int i = 0; i < DimensionRows; i++)
            {
                for (int j = 0; j < DimensionVariablesColumns; j++)
                {
                    outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString());
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
                    outputFile.Write((Convert.ToInt32(res_values[i, j])).ToString());
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
        private static void WriteFile_TableValues(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values)
        {
            for (int i = 0; i < DimensionRows; i++)
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
            outputFile.WriteLine($".e");
        }
        private static void WriteFile_TableValues_ModuleOperation(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values, ref bool[] module_lines)
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
            GetTableDimensions();

            List<string> vars = new List<string>();
            List<string> res = new List<string>();
            bool[,] var_values = new bool[DimensionRows, DimensionVariablesColumns];
            bool[,] res_values = new bool[DimensionRows, DimensionResultColumns];
            bool[] module_lines = new bool[DimensionRows];

            FillList_VarNames(ref vars);
            FillList_ResNames(ref res);
            FillMatrix_VarValues(ref var_values);
            FillMatrix_ResValues(ref res_values, ref var_values, ref module_lines);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable.txt")))
            {
                WriteFile_TruthTable_VarNames(outputFile, ref vars);
                WriteFile_TruthTable_ResNames(outputFile, ref res);
                WriteFile_TruthTable_Values(outputFile, ref var_values, ref res_values);
            }

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable1.txt")))
            {
                WriteFile_UserVariables(outputFile);
                WriteFile_TableValues(outputFile, ref var_values, ref res_values);
            }

            if(Operation == Operation.Mult2 || Operation == Operation.Sum2)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Truthtable2.txt")))
                {
                    WriteFile_UserVariables(outputFile);
                    WriteFile_TableValues_ModuleOperation(outputFile, ref var_values, ref res_values, ref module_lines);
                }
            }    

        }
        #endregion
    }
}