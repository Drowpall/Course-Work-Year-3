using BLL.Contracts;
using BLL.Models;
using System.IO;

namespace BLL.Services
{
    public class FileOutputModuleService : IOutputModuleService
    {
        void IOutputModuleService.OutputModuleTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimensions)
        {
            WriteFileUserParametersModule(outputFile, truthTable, dimensions);
            WriteFileTruthTableValuesModule(outputFile, truthTable, dimensions);
        }


        private static void WriteFileTruthTableValuesModule(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions)
        {
            for (int i = 0; i < dimensions.DimensionRows; i++)
            {
                if (truthTable.ModuleRows[i])
                {

                    for (int j = 0; j < dimensions.DimensionVariablesColumns; j++)
                    {
                        if (truthTable.ModuleCols[j])
                        {
                            outputFile.Write(truthTable.VariableValues[i, j].ToInt());
                        }
                    }

                    outputFile.Write(" ");

                    for (int j = 0; j < dimensions.DimensionResultColumns; j++)
                    {
                        outputFile.Write(truthTable.ResultValues[i, j].ToInt());
                    }

                    outputFile.WriteLine();
                }
            }
            outputFile.WriteLine($".e");
        }

        private void WriteFileUserParametersModule(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions)
        {
            outputFile.WriteLine($".i {CountNumberOfModuleCols(truthTable)}");
            outputFile.WriteLine($".o {dimensions.DimensionResultColumns}");
            outputFile.WriteLine($".p {CountNumberOfModuleRows(truthTable)}");
        }

        private int CountNumberOfModuleRows(TruthTable truthTable)
        {
            int count = 0;
            foreach (bool one in truthTable.ModuleRows)
            {
                if (one)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountNumberOfModuleCols(TruthTable truthTable)
        {
            int count = 0;
            foreach (bool one in truthTable.ModuleCols)
            {
                if (one)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
