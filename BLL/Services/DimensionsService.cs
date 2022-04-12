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
            switch (operation)
            {
                case Operation.Sum:
                    var iterationSize = Convert.ToInt32(Math.Ceiling(Math.Log((Math.Pow(2, digitCapacity) - 1) * operandsNumber, 2)));
                    if (digitCapacity == 1)
                    {
                        iterationSize += 1;
                    }
                    return iterationSize;
                case Operation.Sum2:
                    return Convert.ToInt32(Math.Ceiling(Math.Log(operationModule, 2)));
                case Operation.Mult:
                    return Convert.ToInt32(Math.Ceiling(Math.Log(Math.Pow((Math.Pow(2, digitCapacity) - 1), operandsNumber), 2)));
                case Operation.Mult2:
                    return Convert.ToInt32(Math.Ceiling(Math.Log(operationModule, 2)));
                default:
                    return -1;
            }
        }
    }
}