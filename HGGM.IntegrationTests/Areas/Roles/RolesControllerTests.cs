using AngleSharp.Dom.Html;
using FluentAssertions;
using HGGM.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HGGM.IntegrationTests.Areas.Roles
{
    public class RolesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly WebApplicationFactory<Startup> _factory;

        public RolesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task EmptyRoleDoesNotRegister()
        {
            var client = _factory.CreateClient();

            // Arrange
            var defaultPage = await client.GetAsync("/roles/Create");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[action=\"/roles/Create\"]"),
                (IHtmlButtonElement)content.QuerySelector("button"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


    }
}
