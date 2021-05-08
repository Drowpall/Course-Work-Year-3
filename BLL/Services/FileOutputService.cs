using BLL.Contracts;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FileOutputService : IOutputService
    {
        public FileOutputService()
        {

        }

        public void OutputTruthTable0(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimension)
        {
            WriteFileNames(outputFile, truthTable, userParameters, dimension);
            WriteFileTruthTableValues(outputFile, truthTable, userParameters);
        }        
        
        public void OutputTruthTable1(StreamWriter outputFile, TruthTable truthTable, Dimensions dimension)
        {
            WriteFileUserParameters(outputFile, dimension);
            WriteFileTruthTableValuesСontracted(outputFile, truthTable);
        }

        public void OutputTruthTable2(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions)
        { 

        }


        private static void WriteFileTruthTableValues(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters)
        {
            for (int i = 0; i < truthTable.DimensionRows; i++)
            {
                for (int j = 0; j < truthTable.DimensionVariablesColumns; j++)
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

                for (int j = 0; j < truthTable.DimensionResultColumns; j++)
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

        private static void WriteFileTruthTableValuesСontracted(StreamWriter outputFile, TruthTable truthTable)
        {
            for (int i = 0; i < truthTable.DimensionRows; i++)
            {
                for (int j = 0; j < truthTable.DimensionVariablesColumns; j++)
                {
                    outputFile.Write(truthTable.VariableValues[i, j].ToInt());
                }

                outputFile.Write(" ");

                for (int j = 0; j < truthTable.DimensionResultColumns; j++)
                {
                    outputFile.Write(truthTable.ResultValues[i, j].ToInt());
                }

                outputFile.WriteLine();
            }
            outputFile.WriteLine($".e");
        }

        private static void WriteFileNames(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimensions)
        {
            WriteFileTruthTableVariableNames(outputFile, userParameters);
            WriteFileTruthTableResultNames(outputFile, dimensions);
        }

        private static void WriteFileTruthTableVariableNames(StreamWriter outputFile, UserParameters userParameters)
        {
            List<string> tableVariables = new List<string>();
            FillListVariableNames(tableVariables, userParameters);

            for (int i = 0; i < userParameters.DigitCapacity * userParameters.OperandsNumber; i++)
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
            List<string> tableResults = new List<string>();
            FillListResultNames(tableResults, dimensions);

            for (int i = 0; i < dimensions.IterationSize; i++)
            {
                outputFile.Write(tableResults[i] + "   ");
            }
            outputFile.WriteLine();
        }

        private static void FillListResultNames(List<string> res, Dimensions dimensions)
        {
            for (int i = 0; i < dimensions.IterationSize; i++)
            {
                res.Add($"S{i}");
            }
        }

        private static void FillListVariableNames(List<string> vars, UserParameters userParameters)
        {
            for (int i = 0; i < userParameters.DigitCapacity * userParameters.OperandsNumber; i++)
            {
                vars.Add($"X{i}");
            }
        }

        private static void WriteFileUserParameters(StreamWriter outputFile, Dimensions dimensions)
        {
            outputFile.WriteLine($".i {dimensions.DimensionVariablesColumns}");
            outputFile.WriteLine($".o {dimensions.DimensionResultColumns}");
            outputFile.WriteLine($".p {dimensions.DimensionRows}");
        }

    }
}
