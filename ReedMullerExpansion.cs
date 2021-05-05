using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static System.Linq.Enumerable;

namespace Course_Work_v1
{
    internal static class ReedMullerExpansion
    {
        #region Array & Variables Declarations
        private static int sq_dim;
        private static int res_cols;
        private static List<int[,]> matrices_B = new List<int[,]>();
        private static List<int[,]> matrices_M = new List<int[,]>();
        private static List<int[]> shortestPolynomials = new List<int[]>();
        private static int[,] binary_matrixZ;
        

        internal static bool[,] truth_table_results = new bool[Calculations.GetDimensionRows(), Calculations.GetDimensionResCols()];
        #endregion
        #region Matrices Construction
        internal static void ConstructBinaryMatricesB()
        {
            int[] f_vector = new int[sq_dim];
            int[,] binary_matrixB = new int[sq_dim,sq_dim];

            for (int k = 0; k < res_cols; k++)
            {
                for (int m = 0; m < sq_dim; m++)
                {
                    f_vector[m] = truth_table_results[m, k] ? 1 : 0;
                }

                for (int i = 0; i < sq_dim / 2; i++)
                {
                    for (int j = 0; j < sq_dim / 2; j++)
                    {
                        binary_matrixB[i, j] = f_vector[(i + j) % (sq_dim / 2)];
                        binary_matrixB[(sq_dim / 2) + i, (sq_dim / 2) + j] = f_vector[(i + j) % (sq_dim / 2)];
                        binary_matrixB[i, (sq_dim / 2) + j] = f_vector[(i + j) % (sq_dim / 2) + (sq_dim / 2)];
                        binary_matrixB[(sq_dim / 2) + i, j] = f_vector[(i + j) % (sq_dim / 2) + (sq_dim / 2)];
                    }
                }
              
                matrices_B.Add((int[,])binary_matrixB.Clone());
            }

        }
        internal static void ConstructBinaryMatricesM()
        {
            for (int i = 0; i < res_cols; i++)
            {
                matrices_M.Add(MultiplyMatrices(matrices_B[i], binary_matrixZ));
                matrices_M[i] = ModMatrix(matrices_M[i]);
            }

        }
        internal static void ConstructBinaryMatrixZ()
        {
            int[,] Z0 = { { 1 } };
            int[,] Z1 = { { 1, 1 }, { 0, 1 } };
            int N = (int)Math.Log(sq_dim, 2);
            if (N < 1)
            {
                throw new ArgumentException();
            }

            if (N == 1)
            {
                SetBinaryMatrixZ(Z1);
            }

            if (N > 1)
            {
                List<int[,]> matrices = new List<int[,]>();
                matrices.Add(Z0);
                matrices.Add(Z1);

                for (int i = 2; i < N + 1; i++)
                {
                    matrices.Add(KroneckerProduct(matrices[i - 1], Z1));
                }
                SetBinaryMatrixZ(matrices[N]);
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

        internal static int[,] MultiplyMatrices(int[,] A, int[,] B)
        {
            int[,] result = new int[sq_dim, sq_dim];

            for (int i = 0; i < sq_dim; ++i)
            {
                for (int j = 0; j < sq_dim; ++j)
                {
                    for (int k = 0; k < sq_dim; ++k)
                    {
                        result[i,j] += A[i,k] * B[k,j];
                    }
                }
            }
            return result;
        }

        internal static int[,] ModMatrix(int[,] matr)
        {
            for (int i = 0; i < sq_dim; i++)
            {
                for (int j = 0; j < sq_dim; j++)
                {
                    matr[i, j] %= 2;
                }
            }
            return matr;
        }

        internal static void ProcessMatricesM()
        {
            int[] amount_in_every_row = new int[sq_dim];
            int number_of_ones;
            int count = 0;

            for (int i = 0; i < res_cols; i++)         // in every matrix    
            {
                for (int j = 0; j < sq_dim; j++)       // in every row
                {
                    number_of_ones = 0;
                    for (int k = 0; k < sq_dim; k++)   // on each position
                    {
                        if (matrices_M[i][j, k] == 1)   
                            count++;
                    }

                    if(count >= number_of_ones)
                    {
                        number_of_ones = count;
                        count = 0;
                    }
                    amount_in_every_row[j] = number_of_ones;
                }
            }


            for (int i = 0; i < res_cols; i++)
            {
                int minIndex = Array.IndexOf(amount_in_every_row, amount_in_every_row.Min());
                shortestPolynomials.Add(GetRow(matrices_M[i], minIndex));
            }
        }

        public static int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        #endregion
        #region Setters 
        internal static void SetBinaryMatrixZ(int[,] matrix)
        {
            binary_matrixZ = matrix;
        }
        internal static void SetDimension(int dim, int cols)
        {
            sq_dim = dim;
            res_cols = cols;
        }
        internal static void SetTruthTableResValues(bool[,] res_values)
        {
            truth_table_results = res_values;
        }
        #endregion
        #region WriteFile
        private static void WriteFile_MatricesM(StreamWriter outputFile)
        {
            int count = 0;
            foreach (var matrix in matrices_M)
            {
                outputFile.WriteLine($"Matrix: {++count}");
                for (int i = 0; i < sq_dim; i++)
                {
                    for (int j = 0; j < sq_dim; j++)
                    {
                        outputFile.Write(matrix[i, j]);
                        if (j == sq_dim - 1)
                        {
                            outputFile.WriteLine();
                        }
                    }
                }
                outputFile.WriteLine();
            }
        }

        private static void WriteFile_ShortestPolynomials(StreamWriter outputFile)
        {
            int count = 0;
            foreach (var polynomial in shortestPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                foreach (var value in polynomial)
                {
                    outputFile.Write(value + ",");
                }
                outputFile.WriteLine("].");
            }
        }

        #endregion

        #region Main
        internal static void EvaluatePolynomial()
        {
            SetDimension(Calculations.GetDimensionRows(), Calculations.GetDimensionResCols());
            ConstructBinaryMatrixZ();
            ConstructBinaryMatricesB();
            ConstructBinaryMatricesM();
            ProcessMatricesM();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Globals.docPath, "Polynomials.txt")))
            {
                WriteFile_MatricesM(outputFile);
                WriteFile_ShortestPolynomials(outputFile);
            }
        }
        #endregion
    }        
}
