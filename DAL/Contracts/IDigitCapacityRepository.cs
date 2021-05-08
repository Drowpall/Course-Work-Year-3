namespace DAL.Contracts
{
    public interface IDigitCapacityRepository
    {
        int GetDigitCapacity();

        void SetDigitCapacity(int digitCapacity);
    }
}
