using BLL.Models;
using DAL.Models;

namespace BLL.Contracts
{
    public interface IOperationResultsCalculator
    {
        Operation Operation { get; }

        int[] CalculateOperationResult(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters, int[,] operationValues);
    }
}
