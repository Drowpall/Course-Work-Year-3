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
        private static Operation operation;
        private static int operands_num;
        private static int digit_cap;
        private static int iteration_size;
        private static int dimension_rows;
        private static int dimension_vars_cols;
        private static int dimension_res_cols;



        public static void SetOperation(Operation op)
        {
            operation = op;
        }
        public static void SetNumberOfOperands(int num)
        {
            operands_num = num;
        }
        public static void SetDigitCapacity(int cap)
        {
            digit_cap = cap;
        }
        public static string GetOperation_toString()
        {
            switch(operation)
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

        public static void DrawTruthTable()
        {
            GetTableDimensions();

            List<string> vars = new List<string>();
            List<string> res = new List<string>();
            bool[,] var_values = new bool[dimension_rows, dimension_vars_cols];
            bool[,] res_values = new bool[dimension_rows, dimension_res_cols];

            FillList_VarNames(ref vars);
            FillList_ResNames(ref res);
            FillMatrix_VarValues(ref var_values);
            FillMatrix_ResValues(ref res_values);

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Truthtable.txt")))
            {
                WriteFile_TruthTable_VarNames(outputFile, ref vars);
                WriteFile_TruthTable_ResNames(outputFile, ref res);
                WriteFile_TruthTable_Values(outputFile, ref var_values, ref res_values);
                //WriteFile_VarValues(outputFile, ref var_values);
                //WriteFile_ResValues(outputFile, ref res_values);

            }
        }




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
        private static void FillMatrix_ResValues(ref bool[,] res_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_res_cols; j++)
                {
                    switch (operation)
                    {
                        case Operation.Sum:
                            res_values[i, j] = false;
                            break;
                        case Operation.Sum2:
                            res_values[i, j] = false;
                            break;
                        case Operation.Mult:
                            res_values[i, j] = false;
                            break;
                        case Operation.Mult2:
                            res_values[i, j] = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
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
        private static void WriteFile_VarValues(StreamWriter outputFile, ref bool[,] var_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    if ((j + 1) % digit_cap == 0)
                    {
                        outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString() + "    ");
                    }
                    else
                    {
                        outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString() + "   ");
                    }
                }
                outputFile.WriteLine();
            }
        }
        private static void WriteFile_ResValues(StreamWriter outputFile, ref bool[,] res_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_res_cols; j++)
                {
                    outputFile.Write((Convert.ToInt32(res_values[i, j])).ToString() + "   ");
                }
                outputFile.WriteLine();
            }
        }

        private static void WriteFile_TruthTable_Values(StreamWriter outputFile, ref bool[,] var_values, ref bool[,] res_values)
        {
            for (int i = 0; i < dimension_rows; i++)
            {
                for (int j = 0; j < dimension_vars_cols; j++)
                {
                    if ((j + 1) % digit_cap == 0)
                    {
                        outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString() + "    ");
                    }
                    else
                    {
                        outputFile.Write((Convert.ToInt32(var_values[i, j])).ToString() + "   ");
                    }
                }

                for (int j = 0; j < dimension_res_cols; j++)
                {
                    outputFile.Write((Convert.ToInt32(res_values[i, j])).ToString() + "   ");
                }

                outputFile.WriteLine();
            }
        }

        public static void GetIterationSize()
        {
            switch (operation)
            {
                case Operation.Sum:
                    iteration_size = Convert.ToInt32(Math.Log((Math.Pow(2, digit_cap) - 1) * operands_num, 2));
                    break;
                case Operation.Sum2:
                    iteration_size = digit_cap;
                    break;
                case Operation.Mult:
                    iteration_size = Convert.ToInt32(Math.Log(Math.Pow((Math.Pow(2, digit_cap) - 1), operands_num), 2));
                    break;
                case Operation.Mult2:
                    iteration_size = digit_cap;
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

        public static string GetOperandsNum_toString() => operands_num.ToString();
        public static string GetDigitCapacity_toString() => digit_cap.ToString();
    }
}
