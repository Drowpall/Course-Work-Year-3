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
        public List<int[]> minimalPolynomials { get; private set; }
        public List<int> numbersOfLinesShortest { get; private set; }
        public List<int> numbersOfLinesMinimal { get; private set; }

        public Matrices(TruthTable truthTable)
        {
            this.SquareDimensions = truthTable.DimensionRows;
            this.MatricesB = new List<int[,]>();
            this.MatricesM = new List<int[,]>();
            this.shortestPolynomials = new List<int[]>();
            this.minimalPolynomials = new List<int[]>();
            this.numbersOfLinesShortest = new List<int>();
            this.numbersOfLinesMinimal = new List<int>();
        }
    }
}
