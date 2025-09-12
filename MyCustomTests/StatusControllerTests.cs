using Application.Response;
using FluentAssertions;
using MyCustomTests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace MyCustomTests
{
    public class StatusControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StatusControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        // ---------- STATUS TESTS ----------
        [Fact]
        public async Task Get_Should_Return_200_With_All_Statuses()
        {
            var response = await _client.GetAsync("api/v1/Status");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var statuses = await response.Content.ReadFromJsonAsync<List<GenericResponse>>();
            statuses.Should().NotBeNull();
            statuses!.Count.Should().Be(5);

            statuses.Should().Contain(x => x.Id == 1 && x.Name == "Pending");
            statuses.Should().Contain(x => x.Id == 2 && x.Name == "In progress");
            statuses.Should().Contain(x => x.Id == 3 && x.Name == "Ready");
            statuses.Should().Contain(x => x.Id == 4 && x.Name == "Delivery");
            statuses.Should().Contain(x => x.Id == 5 && x.Name == "Closed");

        }
    }
}
