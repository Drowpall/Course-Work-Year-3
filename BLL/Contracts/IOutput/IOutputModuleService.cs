using BLL.Models;
using System.IO;

namespace BLL.Contracts
{
    public interface IOutputModuleService
    {
        void OutputModuleTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimensions);
    }
}
