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

        public void EvaluatePolynomialShortest(TruthTable truthTable, Matrices matrices)
        {
            SelectShortestPolynomials(matrices, truthTable);
        }

        public void EvaluatePolynomialMinimal(TruthTable truthTable, Matrices matrices, UserParameters userParameters)
        {
            SelectMinimalPolynomials(truthTable, matrices, userParameters);
        }

        private void SelectMinimalPolynomials(TruthTable truthTable, Matrices matrices, UserParameters userParameters)
        {
            for (int matrix = 0; matrix < matrices.MatricesM.Count; matrix++)
            {
                List<int> XInEveryRow = new List<int>();
                for (int row = 0; row < matrices.SquareDimensions; row++)
                {
                    XInEveryRow.Add(CountNumberOfXInOneLine(matrices, row, matrix, userParameters));
                }
                int minIndex = XInEveryRow.IndexOf(XInEveryRow.Min());
                matrices.numbersOfLinesMinimal.Add(minIndex);
                matrices.minimalPolynomials.Add(GetRow(matrices.MatricesM[matrix], minIndex));
            }
        }

        private int CountNumberOfXInOneLine(Matrices matrices, int row, int matrixNum, UserParameters userParameters)
        {
            int numberOfX = 0;
            var vector = GetRow(matrices.MatricesM[matrixNum], row);
            int[] xTable = new int[userParameters.DigitCapacity * userParameters.OperandsNumber];

            for (int vectorPos = 0; vectorPos < vector.Length; vectorPos++)
            {
                if(vector[vectorPos] == 1)
                {
                    int temp = vectorPos;

                    for (int position = 0; temp > 0; position++)
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
            }

            foreach (var unit in xTable)
            {
                if (unit == 1)
                {
                    numberOfX++;
                }
            }

            return numberOfX;
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
                matrices.numbersOfLinesShortest.Add(minIndex);
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
