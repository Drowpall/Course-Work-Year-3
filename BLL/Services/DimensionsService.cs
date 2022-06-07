using BLL.Contracts;
using BLL.Models;
using DAL.Models;
using System;

namespace BLL.Services
{
    public class DimensionsService : IDimensionsService
    {
        public Dimensions GetDimensions(UserParameters userParameters)
        {
            var operation = userParameters.Operation;
            var digitCapacity = userParameters.DigitCapacity;
            var operandsNumber = userParameters.OperandsNumber;
            var operationModule = userParameters.OperationModule;

            var iterationSize = GetIterationSize(operation, digitCapacity, operandsNumber, operationModule);
            var dimensionRows = (int)Math.Pow(2, operandsNumber * digitCapacity);

            return new Dimensions
            {
                IterationSize = iterationSize,
                DimensionRows = dimensionRows,
                DimensionVariablesColumns = digitCapacity * operandsNumber,
                DimensionResultColumns = iterationSize
            };
        }

        private static int GetIterationSize(Operation operation, int digitCapacity, int operandsNumber, int operationModule)
        {
            var iterationSizeSum = Convert.ToInt32(Math.Ceiling(Math.Log((Math.Pow(2, digitCapacity) - 1) * operandsNumber, 2)));
            var iterationSizeMult = Convert.ToInt32(Math.Ceiling(Math.Log(Math.Pow((Math.Pow(2, digitCapacity) - 1), operandsNumber), 2)));
            
            switch (operation)
            {
                case Operation.Sum:
                    if (digitCapacity == 1 &&
                        (operandsNumber == 2 ||
                         operandsNumber == 4 ||
                         operandsNumber == 8 ||
                         operandsNumber == 16))
                    {
                        return iterationSizeSum + 1;
                    }

                    return iterationSizeSum;
                case Operation.Sum2:
                    return Math.Min(Convert.ToInt32(Math.Ceiling(Math.Log(operationModule, 2))), iterationSizeSum);
                case Operation.Mult:
                    return digitCapacity == 1 ? 1 : iterationSizeMult;
                case Operation.Mult2:
                    return Math.Min(Convert.ToInt32(Math.Ceiling(Math.Log(operationModule, 2))), iterationSizeMult);
                default:
                    return -1;
            }
        }
    }
}