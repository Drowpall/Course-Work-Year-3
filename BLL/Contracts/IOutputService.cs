using BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IOutputService
    {
        void OutputTruthTable0(StreamWriter outputFile, TruthTable truthTable, UserParameters userParameters, Dimensions dimension);

        void OutputTruthTable1(StreamWriter outputFile, TruthTable truthTable, Dimensions dimension);

        void OutputTruthTable2(StreamWriter outputFile, TruthTable truthTable, Dimensions dimensions);
    }
}
