using BLL.Contracts;
using BLL.Models;
using DAL.Contracts;
using DAL.Models;
using System;

namespace BLL.Services
{
    public class DimensionsService : IDimensionsService
    {
        private readonly IOperandsNumberRepository operandsNumberRepository;
        private readonly IOperationModuleRepository operationModuleRepository;
        private readonly IDigitCapacityRepository digitCapacityRepository;
        private readonly IOperationRepository operationRepository;

        public DimensionsService(IOperandsNumberRepository operandsNumberRepository,
                                 IOperationModuleRepository operationModuleRepository, 
                                 IDigitCapacityRepository digitCapacityRepository,
                                 IOperationRepository operationRepository)
        {
            this.operandsNumberRepository = operandsNumberRepository;
            this.operationModuleRepository = operationModuleRepository;
            this.digitCapacityRepository = digitCapacityRepository;
            this.operationRepository = operationRepository;
        }

        public Dimensions GetDimensions()
        {
            var operation = operationRepository.GetOperation();
            var digitCapacity = digitCapacityRepository.GetDigitCapacity();
            var operandsNumber = operandsNumberRepository.GetOperandsNumber();
            var operationModule = operationModuleRepository.GetOperationModule();

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

        private int GetIterationSize(Operation operation, int digitCapacity, int operandsNumber, int operationModule)
        {
            int iterationSize; 

            switch (operation)
            {
                case Operation.Sum:
                    iterationSize = Convert.ToInt32(Math.Ceiling(Math.Log((Math.Pow(2, digitCapacity) - 1) * operandsNumber, 2)));
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