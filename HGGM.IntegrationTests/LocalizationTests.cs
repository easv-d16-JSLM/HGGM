using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HGGM.IntegrationTests
{
    public class LocalizationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public LocalizationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("en", "Welcome.")]
        [InlineData("cs", "Vítej.")]
        public async Task TranslationChanges(string lang, string text)
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.AcceptLanguage.Clear();
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(lang);
            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            (await response.Content.ReadAsStringAsync()).Should().Contain(text);
        }
    }
}