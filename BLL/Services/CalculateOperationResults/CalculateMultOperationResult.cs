using BLL.Contracts;
using BLL.Models;
using DAL.Models;

namespace BLL.Services.CalculateOperationResults
{
    public class CalculateMultOperationResult : IOperationResultsCalculator
    {
        public Operation Operation => Operation.Mult;

        public int[] CalculateOperationResult(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters, int[,] operationValues)
        {
            var operationResults = new int[dimensions.DimensionRows];
            for (var k = 0; k < truthTable.DimensionRows; k++)
            {
                operationResults[k] = 1;

                for (var m = 0; m < userParameters.OperandsNumber; m++)
                {
                    operationResults[k] *= operationValues[k, m];
                }
            }
            return operationResults;
        }
    }
}
