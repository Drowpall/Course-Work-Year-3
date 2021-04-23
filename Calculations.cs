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
            List<string> vars = new List<string>();
            List<string> res = new List<string>();

            for(int i = 0; i < digit_cap * operands_num; i++)
            {
                vars.Add($"X{i}");
            }

            //if (operation == Operation.Mult)
            //{
            for (int i = 0; i < 2 * digit_cap * operands_num; i++)
            {
                res.Add($"S{i}");
            }
            //}

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Truthtable.txt")))
            {
                for (int i = 0; i < digit_cap * operands_num; i++)
                {
                    if ((i + 1) % digit_cap == 0)
                    {
                        outputFile.Write(vars[i] + "   ");
                    }
                    else
                    {
                        //outputFile.Write(res[i] + " ");
                       outputFile.Write(vars[i] + "  ");
                    }
                }

                //if (operation == Operation.Mult)
                //{
                for (int i = 0; i < 2 * digit_cap * operands_num; i++)
                {
                    outputFile.Write(res[i] + " ");
                }
                //}

            }
        }

        public static string GetOperandsNum_toString() => operands_num.ToString();
        public static string GetDigitCapacity_toString() => digit_cap.ToString();
    }
}
