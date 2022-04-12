using BLL.Contracts;
using BLL.Models;
using DAL.Models;

namespace BLL.Services.CalculateOperationResults
{
    public class CalculateSumOperationResult : IOperationResultsCalculator
    {
        public Operation Operation => Operation.Sum;

        public int[] CalculateOperationResult(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters, int[,] operationValues)
        {
            var operationResults = new int[dimensions.DimensionRows];
            for (var k = 0; k < truthTable.DimensionRows; k++)
            {
                for (var m = 0; m < userParameters.OperandsNumber; m++)
                {
                    operationResults[k] += operationValues[k, m];
                }
            }
            return operationResults;
        }
    }
}
