using FluentAssertions;
using HGGM.Services;
using Xunit;

namespace HGGM.UnitTests.Services
{
    public class MarkdownServiceTests
    {
        private readonly MarkdownService _markdownService = new MarkdownService();

        [Theory]
        [InlineData("**b**", "<p><strong>b</strong></p>\n")]
        public void Test(string src, string dst)
        {
            _markdownService.ToHtml(src).Should().Be(dst);
        }
    }
}