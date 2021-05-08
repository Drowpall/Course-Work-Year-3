using DAL.Models;

namespace DAL.Contracts
{
    public interface IOperationRepository
    {
        Operation GetOperation();

        void SetOperation(Operation op);
    }
}
