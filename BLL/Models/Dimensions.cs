using Newtonsoft.Json;

namespace BLL.Models
{
    public class Dimensions
    {
        public int IterationSize { get; set; }

        public int DimensionRows { get; set; }

        public int DimensionResultColumns { get; set; }

        public int DimensionVariablesColumns { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
