using System.Collections.Generic;
using BLL.Models;

namespace BLL.Contracts
{
    public interface ITruthTableCalculator
    {
        TruthTable CalculateTruthTable(Dimensions dimensions, UserParameters userParameters);
        IEnumerable<bool[]> CalculateTruthTableComplex(Dimensions dimensions, UserParameters userParameters);
    }
}
