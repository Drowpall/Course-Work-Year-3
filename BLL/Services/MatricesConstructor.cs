using BLL.Contracts;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

namespace BLL.Services
{
    public class MatricesConstructor : IMatricesConstructor
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
            var fVector = new int[matrices.SquareDimensions];
            var binaryMatrixB = new int[matrices.SquareDimensions, matrices.SquareDimensions];

            for (var k = 0; k < truthTable.DimensionResultColumns; k++)
            {
                for (var m = 0; m < matrices.SquareDimensions; m++)
                {
                    fVector[m] = truthTable.ResultValues[m, k] ? 1 : 0;
                }

                for (var i = 0; i < matrices.SquareDimensions / 2; i++)
                {
                    for (var j = 0; j < matrices.SquareDimensions / 2; j++)
                    {
                        binaryMatrixB[i, j] = fVector[(i + j) % (matrices.SquareDimensions / 2)];
                        binaryMatrixB[(matrices.SquareDimensions / 2) + i, (matrices.SquareDimensions / 2) + j] = fVector[(i + j) % (matrices.SquareDimensions / 2)];
                        binaryMatrixB[i, (matrices.SquareDimensions / 2) + j] = fVector[(i + j) % (matrices.SquareDimensions / 2) + (matrices.SquareDimensions / 2)];
                        binaryMatrixB[(matrices.SquareDimensions / 2) + i, j] = fVector[(i + j) % (matrices.SquareDimensions / 2) + (matrices.SquareDimensions / 2)];
                    }
                }

                matrices.MatricesB.Add((int[,])binaryMatrixB.Clone());
            }

        }

        private void ConstructBinaryMatrixZ(Matrices matrices)
        {
            int[,] Z0 = { { 1 } };
            int[,] Z1 = { { 1, 1 }, { 0, 1 } };

            var N = (int)Math.Log(matrices.SquareDimensions, 2);


            if (N < 1)
            {
                throw new ArgumentException();
            }

            if (N == 1)
            {
                matrices.MatrixZ = Z1;
            }

            if (N <= 1) return;
            
            var arrayOfZ = new List<int[,]>
            {
                Z0,
                Z1
            };

            for (var i = 2; i < N + 1; i++)
            {
                arrayOfZ.Add(KroneckerProduct(arrayOfZ[i - 1], Z1));
            }
            matrices.MatrixZ = (arrayOfZ[N]);
        }

        private void ConstructBinaryMatricesM(Matrices matrices, TruthTable truthTable)
        {
            for (var i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                matrices.MatricesM.Add(MultiplyMatrices(matrices.MatricesB[i], matrices.MatrixZ, matrices));
                matrices.MatricesM[i] = ModMatrix(matrices.MatricesM[i], matrices);
            }
        }

        private static int[,] MultiplyMatrices(int[,] A, int[,] B, Matrices matrices)
        {
            var result = new int[matrices.SquareDimensions, matrices.SquareDimensions];

            for (var i = 0; i < matrices.SquareDimensions; ++i)
            {
                for (var j = 0; j < matrices.SquareDimensions; ++j)
                {
                    for (var k = 0; k < matrices.SquareDimensions; ++k)
                    {
                        result[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return result;
        }

        private static int[,] KroneckerProduct(int[,] left, int[,] right)
        {
            var (lRows, lColumns) = (left.GetLength(0), left.GetLength(1));
            var (rRows, rColumns) = (right.GetLength(0), right.GetLength(1));
            var result = new int[lRows * rRows, lColumns * rColumns];

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

        private static int[,] ModMatrix(int[,] matr, Matrices matrices)
        {
            for (var i = 0; i < matrices.SquareDimensions; i++)
            {
                for (var j = 0; j < matrices.SquareDimensions; j++)
                {
                    matr[i, j] %= 2;
                }
            }
            return matr;
        }
    }
}
