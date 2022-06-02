using BLL.Contracts.IOutput;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void OutputComplexPolynomialsText(StreamWriter outputFile, IEnumerable<bool[]> resultCols, UserParameters userParameters,
            Dimensions dimensions)
        {
            var numberOfDigits = userParameters.DigitCapacity * userParameters.OperandsNumber;
            
            for (var i = 0; i < resultCols.Count(); i++) // for reach S[i]
            {
                for (var j = 0; j < dimensions.DimensionRows; j++) // for each row
                {
                    if (resultCols.ToList()[i][j]) // If vector's element is 1
                    {
                        var isFirstVariable = true;
                        for (var k = 0; k < numberOfDigits; k++)  // for reach X
                        {
                            if (GetRightNthBit(j, numberOfDigits - k)) // if coef A is 1
                            {
                                if(!isFirstVariable) outputFile.Write("& ");
                                outputFile.Write($"X{k + 1} ");
                                isFirstVariable = false;
                            }
                        }

                        outputFile.WriteLine(" ^");
                    }
                }
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
                        outputFile.Write("^");
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
        
        public void OutputMinimalPolynomialsHdl(StreamWriter outputFile, Matrices matrices, UserParameters userParameters,
            Dimensions dimensions)
        {
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine("// Auto generated code: MinimalPolynomials");
            outputFile.WriteLine("// " + DateTime.Now);
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine();
            outputFile.WriteLine();
            outputFile.WriteLine("module Minimal(");

            var numberOfInVars = userParameters.DigitCapacity * userParameters.OperandsNumber;
            var numberOfOutVars = matrices.minimalPolynomials.Select(vector => vector).Count();
            
            for (var i = 0; i < numberOfInVars; i ++)
            {
                outputFile.WriteLine($"\tinput wire IN{i+1},");
            }
            
            for (var j = 0; j < numberOfOutVars - 1; j ++)
            {
                outputFile.WriteLine($"\toutput wire OUT{j+1},");
            }
            
            outputFile.WriteLine($"\toutput wire OUT{numberOfOutVars}");
            outputFile.WriteLine(");");
            outputFile.WriteLine();

            var vectorNumber = 0;
            foreach (var vector in matrices.minimalPolynomials)
            {
                outputFile.WriteLine($"assign OUT{vectorNumber + 1} =");
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
                            outputFile.Write($"IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"~IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                
                outputFile.WriteLine(";");
                outputFile.WriteLine();
                vectorNumber++;
            }
            
            outputFile.WriteLine("endmodule");
        }
        
        public void OutputShortestPolynomialsHdl(StreamWriter outputFile, Matrices matrices, UserParameters userParameters,
            Dimensions dimensions)
        {
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine("// Auto generated code: ShortestPolynomials");
            outputFile.WriteLine("// " + DateTime.Now);
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine();
            outputFile.WriteLine();
            outputFile.WriteLine("module Shortest(");

            var numberOfInVars = userParameters.DigitCapacity * userParameters.OperandsNumber;
            var numberOfOutVars = matrices.shortestPolynomials.Select(vector => vector).Count();
            
            for (var i = 0; i < numberOfInVars; i ++)
            {
                outputFile.WriteLine($"\tinput wire IN{i+1},");
            }
            
            for (var j = 0; j < numberOfOutVars - 1; j ++)
            {
                outputFile.WriteLine($"\toutput wire OUT{j+1},");
            }
            
            outputFile.WriteLine($"\toutput wire OUT{numberOfOutVars}");
            outputFile.WriteLine(");");
            outputFile.WriteLine();
            
            var vectorNumber = 0;
            foreach (var vector in matrices.shortestPolynomials)
            {
                outputFile.WriteLine($"assign OUT{vectorNumber + 1} =");
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
                            outputFile.Write($"IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"~IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                
                outputFile.WriteLine(";");
                outputFile.WriteLine();
                vectorNumber++;
            }
            
            outputFile.WriteLine("endmodule");
        }

        public void OutputShortestPolynomialsC(StreamWriter outputFile, Matrices matrices, UserParameters userParameters,
            Dimensions dimensions)
        {
            outputFile.WriteLine("#include \"header.h\"\n");
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine("// Auto generated code: ShortestPolynomials");
            outputFile.WriteLine("// " + DateTime.Now);
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine();
            outputFile.WriteLine();
            outputFile.Write("void ShortestPolynomials(");

            var numberOfInVars = userParameters.DigitCapacity * userParameters.OperandsNumber;
            var numberOfOutVars = matrices.shortestPolynomials.Select(vector => vector).Count();
            
            for (var i = 0; i < numberOfInVars; i ++)
            {
                if(i != 0) outputFile.Write(", ");
                outputFile.Write($"bool IN{i+1}");
            }
            
            for (var j = 0; j < numberOfOutVars; j ++)
            {
                outputFile.Write($", bool& OUT{j+1}");
                
                if (j != numberOfOutVars - 1) continue;
                
                outputFile.WriteLine(")");
                outputFile.WriteLine("{");
                
            }

            var vectorNumber = 0;
            foreach (var vector in matrices.shortestPolynomials)
            {
                outputFile.Write($"\tOUT{vectorNumber + 1} = ");
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
                            outputFile.Write($"IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"!IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                
                outputFile.Write(";");
                outputFile.WriteLine("\t");
                vectorNumber++;
            }
            
            outputFile.WriteLine("\treturn;");
            outputFile.WriteLine("}");
        }

        public void OutputMinimalPolynomialsC(StreamWriter outputFile, Matrices matrices, UserParameters userParameters,
            Dimensions dimensions)
        {
            outputFile.WriteLine("#include \"header.h\"\n");
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine("// Auto generated code: MinimalPolynomials");
            outputFile.WriteLine("// " + DateTime.Now);
            outputFile.WriteLine("//////////////////////////////////////////");
            outputFile.WriteLine();
            outputFile.WriteLine();
            outputFile.Write("void MinimalPolynomials(");

            var numberOfInVars = userParameters.DigitCapacity * userParameters.OperandsNumber;
            var numberOfOutVars = matrices.minimalPolynomials.Select(vector => vector).Count();
            
            for (var i = 0; i < numberOfInVars; i ++)
            {
                if(i != 0) outputFile.Write(", ");
                outputFile.Write($"bool IN{i+1}");
            }
            
            for (var j = 0; j < numberOfOutVars; j ++)
            {
                outputFile.Write($", bool& OUT{j+1}");
                
                if (j != numberOfOutVars - 1) continue;
                
                outputFile.WriteLine(")");
                outputFile.WriteLine("{");
                
            }

            var vectorNumber = 0;
            foreach (var vector in matrices.minimalPolynomials)
            {
                outputFile.Write($"\tOUT{vectorNumber + 1} = ");
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
                            outputFile.Write($"IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }
                        else
                        {
                            if (vectorElement != 0 && !GetRightNthBit(vectorElement, i + 1)) continue;
                            
                            if (addSign)
                            {
                                outputFile.Write(" & ");
                            }
                            outputFile.Write($"!IN{userParameters.OperandsNumber * userParameters.DigitCapacity - i}");
                            addSign = true;
                        }

                    }

                    numberOfSuitablePolynomials++;
                }
                
                outputFile.Write(";");
                outputFile.WriteLine("\t");
                vectorNumber++;
            }
            
            outputFile.WriteLine("\treturn;");
            outputFile.WriteLine("}");
        }
        
        public void OutputTestBench(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions)
        {
            throw new NotImplementedException();
        }
    }
}
