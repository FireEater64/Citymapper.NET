using System.Collections.Generic;
using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject]
    internal class MultipleCoverageRequest
    {
        internal MultipleCoverageRequest(List<CoveragePointRequest> givenPoints)
        {
            Points = givenPoints;
        }

        [JsonProperty("points")]
        private List<CoveragePointRequest> Points { get; set; }
    }
}