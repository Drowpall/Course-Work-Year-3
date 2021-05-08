using BLL.Contracts;
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.CalculateOperationResults
{
    public class CalculateMultOperationResult : IOperationResultsCalculator
    {
        public Operation Operation => Operation.Mult;

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
            }
            return operationResults;
        }
    }
}
