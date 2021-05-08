using DAL.Contracts;
using DAL.Models;
using System;

namespace DAL.Services
{
    public class Repository : IOperationRepository, IDigitCapacityRepository, IOperandsNumberRepository, IOperationModuleRepository
    {
        private Operation? operation;

        private int digitCapacity;

        private int operandsNumber;

        private int operationModule;

        public Operation GetOperation()
        {
            if (operation.HasValue)
            {
                return operation.Value;
            }

            throw new Exception("The operation is not initialized");
        }

        public void SetOperation(Operation op)
        {
            this.operation = op;
        }

        public int GetDigitCapacity()
        {
            return digitCapacity;
        }

        public void SetDigitCapacity(int digitCapacity)
        {
            this.digitCapacity = digitCapacity;
        }

        public int GetOperandsNumber()
        {
            return operandsNumber;
        }

        public void SetOperandsNumber(int operandsNumber)
        {
            this.operandsNumber = operandsNumber;
        }

        public int GetOperationModule()
        {
            return operationModule;
        }

        public void SetOperationModule(int operationModule)
        {
            this.operationModule = operationModule;
        }
    }
}
