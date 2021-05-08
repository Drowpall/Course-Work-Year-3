using BLL.Contracts;
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TruthTableCalculator : ITruthTableCalculator
    {
        Dictionary<Operation, IOperationResultsCalculator> operationResultsCalculatorsDict;

        public TruthTableCalculator(List<IOperationResultsCalculator> operationResultsCalculators)
        {
            this.operationResultsCalculatorsDict = operationResultsCalculators.ToDictionary(c => c.Operation);
        }

        public TruthTable CalculateTruthTable(Dimensions dimensions, UserParameters userParemeters)
        {
            var truthTable = new TruthTable(dimensions.DimensionRows, dimensions.DimensionResultColumns, dimensions.DimensionVariablesColumns);
            CalculateVariableValues(truthTable);
            CalculateResultValues(truthTable, dimensions, userParemeters);

            return truthTable;
        }

        private static void CalculateVariableValues(TruthTable truthTable)
        {
            int row_value = 0;
            for (int i = 0; i < truthTable.DimensionRows; i++)
            {
                for (int j = 0; j < truthTable.DimensionVariablesColumns; j++)
                {
                    truthTable.VariableValues[i, j] = GetRightNthBit(row_value, truthTable.DimensionVariablesColumns - j);
                }

                row_value++;
            }
        }
        private void CalculateResultValues(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);
            var operationResultCalucator = this.operationResultsCalculatorsDict[userParameters.Operation];

            var operationResults = operationResultCalucator.CalculateOperationResult(truthTable, dimensions, userParameters, operationValues);

            //if (userParameters.Operation == Operation.Mult2 || userParameters.Operation == Operation.Sum2)
            //    CalculateModuleLines(op_values, module_lines);

            for (int i = 0; i < dimensions.DimensionRows; i++)
            {
                for (int j = 0; j < dimensions.DimensionResultColumns; j++)
                {
                    truthTable.ResultValues[i, j] = GetRightNthBit(operationResults[i], dimensions.DimensionResultColumns - j);
                }
            }

            //ReedMullerExpansion.SetTruthTableResValues(res_values);
        }

        private static int[,] CalculateOperationValues(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            int[,] operationValues = new int[dimensions.DimensionRows, userParameters.OperandsNumber];

            int value_counter;
            for (int i = 0; i < dimensions.DimensionRows; i++)
            {
                value_counter = 0;

                for (int j = 0; j < dimensions.DimensionVariablesColumns; j++)
                {
                    if (truthTable.VariableValues[i, j] == true)
                    {
                        operationValues[i, value_counter / userParameters.DigitCapacity] += Convert.ToInt32(Math.Pow(2, (userParameters.DigitCapacity - 1) - j % userParameters.DigitCapacity));
                    }
                    value_counter++;
                }
            }

            return operationValues;
        }

        private static bool GetRightNthBit(int val, int n)
        {
            return ((val & (1 << (n - 1))) >> (n - 1)) != 0;
        }
    }
}
