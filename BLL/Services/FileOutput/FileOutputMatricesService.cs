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
    public class FileOutputMatricesService : IOutputMatricesService
    {
        public void OutputMatrices(StreamWriter outputFile, Matrices matrices)
        {
            int count = 0;
            foreach (var matrix in matrices.MatricesM)
            {
                outputFile.WriteLine($"Matrix: {++count}");
                for (int i = 0; i < matrices.SquareDimensions; i++)
                {
                    for (int j = 0; j < matrices.SquareDimensions; j++)
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
