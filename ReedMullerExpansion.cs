using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace Course_Work_v1
{
    internal static class ReedMullerExpansion
    {
        #region Array Declarations
        internal static int[,] binary_matrixT;
        internal static int[,] binary_matrixB;
        internal static int[,] binary_matrixZ;
        internal static int[,] binary_matrixM;

        internal static bool[,] truth_table_results = new bool[Calculations.GetDimensionRows(), Calculations.GetDimensionResCols()];
        #endregion
        #region Construct Matrices
        internal static void ConstructBinaryMatrixB(int col)
        {
            int N = Calculations.GetDimensionRows();
            int[] f_vector = new int[N];
            for (int i = 0; i < N; i++)
            {
                f_vector[i] = Convert.ToInt32(truth_table_results[i, col]);
            }

        }
        internal static void ConstructBinaryMatrixM()
        {

        }
        internal static void ConstructBinaryMatrixZ()
        {
            int N = (int)Math.Log(Calculations.GetDimensionRows(), 4);
            int[,] Z1 = { { 1, 1 }, { 0, 1 } };

            if (N < 2)
            {
                throw new ArgumentException();
            }

            if (N == 2)
            {
                SetBinaryMatrixZ(Z1);
            }

            if (N > 2)
            {
                List<int[,]> matrices = new List<int[,]>();
                matrices[1] = Z1;

                for (int i = 2; i < N + 1; i++)
                {
                    matrices[i] = KroneckerProduct(matrices[i - 1], Z1);
                }
                SetBinaryMatrixZ(matrices[N]);
            }
        }
        internal static void ConstructBinaryMatrixT()
        {
            int N = (int)Math.Log(Calculations.GetDimensionRows(), 4);
            int[,] T1 = { { 1, 0 }, { 1, 1 } };

            if (N < 2)
            {
                throw new ArgumentException();
            }
            
            if(N == 2)
            {
                SetBinaryMatrixT(T1);
            }

            if(N > 2)
            {
                List<int[,]> matrices = new List<int[,]>();
                matrices[1] = T1;

                for (int i = 2; i < N + 1; i++)
                {
                    matrices[i] = KroneckerProduct(matrices[i - 1], T1);
                }
                SetBinaryMatrixT(matrices[N]);
            }

        }
        #endregion
        #region Calculations
        internal static int[,] KroneckerProduct(int[,] left, int[,] right)
        {
            (int lRows, int lColumns) = (left.GetLength(0), left.GetLength(1));
            (int rRows, int rColumns) = (right.GetLength(0), right.GetLength(1));
            int[,] result = new int[lRows * rRows, lColumns * rColumns];

            foreach (var (r, c) in from r in Range(0, lRows) from c in Range(0, lColumns) select (r, c))
            {
                Copy(r * rRows, c * rColumns, left[r, c]);
            }
            return result;

            void Copy(int startRow, int startColumn, int multiplier)
            {
                foreach (var (r, c) in from r in Range(0, rRows) from c in Range(0, rColumns) select (r, c))
                {
                    result[startRow + r, startColumn + c] = right[r, c] * multiplier;
                }
            }
        }
        #endregion
        #region Setters
        internal static void SetBinaryMatrixT(int[,] matrix)
        {
            binary_matrixT = matrix;
        }        
        internal static void SetBinaryMatrixB(int[,] matrix)
        {
            binary_matrixB = matrix;
        }        
        internal static void SetBinaryMatrixZ(int[,] matrix)
        {
            binary_matrixZ = matrix;
        }
        internal static void SetBinaryMatrixM(int[,] matrix)
        {
            binary_matrixM = matrix;
        }

        internal static void SetTruthTableResValues(bool[,] res_values)
        {
            truth_table_results = res_values;
        }
        #endregion
    }
}
