using Citymapper.NET.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Citymapper.NET.Test
{
    [TestClass]
    public class TestTravelTimeResponse
    {
        [TestMethod]
        public void TestTravelTimeResponse_WithValidJsonReturned_CanBeDeserializedSuccessfully()
        {
            // Arrange
            const string response = "{\"travel_time_minutes\": 42}";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://developer.citymapper.com/api/1/traveltime/*").Respond("application/json", response);

            var underTest = new CityMapper("fake-api-key", mockHttp.ToHttpClient());

            // Act
            var travelTime = underTest.TravelTimeInMinutesAsync(new Coordinate(), new Coordinate());

            // Assert
            // TODO: Possibly not best practice
            Assert.AreEqual(travelTime.Result, 42);
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}