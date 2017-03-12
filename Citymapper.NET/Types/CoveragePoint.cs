using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CoveragePoint
    {
        internal CoveragePoint() { }

        internal CoveragePoint(string givenId, Coordinate givenCoordinate)
        {
            Id = givenId;
            Coords = givenCoordinate;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("covered")]
        public bool Covered { get; set; }

        [JsonProperty("coords")]
        public Coordinate Coords { get; set; }
    }
}