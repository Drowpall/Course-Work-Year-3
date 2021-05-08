using BLL.Models;

namespace BLL.Contracts
{
    public interface IDimensionsService
    {
        Dimensions GetDimensions(UserParameters userParameters);
    }
}
