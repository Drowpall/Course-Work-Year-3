using System.IO;
using System.Linq;
using BLL.Contracts;
using BLL.Models;

namespace BLL.Services.FileOutput
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
            for (var i = 0; i < dimensions.DimensionRows; i++)
            {
                if (!truthTable.ModuleRows[i]) continue;
                
                for (var j = 0; j < dimensions.DimensionVariablesColumns; j++)
                {
                    if (truthTable.ModuleCols[j])
                    {
                        outputFile.Write(truthTable.VariableValues[i, j].ToInt());
                    }
                }

                outputFile.Write(" ");

                for (var j = 0; j < dimensions.DimensionResultColumns; j++)
                {
                    outputFile.Write(truthTable.ResultValues[i, j].ToInt());
                }

                outputFile.WriteLine();
            }
            outputFile.WriteLine($".e");
        }

        private void WriteFileUserParametersModule(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions)
        {
            outputFile.WriteLine($".i {CountNumberOfModuleCols(truthTable)}");
            outputFile.WriteLine($".o {dimensions.DimensionResultColumns}");
            outputFile.WriteLine($".p {CountNumberOfModuleRows(truthTable)}");
        }

        private static int CountNumberOfModuleRows(TruthTable truthTable)
        {
            return truthTable.ModuleRows.Count(one => one);
        }

        private static int CountNumberOfModuleCols(TruthTable truthTable)
        {
            return truthTable.ModuleCols.Count(one => one);
        }
    }
}
