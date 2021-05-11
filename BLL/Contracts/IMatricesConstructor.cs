using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IMatricesConstructor
    {
        Matrices CalculateMatrices(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters);
    }
}
