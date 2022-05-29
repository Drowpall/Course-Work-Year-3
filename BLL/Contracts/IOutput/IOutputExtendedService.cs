using System.IO;
using BLL.Models;

namespace BLL.Contracts.IOutput
{
    public interface IOutputExtendedService
    {
        void OutputExtendedTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimension);
        void OutputDNF(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters);
    }
}
