using BLL.Models;
using System.IO;

namespace BLL.Contracts
{
    public interface IOutputExtendedService
    {
        void OutputExtendedTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimension);
    }
}
