
using FluentAssertions;
using HGGM.Services.Discourse;
using Microsoft.Extensions.Options;
using Xunit;

namespace HGGM.UnitTests.Services.Discourse
{
    public class DiscourseServiceTests
    {
        [Theory]
        [InlineData("bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI%3D%0A", "2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56", "cb68251eefb5211e58c00ff1395f0c0b", "d836444a9e4084d5b224a60c208dce14")]
        public void PayloadExtracted(string sso,string sig,string nonce,string secret)
        {
            var service = new DiscourseService(new OptionsWrapper<DiscourseService.Options>( new DiscourseService.Options(){Secret = secret}));
            var result = service.OpenPayload(sso, sig);
            result.nonce.Should().BeEquivalentTo(nonce);
            result.returnUrl.Should().NotBeEmpty();
        }

        
    }
}