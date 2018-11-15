using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using FluentAssertions;
using HGGM.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HGGM.IntegrationTests.Controllers
{
    public class RolesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public RolesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task EmptyRoleDoesNotRegister()
        {
            var client = _factory.CreateClient();

            // Arrange
            var defaultPage = await client.GetAsync("/roles/Create");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement) content.QuerySelector("form[action=\"/roles/Create\"]"),
                (IHtmlButtonElement) content.QuerySelector("#SubmitCreate"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateRole()
        {
            var client = _factory.CreateClient();

            // Arrange
            var defaultPage = await client.GetAsync("/roles/Create");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var roleName = "Role" + DateTime.Now.ToFileTime();
            var response = await client.SendAsync(
                (IHtmlFormElement) content.QuerySelector("form[action=\"/roles/Create\"]"),
                (IHtmlButtonElement)
                content.GetElementById("SubmitCreate"),
                new Dictionary<string, string>
                {
                    {"Name", roleName}
                });
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            responseString.Should().NotContain("already exists");
            responseString.Should().NotContain("Invalid");
            responseString.Should().NotContain("AppData");
            responseString.Should().NotContain("invalid");
            response.RequestMessage.RequestUri.AbsolutePath.Should().Be("/roles");
            response.IsSuccessStatusCode.Should().BeTrue("because {0} does not indicate success", response.StatusCode,
                response.ReasonPhrase);

            responseString.Should()
                .Contain(roleName, "because we expect the role to be in the list after it's created");
        }
    }
}