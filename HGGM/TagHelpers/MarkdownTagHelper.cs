using System.Threading.Tasks;
using HGGM.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HGGM.TagHelpers
{
    [HtmlTargetElement("markdown", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MarkdownTagHelper : TagHelper
    {
        private readonly MarkdownService _markdownService;

        public MarkdownTagHelper(MarkdownService markdownService)
        {
            this._markdownService = markdownService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "content");
            var content = output.Content.IsModified
                ? output.Content.GetContent()
                : (await output.GetChildContentAsync()).GetContent();
            output.Content.SetHtmlContent(_markdownService.ToHtml(content));
        }
    }
}