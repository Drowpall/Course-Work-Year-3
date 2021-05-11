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
        void EvaluatePolynomial(TruthTable truthTable, Matrices matrices);
    }
}
