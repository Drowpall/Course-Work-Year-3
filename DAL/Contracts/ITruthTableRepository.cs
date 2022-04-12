namespace DAL.Contracts
{
    public interface ITruthTableRepository
    {
        void SetTruthTable(bool[,] truthTable);
    }
}
