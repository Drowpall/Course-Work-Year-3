using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Course_Work_v1
{
    public enum Operation
    {
        Sum,
        Sum2,
        Mult,
        Mult2
    }
    public static class Calculations
    {
        /// <summary>
        ///     
        /// </summary>
        
        #region Variable Declarations
        private static Operation operation;
        private static int operands_num;
        private static int digit_cap;
        private static int iteration_size;
        private static int dimension_rows;
        private static int dimension_vars_cols;
        private static int dimension_res_cols;
        private static int operation_module;
        #endregion
        #region Setters Fields
        internal static void SetOperation(Operation op)
        {
            operation = op;
        }
        internal static void SetNumberOfOperands(int num)
        {
            operands_num = num;
        }
        internal static void SetDigitCapacity(int cap)
        {
            digit_cap = cap;
        }
        internal static void SetOperationModule(int operationModule)
        {
            operation_module = operationModule;
        }

        #endregion
        #region Getters Fields

        internal static int GetOperandsNum() => operands_num;
        internal static int GetDigitCapacity() => digit_cap;
        internal static int GetOperationModule() => operation_module;
        internal static int GetDimensionRows() => dimension_rows;
        internal static int GetDimensionResCols() => dimension_res_cols;
        internal static int GetDimensionVarsCols() => dimension_vars_cols;
        internal static Operation GetOperation() => operation;
        #endregion
        #region Getters Logic
        public static void GetIterationSize()
        {
            switch (operation)
            {
                case Operation.Sum:
                    iteration_size = Convert.ToInt32(Math.Ceiling(Math.Log((Math.Pow(2, digit_cap) - 1) * operands_num, 2)));
                    if(digit_cap == 1)
                    {
                        iteration_size += 1;
                    }
                    break;
                case Operation.Sum2:
                    iteration_size = Convert.ToInt32(Math.Ceiling(Math.Log(operation_module, 2)));
                    break;
                case Operation.Mult:
                    iteration_size = Convert.ToInt32(Math.Ceiling(Math.Log(Math.Pow((Math.Pow(2, digit_cap) - 1), operands_num), 2)));
                    break;
                case Operation.Mult2:
                    iteration_size = Convert.ToInt32(Math.Ceiling(Math.Log(operation_module, 2)));
                    break;
                default:
                    iteration_size = -1;
                    break;
            }
        }
        private static void GetTableDimension_Rows()
        {
            dimension_rows = (int)Math.Pow(2, operands_num * digit_cap);
        }
        private static void GetTableDimension_ValsCols()
        {
            dimension_vars_cols = digit_cap * operands_num;
        }
        private static void GetTableDimension_ResCols()
        {
            dimension_res_cols = iteration_size;
        }
        private static void GetTableDimensions()
        {
            GetIterationSize();
            GetTableDimension_Rows();
            GetTableDimension_ValsCols();
            GetTableDimension_ResCols();
        }
        private static bool GetRightNthBit(int val, int n)
        {
            return ((val & (1 << (n - 1))) >> (n - 1)) != 0;
        }
        #endregion
        #region ToString
        internal static string Operation_toString()
        {
            switch (operation)
            {
                case Operation.Sum:
                    return "summ";
                case Operation.Sum2:
                    return "summ2";
                case Operation.Mult:
                    return "mult";
                case Operation.Mult2:
                    return "mult2";
                default:
                    return "Error!";
            }
        }
        internal static string OperandsNum_toString() => operands_num.ToString();
        internal static string DigitCapacity_toString() => digit_cap.ToString();
        internal static string OperationModule_toString() => operation_module.ToString();
        #endregion
        #region Calculations
        private static void CalculateOperationValues(ref int[,] op_values, ref bool[,] var_values)
        {
            int value_counter;
            for (int i = 0; i < dimension_rows; i++)
            {
                value_counter = 0;

                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    if (var_values[i, j] == true)
                    {
                        op_values[i, value_counter / digit_cap] += Convert.ToInt32(Math.Pow(2, (digit_cap - 1) - j % digit_cap));
                    }
                    value_counter++;
                }
            }
        }
        private static void CalculateOperationResults(ref int[,] op_values, ref int[] op_results)
        {
            switch (operation)
            {
                case Operation.Sum:
                    for (int k = 0; k < dimension_rows; k++)
                    {
                        for (int m = 0; m < operands_num; m++)
                        {
                            op_results[k] += op_values[k, m];
                        }
                    }
                    break;
                case Operation.Sum2:
                    for (int k = 0; k < dimension_rows; k++)
                    {
                        for (int m = 0; m < operands_num; m++)
                        {
                            op_results[k] += op_values[k, m];
                        }
                        op_results[k] = op_results[k] % operation_module;
                    }
                    break;
                case Operation.Mult:
                    for (int k = 0; k < dimension_rows; k++)
                    {
                        op_results[k] = 1;

                        for (int m = 0; m < operands_num; m++)
                        {
                            op_results[k] *= op_values[k, m];
                        }
                    }
                    break;
                case Operation.Mult2:
                    for (int k = 0; k < dimension_rows; k++)
                    {
                        op_results[k] = 1;

                        for (int m = 0; m < operands_num; m++)
                        {
                            op_results[k] *= op_values[k, m];
                        }
                        op_results[k] = op_results[k] % operation_module;
                    }
                    break;
                default:
                    break;
            }
        }
        private static void CalculateModuleLines(ref int[,] op_values, ref bool[] module_lines)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                module_lines[i] = true;
                for (int j = 0; j < operands_num; j++)
                {
                    if (op_values[i, j] >= operation_module)
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
            for (int i = 0; i < iteration_size; i++)
            {
                res.Add($"S{i}");
            }
        }
        private static void FillList_VarNames(ref List<string> vars)
        {
            for (int i = 0; i < digit_cap * operands_num; i++)
            {
                vars.Add($"X{i}");
            }
        }
        private static void FillMatrix_VarValues(ref bool[,] var_values)
        {
            int row_value = 0;
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    var_values[i, j] = GetRightNthBit(row_value, dimension_vars_cols - j);
                }
                row_value++;
            }
        }
        private static void FillMatrix_ResValues(ref bool[,] res_values, ref bool[,] var_values, ref bool[] module_lines)
        {
            int[,] op_values = new int[dimension_rows, operands_num];
            CalculateOperationValues(ref op_values, ref var_values);
            int[] op_results = new int[dimension_rows];
            CalculateOperationResults(ref op_values, ref op_results);

            if (operation == Operation.Mult2 || operation == Operation.Sum2)
                CalculateModuleLines(ref op_values, ref module_lines);

            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_res_cols; j++)
                {
                    res_values[i, j] = GetRightNthBit(op_results[i], dimension_res_cols - j);
                }
            }

            ReedMullerExpansion.SetTruthTableResValues(res_values);
        }
        #endregion
        #region WriteFileDefault
        private static void WriteFile_TruthTable_VarNames(StreamWriter outputFile, ref List<string> vars)
        {
            for (int i = 0; i < digit_cap * operands_num; i++)
            {
                if ((i + 1) % digit_cap == 0)
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
            for (int i = 0; i < iteration_size; i++)
            {
                outputFile.Write(res[i] + "   ");
            }
            outputFile.WriteLine();
        }
        private static void WriteFile_TruthTable_Values(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString());
                    outputFile.Write("   ");

                    if ((j + 1) % digit_cap == 0)
                    {
                        outputFile.Write(" ");
                    }

                    if (j > 9)
                    {
                        outputFile.Write(" ");
                    }

                }

                for (int j = 0; j < dimension_res_cols; j++)
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
            outputFile.WriteLine($".i {dimension_vars_cols}");
            outputFile.WriteLine($".o {dimension_res_cols}");
            outputFile.WriteLine($".p {dimension_rows}");
        }
        private static void WriteFile_TableValues(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    outputFile.Write(Convert.ToInt32(var_values[i, j]).ToString());
                }

                outputFile.Write(" ");

                for (int j = 0; j < dimension_res_cols; j++)
                {
                    outputFile.Write(Convert.ToInt32(res_values[i, j]).ToString());
                }

                outputFile.WriteLine();
            }
            outputFile.WriteLine($".e");
        }
        private static void WriteFile_TableValues_ModuleOperation(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values, ref bool[] module_lines)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                if (module_lines[i])
                {
                    for (int j = 0; j < dimension_vars_cols; j++)
                    {
                        outputFile.Write(Convert.ToInt32(var_values[i, j]).ToString());
                    }

                    outputFile.Write(" ");

                    for (int j = 0; j < dimension_res_cols; j++)
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
            bool[,] var_values = new bool[dimension_rows, dimension_vars_cols];
            bool[,] res_values = new bool[dimension_rows, dimension_res_cols];
            bool[] module_lines = new bool[dimension_rows];

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

            if(operation == Operation.Mult2 || operation == Operation.Sum2)
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
