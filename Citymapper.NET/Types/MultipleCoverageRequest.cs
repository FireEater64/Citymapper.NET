using System.Collections.Generic;
using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject]
    internal class MultipleCoverageRequest
    {
        internal MultipleCoverageRequest(List<CoveragePoint> givenPoints)
        {
            Points = givenPoints;
        }

        [JsonProperty("points")]
        private List<CoveragePoint> Points { get; set; }
    }
}