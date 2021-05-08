using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface ITruthTableCalculator
    {
        TruthTable CalculateTruthTable(Dimensions dimensions, UserParameters userParemeters);
    }
}
