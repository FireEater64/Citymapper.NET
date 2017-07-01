using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Citymapper.NET.Types;
using Newtonsoft.Json;

namespace Citymapper.NET
{
    public class Citymapper
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        private readonly string TIME_TRAVEL_ENDPOINT = @"https://developer.citymapper.com/api/1/traveltime/?key={0}&time_type={1}&startcoord={2}&endcoord={3}";
        private readonly string IS_COVERED_ENDPOINT = @"https://developer.citymapper.com/api/1/singlepointcoverage/?key={0}&coord={1}";
        private readonly string MULTIPLE_COVERAGE_ENDPOINT = @"https://developer.citymapper.com/api/1/coverage/?key={0}";

        public Citymapper(string apiKey, HttpClient givenHttpClient = null)
        {
            this._apiKey = apiKey;
            this._httpClient = givenHttpClient ?? new HttpClient();
        }

        public async Task<int> TravelTimeInMinutesAsync(Coordinate startCoordinate, Coordinate endCoordinate, CitymapperTimeType timeType = CitymapperTimeType.Arrival)
        {
            var uri = string.Format(TIME_TRAVEL_ENDPOINT, _apiKey, timeType, startCoordinate, endCoordinate);
            var travelTimeString = await MakeApiRequestAsync(uri, true);
            var travelTimeResponse = JsonConvert.DeserializeObject<TravelTimeResponse>(travelTimeString);
            return travelTimeResponse.TravelTimeMinutes;
        }

        public async Task<bool> IsCoveredAsync(Coordinate givenCoordinate)
        {
            var uri = string.Format(IS_COVERED_ENDPOINT, _apiKey, givenCoordinate);
            var responseString = await MakeApiRequestAsync(uri, true);
            var isCoveredResponse = JsonConvert.DeserializeObject<CoverageResponse>(responseString);
            return isCoveredResponse.Points[0].Covered;
        }

        public async Task<Dictionary<string, bool>> AreCoveredAsync(IDictionary<string, Coordinate> givenCoordinates)
        {
            var uri = string.Format(MULTIPLE_COVERAGE_ENDPOINT, _apiKey);

            var requestedCoveragePoints = givenCoordinates.Select(x => new CoveragePointRequest(x.Key, x.Value)).ToList();
            var requestString = JsonConvert.SerializeObject(new MultipleCoverageRequest(requestedCoveragePoints));

            var responseString = await MakeApiRequestAsync(uri, false, requestString);
            var response = JsonConvert.DeserializeObject<CoverageResponse>(responseString);

            return response.Points.ToDictionary(k => k.Id, v => v.Covered);
        }

        private async Task<string> MakeApiRequestAsync(string uri, bool get, string data = null)
        {
            var response = get ? await _httpClient.GetAsync(uri) : await _httpClient.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            return Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);
        }
    }
}
