using BLL.Contracts;
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class TruthTableCalculator : ITruthTableCalculator
    {
        Dictionary<Operation, IOperationResultsCalculator> operationResultsCalculatorsDict;

        public TruthTableCalculator(List<IOperationResultsCalculator> operationResultsCalculators)
        {
            this.operationResultsCalculatorsDict = operationResultsCalculators.ToDictionary(c => c.Operation);
        }

        public TruthTable CalculateTruthTable(Dimensions dimensions, UserParameters userParameters)
        {
            var truthTable = new TruthTable(dimensions.DimensionRows, dimensions.DimensionResultColumns, dimensions.DimensionVariablesColumns);
            CalculateVariableValues(truthTable);
            CalculateResultValues(truthTable, dimensions, userParameters);
            CalculateModuleLines(truthTable, dimensions, userParameters);
            CalculateModuleCols(truthTable, dimensions, userParameters);

            return truthTable;
        }

        private void CalculateVariableValues(TruthTable truthTable)
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

            for (int i = 0; i < dimensions.DimensionRows; i++)
            {
                for (int j = 0; j < dimensions.DimensionResultColumns; j++)
                {
                    truthTable.ResultValues[i, j] = GetRightNthBit(operationResults[i], dimensions.DimensionResultColumns - j);
                }
            }
        }

        private int[,] CalculateOperationValues(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
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

        private bool GetRightNthBit(int val, int n)
        {
            return ((val & (1 << (n - 1))) >> (n - 1)) != 0;
        }

        private void CalculateModuleLines(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            if(userParameters.OperationModule != -1)
            {
                var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);

                for (int i = 0; i < truthTable.DimensionRows; i++)
                {
                    truthTable.ModuleRows[i] = true;
                    for (int j = 0; j < userParameters.OperandsNumber; j++)
                    {
                        if (operationValues[i, j] >= userParameters.OperationModule)
                        {
                            truthTable.ModuleRows[i] = false;
                        }
                    }
                }
            }
        }
        
        private void CalculateModuleCols(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            if(userParameters.OperationModule != -1)
            {
                var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);
                int operationValueSize;
                int operationModuleSize;
                int numberOfColumnsToBeTaken = 0;

                if (operationValues[dimensions.DimensionRows - 1, 0] >= userParameters.OperationModule)
                {
                    operationValueSize = Convert.ToInt32(Math.Ceiling(Math.Log(operationValues[dimensions.DimensionRows - 1, 0], 2)));
                    operationModuleSize = Convert.ToInt32(Math.Ceiling(Math.Log(userParameters.OperationModule, 2)));
                    numberOfColumnsToBeTaken = operationValueSize - operationModuleSize;
                }

                for (int col = 0; col < dimensions.DimensionVariablesColumns; col++)
                {
                    truthTable.ModuleCols[col] = true;
                }

                if(numberOfColumnsToBeTaken > 0)
                {
                    for (int operand = 0; operand < userParameters.OperandsNumber; operand++)
                    {
                        truthTable.ModuleCols[operand * userParameters.DigitCapacity + numberOfColumnsToBeTaken - 1] = false;
                    }
                }
            }
        }
    }
}
