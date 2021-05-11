using BLL.Models;
using System.IO;

namespace BLL.Contracts.IOutput
{
    public interface IOutputReducedService
    {
        void OutputReducedTruthTable(StreamWriter outputFile, TruthTable truthTable, Dimensions dimension);
    }
}
