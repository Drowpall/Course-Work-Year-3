using BLL.Contracts;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace BLL.Services
{
    public class MatricesConctructor : IMatricesConstructor
    {

        public Matrices CalculateMatrices(TruthTable truthTable, Dimensions dimensions, UserParameters userParameters)
        {
            var matrices = new Matrices(truthTable);
            ConstructBinaryMatricesB(matrices, truthTable);
            ConstructBinaryMatrixZ(matrices);
            ConstructBinaryMatricesM(matrices, truthTable);

            return matrices;
        }

        private void ConstructBinaryMatricesB(Matrices matrices, TruthTable truthTable)
        {
            int[] f_vector = new int[matrices.SquareDimensions];
            int[,] binary_matrixB = new int[matrices.SquareDimensions, matrices.SquareDimensions];

            for (int k = 0; k < truthTable.DimensionResultColumns; k++)
            {
                for (int m = 0; m < matrices.SquareDimensions; m++)
                {
                    f_vector[m] = truthTable.ResultValues[m, k] ? 1 : 0;
                }

                for (int i = 0; i < matrices.SquareDimensions / 2; i++)
                {
                    for (int j = 0; j < matrices.SquareDimensions / 2; j++)
                    {
                        binary_matrixB[i, j] = f_vector[(i + j) % (matrices.SquareDimensions / 2)];
                        binary_matrixB[(matrices.SquareDimensions / 2) + i, (matrices.SquareDimensions / 2) + j] = f_vector[(i + j) % (matrices.SquareDimensions / 2)];
                        binary_matrixB[i, (matrices.SquareDimensions / 2) + j] = f_vector[(i + j) % (matrices.SquareDimensions / 2) + (matrices.SquareDimensions / 2)];
                        binary_matrixB[(matrices.SquareDimensions / 2) + i, j] = f_vector[(i + j) % (matrices.SquareDimensions / 2) + (matrices.SquareDimensions / 2)];
                    }
                }

                matrices.MatricesB.Add((int[,])binary_matrixB.Clone());
            }

        }

        private void ConstructBinaryMatrixZ(Matrices matrices)
        {
            int[,] Z0 = { { 1 } };
            int[,] Z1 = { { 1, 1 }, { 0, 1 } };

            int N = (int)Math.Log(matrices.SquareDimensions, 2);


            if (N < 1)
            {
                throw new ArgumentException();
            }

            if (N == 1)
            {
                matrices.MatrixZ = Z1;
            }

            if (N > 1)
            {
                List<int[,]> ArrayOfZ = new List<int[,]>
                {
                    Z0,
                    Z1
                };

                for (int i = 2; i < N + 1; i++)
                {
                    ArrayOfZ.Add(KroneckerProduct(ArrayOfZ[i - 1], Z1));
                }
                matrices.MatrixZ = (ArrayOfZ[N]);
            }
        }

        private void ConstructBinaryMatricesM(Matrices matrices, TruthTable truthTable)
        {
            for (int i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                matrices.MatricesM.Add(MultiplyMatrices(matrices.MatricesB[i], matrices.MatrixZ, matrices));
                matrices.MatricesM[i] = ModMatrix(matrices.MatricesM[i], matrices);
            }
        }

        private int[,] MultiplyMatrices(int[,] A, int[,] B, Matrices matrices)
        {
            int[,] result = new int[matrices.SquareDimensions, matrices.SquareDimensions];

            for (int i = 0; i < matrices.SquareDimensions; ++i)
            {
                for (int j = 0; j < matrices.SquareDimensions; ++j)
                {
                    for (int k = 0; k < matrices.SquareDimensions; ++k)
                    {
                        result[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return result;
        }

        private int[,] KroneckerProduct(int[,] left, int[,] right)
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

        private int[,] ModMatrix(int[,] matr, Matrices matrices)
        {
            for (int i = 0; i < matrices.SquareDimensions; i++)
            {
                for (int j = 0; j < matrices.SquareDimensions; j++)
                {
                    matr[i, j] %= 2;
                }
            }
            return matr;
        }
    }
}
