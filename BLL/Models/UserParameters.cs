using DAL.Models;

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
