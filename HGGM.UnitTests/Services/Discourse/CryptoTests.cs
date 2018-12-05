using FluentAssertions;
using HGGM.Services.Discourse;
using Xunit;

namespace HGGM.UnitTests.Services.Discourse
{
    public class CryptoTests
    {
        [Theory]
        [InlineData("d836444a9e4084d5b224a60c208dce14", "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n",
            "2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56")]
        public void TestCreateHmacsha256(string secret, string sso, string sig)
        {
            Crypto.CreateHmacsha256(secret, sso).Should().Be(sig);
        }

        [Theory]
        [InlineData("d836444a9e4084d5b224a60c208dce14", "bm9uY2U9Y2I2ODI1MWVlZmI1MjExZTU4YzAwZmYxMzk1ZjBjMGI=\n",
            "2828aa29899722b35a2f191d34ef9b3ce695e0e6eeec47deb46d588d70c7cb56")]
        public void ValidateSignature(string secret, string sso, string sig)
        {
            Crypto.IsSignatureValid(secret, sso, sig).Should().BeTrue();
        }
    }
}