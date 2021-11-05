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
        public void OutputMinimalPolynomialsVectors(StreamWriter outputFile, Matrices matrices)
        {
            int count = 0;
            foreach (var polynomial in matrices.minimalPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                bool isFirstValue = true;
                foreach (var value in polynomial)
                {
                    if (isFirstValue)
                    {
                        outputFile.Write(value);
                        isFirstValue = false;
                    }
                    else
                    {
                        outputFile.Write("," + value);
                    }
                }
                outputFile.WriteLine("].");
            }
            outputFile.WriteLine();
        }

        public void OutputMinimalPolynomialsText(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions)
        {
            outputFile.WriteLine("Shortest polynomials: ");
            int vectorNumber = 0;
            foreach (var vector in matrices.minimalPolynomials)
            {
                outputFile.WriteLine($"S[{vectorNumber}]:");
                int numberOfSuitablePolynoms = 0;

                int[] negativePos = new int[userParameters.OperandsNumber * userParameters.DigitCapacity];
                int polarity = matrices.numbersOfLinesShortest[vectorNumber];
                for (int position = 0; polarity > 0; position++)
                {
                    if (polarity % 2 == 1)
                    {
                        negativePos[position] = 1;
                    }
                    polarity /= 2;
                }
                for (int vectorElement = 0; vectorElement < dimensions.DimensionRows; vectorElement++)
                {
                    bool addSign = false;
                    if (vector[vectorElement] == 1)
                    {
                        if (numberOfSuitablePolynoms != 0)
                        {
                            outputFile.Write(" ^ ");
                            addSign = false;
                        }

                        for (int i = 0; i < userParameters.OperandsNumber * userParameters.DigitCapacity; i++)
                        {
                            if (negativePos[i] == 0)
                            {
                                if (vectorElement == 0  || GetRightNthBit(vectorElement, i + 1))
                                {
                                    if (addSign == true)
                                    {
                                        outputFile.Write(" & ");
                                    }
                                    outputFile.Write($"X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                                    addSign = true;
                                }
                            }
                            else
                            {
                                if (vectorElement == 0  || GetRightNthBit(vectorElement, i + 1))
                                {
                                    if (addSign == true)
                                    {
                                        outputFile.Write(" & ");
                                    }
                                    outputFile.Write($"~X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                                    addSign = true;
                                }
                            }

                        }

                        numberOfSuitablePolynoms++;
                    }
                }
                outputFile.WriteLine();
                vectorNumber++;
            }
        }

        public void OutputShortestPolynomialsVectors(StreamWriter outputFile, Matrices matrices)
        {
            int count = 0;
            foreach (var polynomial in matrices.shortestPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                bool isFirstValue = true;
                foreach (var value in polynomial)
                {
                    if(isFirstValue)
                    {
                        outputFile.Write(value);
                        isFirstValue = false;
                    }
                    else
                    {
                        outputFile.Write("," + value);
                    }
                }
                outputFile.WriteLine("].");
            }
            outputFile.WriteLine();
        }

        public void OutputShortestPolynomialsText(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions)
        {
            outputFile.WriteLine("Minimal polynomials: ");
            int vectorNumber = 0;
            foreach (var vector in matrices.shortestPolynomials)
            {
                outputFile.WriteLine($"S[{vectorNumber}]:");
                int numberOfSuitablePolynoms = 0;

                int[] negativePos = new int[userParameters.OperandsNumber * userParameters.DigitCapacity];
                int polarity = matrices.numbersOfLinesShortest[vectorNumber];
                for (int position = 0; polarity > 0; position++)
                {
                    if(polarity % 2 == 1)
                    {
                        negativePos[position] = 1;
                    }
                    polarity /= 2; 
                }
                for (int vectorElement = 0; vectorElement < dimensions.DimensionRows; vectorElement++)
                {
                    bool addSign = false;

                    if (vector[vectorElement] == 1)
                    {
                        if (numberOfSuitablePolynoms != 0)
                        {
                            outputFile.Write(" ^ ");
                            addSign = false;
                        }

                        for (int i = 0; i < userParameters.OperandsNumber * userParameters.DigitCapacity; i++)
                        {

                            if(negativePos[i] == 0)
                            {
                                if (vectorElement == 0 || GetRightNthBit(vectorElement, i + 1))
                                {
                                    if (addSign == true)
                                    {
                                        outputFile.Write(" & ");
                                    }
                                    outputFile.Write($"X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                                    addSign = true;
                                }
                            }
                            else
                            {
                                if (vectorElement == 0  || GetRightNthBit(vectorElement, i + 1))
                                {
                                    if (addSign == true)
                                    {
                                        outputFile.Write(" & ");
                                    }
                                    outputFile.Write($"~X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                                    addSign = true;
                                }
                            }

                        }

                        numberOfSuitablePolynoms++;
                    }
                }
                outputFile.WriteLine();
                vectorNumber++;
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
