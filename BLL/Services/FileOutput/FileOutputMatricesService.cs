using BLL.Contracts.IOutput;
using BLL.Models;
using System.IO;

namespace BLL.Services.FileOutput
{
    public class FileOutputMatricesService : IOutputMatricesService
    {
        public void OutputMatrices(StreamWriter outputFile, Matrices matrices)
        {
            var count = 0;
            foreach (var matrix in matrices.MatricesM)
            {
                outputFile.WriteLine($"Matrix: {++count}");
                for (var i = 0; i < matrices.SquareDimensions; i++)
                {
                    for (var j = 0; j < matrices.SquareDimensions; j++)
                    {
                        outputFile.Write(matrix[i, j]);
                        if (j == matrices.SquareDimensions - 1)
                        {
                            outputFile.WriteLine();
                        }
                    }
                }
                outputFile.WriteLine();
            }
        }
    }
}
