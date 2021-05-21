using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts.IOutput
{
    public interface IOutputPolynomialsService
    {
        void OutputShortestPolynomialsVectors(StreamWriter outputFile, Matrices matrices);
        void OutputMinimalPolynomialsVectors(StreamWriter outputFile, Matrices matrices);

        void OutputShortestPolynomialsText(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions);
        void OutputMinimalPolynomialsText(StreamWriter outputFile, Matrices matrices, UserParameters userParameters, Dimensions dimensions);
    }
}
