using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class Matrices
    {
        public int SquareDimensions { get; private set; }
        public List<int[,]> MatricesB { get; private set; }
        public List<int[,]> MatricesM { get; private set; }

        public int[,] MatrixZ { get; set; }

        public List<int[]> shortestPolynomials { get; private set; }

        public Matrices(TruthTable truthTable)
        {
            this.SquareDimensions = truthTable.DimensionRows;
            this.MatricesB = new List<int[,]>();
            this.MatricesM = new List<int[,]>();
            this.shortestPolynomials = new List<int[]>();
        }
    }
}
