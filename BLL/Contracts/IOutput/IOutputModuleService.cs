using System.IO;
using BLL.Models;

namespace BLL.Contracts.IOutput
{
    public interface IOutputModuleService
    {
        void OutputModuleTruthTable(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimensions);
    }
}
