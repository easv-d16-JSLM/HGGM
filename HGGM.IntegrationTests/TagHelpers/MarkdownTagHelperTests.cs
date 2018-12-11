using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace HGGM.IntegrationTests.TagHelpers
{
    public class MarkdownTagHelperTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public MarkdownTagHelperTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            this.output = output;
        }

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper output;

        [Fact]
        public async Task FormatsExampleOnHomePage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Home/Index");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            output.WriteLine(content);
            content.Should().NotContain(@"<markdown>1. [x] Markdown ~~almost~~ **works**!</markdown>");
            content.Should().Contain(@"<del>almost</del>");
        }
    }
}