using BLL.Contracts;
using BLL.Models;
using DAL.Models;

namespace BLL.Services.CalculateOperationResults
{
    public class CalculateMult2OperationResult : IOperationResultsCalculator
    {
        public Operation Operation => Operation.Mult2;

        public int[] CalculateOperationResult(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters, int[,] operationValues)
        {
            int[] operationResults = new int[dimensions.DimensionRows];
            for (int k = 0; k < truthTable.DimensionRows; k++)
            {
                operationResults[k] = 1;

                for (int m = 0; m < userParameters.OperandsNumber; m++)
                {
                    operationResults[k] *= operationValues[k, m];
                }
                operationResults[k] %= userParameters.OperationModule;
            }
            return operationResults;
        }
    }
}
