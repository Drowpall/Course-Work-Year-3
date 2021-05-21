using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IPolynomialEvaluationService
    {
        void EvaluatePolynomialShortest(TruthTable truthTable, Matrices matrices);
        void EvaluatePolynomialMinimal(TruthTable truthTable, Matrices matrices, UserParameters userParameters);
    }
}
