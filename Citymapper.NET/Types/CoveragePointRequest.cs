using System.Collections.Generic;
using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CoveragePointRequest
    {
        internal CoveragePointRequest() { }

        internal CoveragePointRequest(string givenId, Coordinate givenCoordinate)
        {
            Id = givenId;
            Coords = new List<double> { givenCoordinate.Latitude, givenCoordinate.Longitude };
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("coord")]
        public List<double> Coords { get; set; }
    }
}