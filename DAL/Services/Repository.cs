using DAL.Contracts;
using DAL.Models;
using System;

namespace DAL.Services
{
    public class Repository : IOperationRepository, IDigitCapacityRepository, IOperandsNumberRepository, IOperationModuleRepository, IFileModeRepository
    {
        private Operation? operation;

        private int? digitCapacity;

        private int? operandsNumber;

        private int? operationModule;

        private bool fileMode;

        public bool TruthTableMode { get; set; }
        public bool MinimalTxtMode { get; set; }
        public bool ComplexTxtMode { get; set; }
        public bool MinimalVMode { get; set; }
        public bool ShortestVMode { get; set; }
        public bool ComplexVMode { get; set; } = true;
        public bool MinimalCppMode { get; set; }
        public bool ShortestCppMode { get; set; }
        public bool ComplexCppMode { get; set; } = true;
        public bool TestBenchMode { get; set; }

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
            if (digitCapacity.HasValue)
            {
                return digitCapacity.Value;
            }

            throw new Exception("The input digit capacity is not initialized");
        }

        public void SetDigitCapacity(int digitCapacity)
        {
            this.digitCapacity = digitCapacity;
        }

        public int GetOperandsNumber()
        {
            if (operandsNumber.HasValue)
            {
                return operandsNumber.Value;
            }

            throw new Exception("The number of operands is not initialized");
        }

        public void SetOperandsNumber(int operandsNumber)
        {
            this.operandsNumber = operandsNumber;
        }

        public int GetOperationModule()
        {
            if (operationModule.HasValue)
            {
                return operationModule.Value;
            }
            else
                return -1;
        }

        public void SetOperationModule(int operationModule)
        {
            this.operationModule = operationModule;
        }
    }
}
