using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HGGM.IntegrationTests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Contact")]
        [InlineData("/Home/Privacy")]
        [InlineData("/Home/Error")]
        [InlineData("/swagger")]
        [InlineData("/swagger/v1/swagger.json")]
        [InlineData("/Identity/Account/Register")]
        [InlineData("/Identity/Account/Login")]
        public async Task GetReturnsSuccess(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/hangfire")]
        public async Task UnauthenticatedFails(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}