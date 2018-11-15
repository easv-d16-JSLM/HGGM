using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace HGGM.IntegrationTests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper output;

        public StartupTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            this.output = output;
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
            var content = await response.Content.ReadAsStringAsync();
            output.WriteLine(content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/hangfire")]
        public async Task UnauthenticatedFails(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            output.WriteLine(await response.Content.ReadAsStringAsync());
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}