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
        private readonly Dictionary<Operation, IOperationResultsCalculator> operationResultsCalculatorsDict;

        public TruthTableCalculator(IEnumerable<IOperationResultsCalculator> operationResultsCalculators)
        {
            operationResultsCalculatorsDict = operationResultsCalculators.ToDictionary(c => c.Operation);
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

        public IEnumerable<bool[]> CalculateTruthTableComplex(Dimensions dimensions, UserParameters userParameters)
        {
            var resultCols = new List<bool[]>(dimensions.DimensionResultColumns);
            CalculateResultValuesComplex(resultCols, dimensions, userParameters);
            return resultCols;
        }

        private void CalculateVariableValues(TruthTable truthTable)
        {
            var rowValue = 0;
            for (var i = 0; i < truthTable.DimensionRows; i++)
            {
                for (var j = 0; j < truthTable.DimensionVariablesColumns; j++)
                {
                    truthTable.VariableValues[i, j] = GetRightNthBit(rowValue, truthTable.DimensionVariablesColumns - j);
                }

                rowValue++;
            }
        }

        private void CalculateResultValues(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);
            var operationResultCalculator = this.operationResultsCalculatorsDict[userParameters.Operation];

            var operationResults = operationResultCalculator.CalculateOperationResult(truthTable, dimensions, userParameters, operationValues);

            for (var i = 0; i < dimensions.DimensionRows; i++)
            {
                for (var j = 0; j < dimensions.DimensionResultColumns; j++)
                {
                    truthTable.ResultValues[i, j] = GetRightNthBit(operationResults[i], dimensions.DimensionResultColumns - j);
                }
            }
        }

        private void CalculateResultValuesComplex(ICollection<bool[]> resultCols, Dimensions dimensions, UserParameters userParameters)
        {
            for (var k = 0; k < dimensions.DimensionResultColumns; k++)     // For each result vector
            {
                var resultColVector = new bool[dimensions.DimensionRows];

                for (var row = 0; row < dimensions.DimensionRows; row++)    // for each row
                {
                    var rowValuesVector = new bool[dimensions.DimensionVariablesColumns];
                    var operandsValues = new int[userParameters.OperandsNumber];
                    
                    var valueCounter = 0;
                    for (var j = 0; j < dimensions.DimensionVariablesColumns; j++)  // for each element in a row
                    {
                        rowValuesVector[j] = GetRightNthBit(row, dimensions.DimensionVariablesColumns - j);
                    
                        if (rowValuesVector[j])
                        {
                            operandsValues[valueCounter / userParameters.DigitCapacity] += Convert.ToInt32(Math.Pow(2, (userParameters.DigitCapacity - 1) - j % userParameters.DigitCapacity));
                        }
                        valueCounter++;
                    }

                    var rowOperationResult = 0;
                    switch (userParameters.Operation)
                    {
                        case Operation.Sum:
                            for (var i = 0; i < userParameters.OperandsNumber; i++)
                            {
                                rowOperationResult += operandsValues[i];
                            }
                            
                            break;
                        case Operation.Sum2:
                            for (var i = 0; i < userParameters.OperandsNumber; i++)
                            {
                                rowOperationResult += operandsValues[i];
                            }
                            rowOperationResult %= userParameters.OperationModule;
                            
                            break;
                        case Operation.Mult:
                            for (var i = 0; i < userParameters.OperandsNumber; i++)
                            {
                                rowOperationResult *= operandsValues[i];
                            }
                            
                            break;
                        case Operation.Mult2:
                            for (var i = 0; i < userParameters.OperandsNumber; i++)
                            {
                                rowOperationResult *= operandsValues[i];
                            }
                            rowOperationResult %= userParameters.OperationModule;
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    resultColVector[row] = GetRightNthBit(rowOperationResult, dimensions.DimensionResultColumns - k);
                }
 

                resultCols.Add(resultColVector);
            }
        }

        private static int[,] CalculateOperationValues(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            var operationValues = new int[dimensions.DimensionRows, userParameters.OperandsNumber];

            for (var i = 0; i < dimensions.DimensionRows; i++)
            {
                var valueCounter = 0;

                for (int j = 0; j < dimensions.DimensionVariablesColumns; j++)
                {
                    if (truthTable.VariableValues[i, j] == true)
                    {
                        operationValues[i, valueCounter / userParameters.DigitCapacity] += Convert.ToInt32(Math.Pow(2, (userParameters.DigitCapacity - 1) - j % userParameters.DigitCapacity));
                    }
                    valueCounter++;
                }
            }

            return operationValues;
        }

        private static bool GetRightNthBit(int val, int n)
        {
            return (val & (1 << (n - 1))) >> (n - 1) != 0;
        }

        private static void CalculateModuleLines(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            if (userParameters.OperationModule == -1) return;
            
            var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);

            for (var i = 0; i < truthTable.DimensionRows; i++)
            {
                truthTable.ModuleRows[i] = true;
                for (var j = 0; j < userParameters.OperandsNumber; j++)
                {
                    if (operationValues[i, j] >= userParameters.OperationModule)
                    {
                        truthTable.ModuleRows[i] = false;
                    }
                }
            }
        }
        
        private static void CalculateModuleCols(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            if (userParameters.OperationModule == -1) return;
            
            var operationValues = CalculateOperationValues(truthTable, dimensions, userParameters);
            var numberOfColumnsToBeTaken = 0;

            if (operationValues[dimensions.DimensionRows - 1, 0] >= userParameters.OperationModule)
            {
                var operationValueSize = Convert.ToInt32(Math.Ceiling(Math.Log(operationValues[dimensions.DimensionRows - 1, 0], 2)));
                var operationModuleSize = Convert.ToInt32(Math.Ceiling(Math.Log(userParameters.OperationModule, 2)));
                numberOfColumnsToBeTaken = operationValueSize - operationModuleSize;
            }

            for (var col = 0; col < dimensions.DimensionVariablesColumns; col++)
            {
                truthTable.ModuleCols[col] = true;
            }

            if (numberOfColumnsToBeTaken <= 0) return;
            
            for (var operand = 0; operand < userParameters.OperandsNumber; operand++)
            {
                truthTable.ModuleCols[operand * userParameters.DigitCapacity + numberOfColumnsToBeTaken - 1] = false;
            }
        }
    }
}
