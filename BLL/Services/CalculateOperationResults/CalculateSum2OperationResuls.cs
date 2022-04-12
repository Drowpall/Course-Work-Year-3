using BLL.Contracts;
using BLL.Models;
using DAL.Models;

namespace BLL.Services.CalculateOperationResults
{
    public class CalculateSum2OperationResuls : IOperationResultsCalculator
    {
        public Operation Operation => Operation.Sum2;

        public int[] CalculateOperationResult(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters, int[,] operationValues)
        {
            var operationResults = new int[dimensions.DimensionRows];
            for (var k = 0; k < truthTable.DimensionRows; k++)
            {
                for (var m = 0; m < userParameters.OperandsNumber; m++)
                {
                    operationResults[k] += operationValues[k, m];
                }
                operationResults[k] %= userParameters.OperationModule;
            }
            return operationResults;
        }
    }
}
