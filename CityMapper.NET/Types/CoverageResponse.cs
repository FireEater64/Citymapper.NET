using System.Collections.Generic;
using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CoverageResponse
    {
        [JsonProperty]
        public List<CoveragePoint> Points { get; set; }
    }
}