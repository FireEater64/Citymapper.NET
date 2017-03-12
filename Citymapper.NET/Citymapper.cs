using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Citymapper.NET.Types;
using Newtonsoft.Json;

namespace Citymapper.NET
{
    public class Citymapper
    {
        private string _apiKey;
        private HttpClient _httpClient;

        private readonly string TIME_TRAVEL_ENDPOINT = @"https://developer.citymapper.com/api/1/traveltime/?key={0}&time_type={1}&startcoord={2}&endcoord={3}";
        private readonly string IS_COVERED_ENDPOINT = @"https://developer.citymapper.com/api/1/singlepointcoverage/?key={0}&coord={1}";
        private readonly string MULTIPLE_COVERAGE_ENDPOINT = @"https://developer.citymapper.com/api/1/coverage/?key={0}";

        public Citymapper(string apiKey, HttpClient givenHttpClient = null)
        {
            this._apiKey = apiKey;
            this._httpClient = givenHttpClient ?? new HttpClient();
        }

        public int TravelTimeInMinutes(Coordinate startCoordinate, Coordinate endCoordinate,
            CitymapperTimeType timeType = CitymapperTimeType.Arrival)
        {
            return TravelTimeInMinutesAsync(startCoordinate, endCoordinate, timeType).Result;
        }

        public async Task<int> TravelTimeInMinutesAsync(Coordinate startCoordinate, Coordinate endCoordinate, CitymapperTimeType timeType = CitymapperTimeType.Arrival)
        {
            var uri = string.Format(TIME_TRAVEL_ENDPOINT, _apiKey, timeType, startCoordinate, endCoordinate);
            var travelTimeString = await makeApiRequest(uri, "get");
            var travelTimeResponse = JsonConvert.DeserializeObject<TravelTimeResponse>(travelTimeString);
            return travelTimeResponse.TravelTimeMinutes;
        }

        public bool IsCovered(Coordinate givenCoordinate)
        {
            return IsCoveredAsync(givenCoordinate).Result;
        }

        public async Task<bool> IsCoveredAsync(Coordinate givenCoordinate)
        {
            var uri = string.Format(IS_COVERED_ENDPOINT, _apiKey, givenCoordinate);
            var responseString = await makeApiRequest(uri, "get");
            var isCoveredResponse = JsonConvert.DeserializeObject<CoverageResponse>(responseString);
            return isCoveredResponse.Points[0].Covered;
        }

        public Dictionary<string, bool> AreCovered(IDictionary<string, Coordinate> givenCoordinates)
        {
            return AreCoveredAsync(givenCoordinates).Result;
        }

        public async Task<Dictionary<string, bool>> AreCoveredAsync(IDictionary<string, Coordinate> givenCoordinates)
        {
            var uri = string.Format(MULTIPLE_COVERAGE_ENDPOINT, _apiKey);

            var requestedCoveragePoints = givenCoordinates.Select(x => new CoveragePoint(x.Key, x.Value)).ToList();
            var requestString = JsonConvert.SerializeObject(new MultipleCoverageRequest(requestedCoveragePoints));

            var responseString = await makeApiRequest(uri, "post", requestString);
            var response = JsonConvert.DeserializeObject<CoverageResponse>(responseString);

            return response.Points.ToDictionary(k => k.Id, v => v.Covered);
        }

        private async Task<string> makeApiRequest(string uri, string method, string data = null)
        {
            var response = method == "get" ? await _httpClient.GetAsync(uri) : await _httpClient.PostAsync(uri, new StringContent(data));
            response.EnsureSuccessStatusCode();
            var responseBytes = await _httpClient.GetByteArrayAsync(uri);
            return Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);
        }
    }
}
