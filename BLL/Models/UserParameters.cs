using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserParameters
    {
        public int OperandsNumber { get; set; }

        public int OperationModule { get; set; }

        public int DigitCapacity { get; set; }

        public Operation Operation { get; set; }
    }
}
