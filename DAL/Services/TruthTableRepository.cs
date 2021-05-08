using DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class TruthTableRepository : ITruthTableRepository
    {
        private bool[,] truthTable;
        public void SetTruthTable(bool[,] truthTable)
        {
            this.truthTable = truthTable;
        }
    }
}
