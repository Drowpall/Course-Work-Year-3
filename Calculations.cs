using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string GetOperandsNum_toString() => operands_num.ToString();
        public static string GetDigitCapacity_toString() => digit_cap.ToString();
    }
}
