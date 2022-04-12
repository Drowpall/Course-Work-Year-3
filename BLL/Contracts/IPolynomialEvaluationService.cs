using BLL.Models;

namespace BLL.Contracts
{
    public interface IPolynomialEvaluationService
    {
        void EvaluatePolynomialShortest(TruthTable truthTable, Matrices matrices);
        void EvaluatePolynomialMinimal(Matrices matrices, UserParameters userParameters);
    }
}
