namespace DAL.Contracts
{
    public interface IFileModeRepository
    {
        bool TruthTableMode { get; set; }
        bool MinimalTxtMode { get; set; }
        bool ComplexTxtMode { get; set; }
        bool MinimalVMode { get; set; }
        bool ShortestVMode { get; set; }
        bool ComplexVMode { get; set; }
        bool MinimalCppMode { get; set; }
        bool ShortestCppMode { get; set; }
        bool ComplexCppMode { get; set; }
        bool TestBenchMode { get; set; }
    }
}