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
        void OutputShortestPolynomials(StreamWriter outputFile, Matrices matrices);
    }
}
