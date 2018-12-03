using FluentAssertions;
using HGGM.Services.Discourse;
using Microsoft.Extensions.Options;
using Xunit;

namespace HGGM.UnitTests.Services.Discourse
{
    public class DiscourseServiceTests
    {
        [Theory]
        [InlineData("bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n",
            "2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56", "cb68251eefb5211e58c00ff1395f0c0b",
            "d836444a9e4084d5b224a60c208dce14")]
        public void PayloadExtracted(string sso, string sig, string nonce, string secret)
        {
            var service =
                new DiscourseService(new OptionsWrapper<DiscourseService.Options>(new DiscourseService.Options
                    {Secret = secret}));
            var result = service.OpenPayload(sso, sig);
            result.nonce.Should().BeEquivalentTo(nonce);
            result.returnUrl.Should().BeNull();
        }

        [Fact]
        public void PayloadCreated()
        {
            var service = new DiscourseService(new OptionsWrapper<DiscourseService.Options>(new DiscourseService.Options
                {Secret = "d836444a9e4084d5b224a60c208dce14"}));
            var result = service.CreatePayload("cb68251eefb5211e58c00ff1395f0c0b",
                "test@test.com",
                "hello123",
                "samsam",
                "sam",
                emailRequireActivation: true
            );
            result.payload.Should()
                .Be(
                    "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGImZW1haWw9dGVzdCU0MHRlc3QuY29tJmV4dGVybmFsX2lkPWhlbGxvMTIzJnVzZXJuYW1lPXNhbXNhbSZuYW1lPXNhbSZyZXF1aXJlX2FjdGl2YXRpb249dHJ1ZQ==");
            result.signature.Should().Be("19d360ba4bb346c06ec7fd40702960bb60588c997ecdae14e65ffb6298cc33eb");
        }
    }
}