using System;
using System.Collections.Generic;
using System.IO;
using BLL.Contracts;
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
