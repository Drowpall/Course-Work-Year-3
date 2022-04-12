using BLL.Contracts;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class PolynomialEvaluationService : IPolynomialEvaluationService
    {
        public void EvaluatePolynomialShortest(TruthTable truthTable, Matrices matrices)
        {
            SelectShortestPolynomials(matrices, truthTable);
        }

        public void EvaluatePolynomialMinimal(Matrices matrices, UserParameters userParameters)
        {
            SelectMinimalPolynomials(matrices, userParameters);
        }

        private void SelectMinimalPolynomials(Matrices matrices, UserParameters userParameters)
        {
            for (var matrix = 0; matrix < matrices.MatricesM.Count; matrix++)
            {
                var xInEveryRow = new List<int>();
                for (var row = 0; row < matrices.SquareDimensions; row++)
                {
                    xInEveryRow.Add(CountNumberOfXInOneLine(matrices, row, matrix, userParameters));
                }
                var minIndex = xInEveryRow.IndexOf(xInEveryRow.Min());
                matrices.numbersOfLinesMinimal.Add(minIndex);
                matrices.minimalPolynomials.Add(GetRow(matrices.MatricesM[matrix], minIndex));
            }
        }

        private int CountNumberOfXInOneLine(Matrices matrices, int row, int matrixNum, UserParameters userParameters)
        {
            var vector = GetRow(matrices.MatricesM[matrixNum], row);
            var xTable = new int[userParameters.DigitCapacity * userParameters.OperandsNumber];

            for (var vectorPos = 0; vectorPos < vector.Length; vectorPos++)
            {
                if (vector[vectorPos] != 1) continue;
                
                var temp = vectorPos;

                for (var position = 0; temp > 0; position++)
                {
                    if (temp % 2 == 1)
                    {
                        if(xTable[position] == 0)
                        {
                            xTable[position] = 1;
                        }
                    }
                    temp /= 2;
                }
            }

            return xTable.Count(unit => unit == 1);
        }

        private void SelectShortestPolynomials(Matrices matrices, TruthTable truthTable)
        {
            var amountInEveryRow = new int[matrices.SquareDimensions];
            var count = 0;

            for (var i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                for (var j = 0; j < matrices.SquareDimensions; j++)
                {
                    var numberOfOnes = 0;
                    for (var k = 0; k < matrices.SquareDimensions; k++) 
                    {
                        if (matrices.MatricesM[i][j, k] == 1)
                            count++;
                    }

                    if (count >= numberOfOnes)
                    {
                        numberOfOnes = count;
                        count = 0;
                    }
                    amountInEveryRow[j] = numberOfOnes;
                }
            }


            for (var i = 0; i < truthTable.DimensionResultColumns; i++)
            {
                var minIndex = Array.IndexOf(amountInEveryRow, amountInEveryRow.Min());
                matrices.shortestPolynomials.Add(GetRow(matrices.MatricesM[i], minIndex));
                matrices.numbersOfLinesShortest.Add(minIndex);
            }
        }

        private static int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
