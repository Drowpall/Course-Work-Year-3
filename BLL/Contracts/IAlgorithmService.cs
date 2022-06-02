using BLL.Services;

namespace BLL.Contracts
{
    public interface IAlgorithmService
    {
        void AlgorithmMain(AlgorithmService.AlgorithmOperation selectOutputFormat);
        void CleanAlgorithm();
    }
}
