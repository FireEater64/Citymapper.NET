using Newtonsoft.Json;

namespace Citymapper.NET.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TravelTimeResponse
    {
        [JsonProperty("travel_time_minutes")]
        public int TravelTimeMinutes { get; set; }
    }
}