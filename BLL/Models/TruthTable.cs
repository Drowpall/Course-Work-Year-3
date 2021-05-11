namespace BLL.Models
{
    public class TruthTable
    {
        public int DimensionRows { get; private set; }

        public int DimensionResultColumns { get; private set; }

        public int DimensionVariablesColumns { get; private set; }

        public bool[,] VariableValues { get; private set; }

        public bool[,] ResultValues { get; private set; }

        public bool[] ModuleRows { get; private set; }

        public bool[] ModuleCols { get; private set; }

        public TruthTable(int dimensionRows, int dimensionResultColumns, int dimensionVariablesColumns)
        {
            this.DimensionRows = dimensionRows;
            this.DimensionResultColumns = dimensionResultColumns;
            this.DimensionVariablesColumns = dimensionVariablesColumns;

            this.VariableValues = new bool[DimensionRows, DimensionVariablesColumns]; 
            this.ResultValues = new bool[DimensionRows, DimensionResultColumns];
            this.ModuleRows = new bool[DimensionRows];
            this.ModuleCols = new bool[DimensionVariablesColumns];
        }
    }
}
