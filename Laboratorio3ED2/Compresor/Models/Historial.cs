namespace Compresor.Models
{
    public class Historial
    {
        public string OriginalName { get; set; }
        public string CompressedFilePath { get; set; }
        public decimal CompressionRatio { get; set; }
        public decimal CompressionFactor { get; set; }
        public decimal ReductionPercentage { get; set; }
    }
}