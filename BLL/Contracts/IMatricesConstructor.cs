using BLL.Models;

namespace BLL.Contracts
{
    public interface IMatricesConstructor
    {
        Matrices CalculateMatrices(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters);
    }
}
