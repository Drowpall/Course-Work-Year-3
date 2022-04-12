using BLL.Contracts.IOutput;
using BLL.Models;
using System;
using System.IO;

namespace BLL.Services.FileOutput
{
    public class FileOutputPolynomialsService : IOutputPolynomialsService
    {
        public void OutputMinimalPolynomialsVectors(StreamWriter outputFile, Matrices matrices)
        {
            var count = 0;
            foreach (var polynomial in matrices.minimalPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                var isFirstValue = true;
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
            var vectorNumber = 0;
            foreach (var vector in matrices.minimalPolynomials)
            {
                outputFile.WriteLine($"S[{vectorNumber}]:");
                var numberOfSuitablePolynomials = 0;

                var negativePos = new int[userParameters.OperandsNumber * userParameters.DigitCapacity];
                var polarity = matrices.numbersOfLinesShortest[vectorNumber];
                for (var position = 0; polarity > 0; position++)
                {
                    if (polarity % 2 == 1)
                    {
                        negativePos[position] = 1;
                    }
                    polarity /= 2;
                }
                for (var vectorElement = 0; vectorElement < dimensions.DimensionRows; vectorElement++)
                {
                    var addSign = false;
                    if (vector[vectorElement] != 1) continue;
                    
                    if (numberOfSuitablePolynomials != 0)
                    {
                        outputFile.Write(" ^ ");
                    }

                    for (var i = 0; i < userParameters.OperandsNumber * userParameters.DigitCapacity; i++)
                    {
                        if (negativePos[i] == 0)
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"~X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                outputFile.WriteLine();
                vectorNumber++;
            }
        }

        public void OutputShortestPolynomialsVectors(StreamWriter outputFile, Matrices matrices)
        {
            var count = 0;
            foreach (var polynomial in matrices.shortestPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                var isFirstValue = true;
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
            var vectorNumber = 0;
            foreach (var vector in matrices.shortestPolynomials)
            {
                outputFile.WriteLine($"S[{vectorNumber}]:");
                var numberOfSuitablePolynomials = 0;

                var negativePos = new int[userParameters.OperandsNumber * userParameters.DigitCapacity];
                var polarity = matrices.numbersOfLinesShortest[vectorNumber];
                for (var position = 0; polarity > 0; position++)
                {
                    if(polarity % 2 == 1)
                    {
                        negativePos[position] = 1;
                    }
                    polarity /= 2; 
                }
                for (var vectorElement = 0; vectorElement < dimensions.DimensionRows; vectorElement++)
                {
                    var addSign = false;

                    if (vector[vectorElement] != 1) continue;
                    
                    if (numberOfSuitablePolynomials != 0)
                    {
                        outputFile.Write(" ^ ");
                    }

                    for (var i = 0; i < userParameters.OperandsNumber * userParameters.DigitCapacity; i++)
                    {

                        if(negativePos[i] == 0)
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"~X{userParameters.OperandsNumber * userParameters.DigitCapacity - i - 1}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                outputFile.WriteLine();
                vectorNumber++;
            }
        }

        private static int GetNumberOfBinaryDigits(int number) => Convert.ToInt32(Math.Log(number, 2) + 1);


        private static bool GetRightNthBit(int val, int n)
        {
            if (n > GetNumberOfBinaryDigits(val))
                return false;
            return (val & (1 << (n - 1))) >> (n - 1) != 0;
        }
    }
}
