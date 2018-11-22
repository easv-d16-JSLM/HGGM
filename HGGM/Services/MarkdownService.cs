using Markdig;

namespace HGGM.Services
{
    public class MarkdownService
    {
        private readonly MarkdownPipeline m = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public string ToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown, m);
        }
    }
}