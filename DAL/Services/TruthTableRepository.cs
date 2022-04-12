using DAL.Contracts;

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
