using BLL.Contracts.IOutput;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FileOutput
{
    public class FileOutputPolynomialsService : IOutputPolynomialsService
    {
        public void OutputShortestPolynomials(StreamWriter outputFile, Matrices matrices)
        {
            int count = 0;
            foreach (var polynomial in matrices.shortestPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                foreach (var value in polynomial)
                {
                    outputFile.Write(value + ",");
                }
                outputFile.WriteLine("].");
            }
            outputFile.WriteLine();
        }

        public void OutputShortestPolynomialsText(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions)
        {
            foreach (var vector in matrices.shortestPolynomials)
            {
                int numberOfSuitablePolynoms = 0;
                for (int vectorElement = 0; vectorElement < dimensions.DimensionRows; vectorElement++)
                {
                    if (vector[vectorElement] == 1)
                    {
                        if (numberOfSuitablePolynoms != 0)
                        {
                            outputFile.Write(" \u2295 ");
                        }

                        for (int i = 0; i < userParameters.OperandsNumber * userParameters.DigitCapacity; i++)
                        {
                            if (GetRightNthBit(vectorElement, i))
                            outputFile.Write($"X{i}");
                        }

                        numberOfSuitablePolynoms++;
                    }
                }
                outputFile.WriteLine();
            }
        }

        private int getNumberOfBinaryDigits(int number) => Convert.ToInt32(Math.Log(number, 2) + 1);


        private bool GetRightNthBit(int val, int n)
        {
            if (n > getNumberOfBinaryDigits(val))
                return false;
            else
                return ((val & (1 << (n - 1))) >> (n - 1)) != 0;
        }
    }
}
