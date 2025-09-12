using Application.Response;
using FluentAssertions;
using MyCustomTests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace MyCustomTests
{
    public class DeliveryTypeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DeliveryTypeTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        // ---------- DELIVERY TYPE TESTS ----------
        [Fact]
        public async Task Get_Should_Return_200_With_All_DeliveryTypes()
        {
            var response = await _client.GetAsync("api/v1/DeliveryType");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var deliveryTypes = await response.Content.ReadFromJsonAsync<List<GenericResponse>>();
            deliveryTypes.Should().NotBeNull();
            deliveryTypes.Should().NotBeEmpty();
            deliveryTypes!.Count.Should().BeGreaterThanOrEqualTo(3);

            deliveryTypes.Should().Contain(x => x.Id == 1 && x.Name == "Delivery");
            deliveryTypes.Should().Contain(x => x.Id == 2 && x.Name == "Take away");
            deliveryTypes.Should().Contain(x => x.Id == 3 && x.Name == "Dine in");
        }
    }
}
