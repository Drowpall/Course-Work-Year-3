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
    public class FileOutputPolynomialsService : IOutputPolynomialsService
    {
        public void OutputShortestPolynomials(StreamWriter outputFile, Matrices matrices)
        {
            int count = 0;
            foreach (var polynomial in matrices.shortestPolynomials)
            {
                outputFile.Write($"Vector {++count}: [");
                foreach (var value in polynomial)
                {
                    outputFile.Write(value + ",");
                }
                outputFile.WriteLine("].");
            }
        }
    }
}
