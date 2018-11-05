using AngleSharp.Dom.Html;
using FluentAssertions;
using HGGM.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HGGM.IntegrationTests.Areas.Identity.Pages.Account.Manage
{
    public class RegisterTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public RegisterTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task EmptyUserDoesnotRegister()
        {
            var client = _factory.CreateClient();

            // Arrange
            var defaultPage = await client.GetAsync("/identity/account/register");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[action=\"/Identity/Account/Register\"]"),
                (IHtmlButtonElement)content.QuerySelector("button"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task UserRegistration()
        {
            var client = _factory.CreateClient();

            // Arrange
            var defaultPage = await client.GetAsync("/identity/account/register");
            defaultPage.EnsureSuccessStatusCode();
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[action=\"/Identity/Account/Register\"]"),
                (IHtmlButtonElement)//content.QuerySelector("button[id=\"SubmitRegister\"]")
                content.GetElementById("SubmitRegister"), 
                new Dictionary<string, string> { {"Input_Email", DateTime.Now.ToFileTimeUtc().ToString()+"@now.now"},
                    {"Input_UserName", DateTime.Now.ToFileTimeUtc().ToString()},
                    {"Input_Password", "@Asd123" },
                    {"Input_ConfirmPassword", "@Asd123" },
                    {"Input_DateOfBirth", "hej" } });
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            responseString.Should().NotContain("already exists");
            response.RequestMessage.RequestUri.AbsolutePath.Should().Be("/");

        }
    }
}
