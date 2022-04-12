using BLL.Models;

namespace BLL.Contracts
{
    public interface ITruthTableCalculator
    {
        TruthTable CalculateTruthTable(Dimensions dimensions, UserParameters userParameters);
    }
}
