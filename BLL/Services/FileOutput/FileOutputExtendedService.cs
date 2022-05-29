using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BLL.Contracts.IOutput;
using BLL.Models;

namespace BLL.Services.FileOutput
{
    public class FileOutputExtendedService : IOutputExtendedService
    {
        public void OutputExtendedTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimension)
        {
            WriteFileNames(outputFile, userParameters, dimension);
            WriteFileTruthTableValues(outputFile, truthTable, userParameters);
        }

        public void OutputDNF(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters)
        {
            var DNFstrings = CalculateDNF(truthTable, userParameters);
            foreach (var DNF in DNFstrings)
            {
                outputFile.WriteLine(DNF);
            }
        }
        
        private IEnumerable<string> CalculateDNF(TruthTable truthTable, UserParameters userParameters)
        {
            var numberOfCols = truthTable.DimensionResultColumns;
            var numberOfRows = truthTable.ResultValues.GetLength(0);

            var resultStrings = new List<string>(numberOfCols);

            for (var i = 0; i < numberOfCols; i++)
            {
                var vector = new List<int>(numberOfRows);
                for (var j = 0; j < numberOfRows; j++)
                {
                    vector.Add(Convert.ToInt32(truthTable.ResultValues[j, i]));
                }

                resultStrings.Add(ComposeDNF(vector.ToArray(), userParameters.OperandsNumber * userParameters.DigitCapacity));
            }

            return resultStrings;
        }

        private string ComposeDNF(IReadOnlyList<int> vector1, int numOfVars)
        {
            var intList = new List<int>();
            for (var index = 0; index < vector1.Count; ++index)
            {
                if (vector1[index] == 1)
                {
                    intList.Add(index);
                }
            }
            
            var dictionary = new Dictionary<char, int>();
            
            for (var index = 0; index < numOfVars; ++index)
            {
                dictionary.Add(Convert.ToChar(Convert.ToInt16('a') + index), index);
            }
            
            var array = dictionary.Keys.ToArray();
            var stringBuilder = new StringBuilder();
            var num1 = -1;
            
            foreach (var num2 in intList)
            {
                ++num1;
                var str1 = Convert.ToString(num2, 2);
                var str2 = str1;
                if (str1.Length < numOfVars)
                {
                    for (var index = 0; index < numOfVars - str1.Length; ++index)
                    {
                        str2 = "0" + str2;
                    }
                }

                var bclInstance = new BCL.TruthTable(3);
                var vector = bclInstance.StringToVector(str2);
                for (var index = 0; index < vector.Length; ++index)
                {
                    if (vector[index] == 1)
                    {
                        stringBuilder.Append(array[index]);
                    }
                    else
                    {
                        stringBuilder.Append('!');
                        stringBuilder.Append(array[index]);
                    }
                    if (index != vector.Length - 1)
                        stringBuilder.Append(" & ");
                }
                if (num1 < intList.Count - 1)
                    stringBuilder.Append(" ^ ");
            }

            return stringBuilder.ToString();
        }

        private static void WriteFileNames(StreamWriter outputFile, UserParameters userParameters, Dimensions dimensions)
        {
            WriteFileTruthTableVariableNames(outputFile, userParameters);
            WriteFileTruthTableResultNames(outputFile, dimensions);
        }

        private static void WriteFileTruthTableVariableNames(StreamWriter outputFile, UserParameters userParameters)
        {
            var tableVariables = new List<string>();
            FillListVariableNames(tableVariables, userParameters);

            for (var i = 0; i < userParameters.DigitCapacity * userParameters.OperandsNumber; i++)
            {
                if ((i + 1) % userParameters.DigitCapacity == 0)
                {
                    outputFile.Write(tableVariables[i] + "   ");
                }
                else
                {
                    outputFile.Write(tableVariables[i] + "  ");
                }
            }
        }
        private static void WriteFileTruthTableResultNames(StreamWriter outputFile, Dimensions dimensions)
        {
            var tableResults = new List<string>();
            FillListResultNames(tableResults, dimensions);

            for (var i = 0; i < dimensions.IterationSize; i++)
            {
                outputFile.Write(tableResults[i] + "   ");
            }
            outputFile.WriteLine();
        }

        private static void FillListResultNames(ICollection<string> res, Dimensions dimensions)
        {
            for (var i = 0; i < dimensions.IterationSize; i++)
            {
                res.Add($"S{i}");
            }
        }

        private static void FillListVariableNames(ICollection<string> vars, UserParameters userParameters)
        {
            for (var i = 0; i < userParameters.DigitCapacity * userParameters.OperandsNumber; i++)
            {
                vars.Add($"X{i}");
            }
        }

        private static void WriteFileTruthTableValues(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters)
        {
            if (outputFile == null) throw new ArgumentNullException(nameof(outputFile));
            for (var i = 0; i < truthTable.DimensionRows; i++)
            {
                for (var j = 0; j < truthTable.DimensionVariablesColumns; j++)
                {
                    outputFile.Write(truthTable.VariableValues[i, j].ToInt());
                    outputFile.Write("   ");

                    if ((j + 1) % userParameters.DigitCapacity == 0)
                    {
                        outputFile.Write(" ");
                    }

                    if (j > 9)
                    {
                        outputFile.Write(" ");
                    }

                }

                for (var j = 0; j < truthTable.DimensionResultColumns; j++)
                {
                    outputFile.Write((truthTable.ResultValues[i, j]).ToInt());
                    outputFile.Write("    ");

                    if (j > 9)
                    {
                        outputFile.Write(" ");
                    }
                }

                outputFile.WriteLine();
            }
        }

    }
}
