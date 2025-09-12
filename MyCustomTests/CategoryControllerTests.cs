using Application.Response;
using FluentAssertions;
using MyCustomTests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace MyCustomTests
{
    public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CategoryControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        // ---------- CATEGORY TESTS ----------
        [Fact]
        public async Task Get_Should_Return_200_With_All_Categories()
        {
            var response = await _client.GetAsync("api/v1/Category");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
            categories.Should().NotBeNull();
            categories!.Should().NotBeEmpty();
            categories.Should().Contain(category => category.Name == "Pizzas");
        }
    }
}
