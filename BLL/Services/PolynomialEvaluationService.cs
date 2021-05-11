using BLL.Contracts;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PolynomialEvaluationService : IPolynomialEvaluationService
    {
        public PolynomialEvaluationService()
        {

        }

        public void EvaluatePolynomial(TruthTable truthTable, Matrices matrices)
        {
            SelectShortestPolynomials(matrices, truthTable);
        }

        private void SelectShortestPolynomials(Matrices matrices, TruthTable truthTable)
        {
            int[] AmountInEveryRow = new int[matrices.SquareDimensions];
            int NumberOfOnes;
            int count = 0;

            for (int i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                for (int j = 0; j < matrices.SquareDimensions; j++)
                {
                    NumberOfOnes = 0;
                    for (int k = 0; k < matrices.SquareDimensions; k++) 
                    {
                        if (matrices.MatricesM[i][j, k] == 1)
                            count++;
                    }

                    if (count >= NumberOfOnes)
                    {
                        NumberOfOnes = count;
                        count = 0;
                    }
                    AmountInEveryRow[j] = NumberOfOnes;
                }
            }


            for (int i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                int minIndex = Array.IndexOf(AmountInEveryRow, AmountInEveryRow.Min());
                matrices.shortestPolynomials.Add(GetRow(matrices.MatricesM[i], minIndex));
            }
        }

        private int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

    }
}
