using BLL.Contracts.IOutput;
using BLL.Models;
using System.IO;

namespace BLL.Services
{
    public class FileOutputReducedService : IOutputReducedService
    {
        public void OutputReducedTruthTable(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions)
        {
            WriteFileUserParameters(outputFile, dimensions);
            WriteFileTruthTableValuesСontracted(outputFile, truthTable);
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

        public void WriteFileUserParameters(StreamWriter outputFile, Dimensions dimensions)
        {
            outputFile.WriteLine($".i {dimensions.DimensionVariablesColumns}");
            outputFile.WriteLine($".o {dimensions.DimensionResultColumns}");
            outputFile.WriteLine($".p {dimensions.DimensionRows}");
        }

    }
}
