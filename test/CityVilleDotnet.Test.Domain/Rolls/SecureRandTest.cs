using AwesomeAssertions;
using CityVilleDotnet.Common.Global;

namespace CityVilleDotnet.Test.Domain.Rolls;

// Expected values computed outside of .NET from the client formula:
// Number("0x" + MD5("YOUR_LIKE_AN_8::" + handshake + "::" + uid + "::" + [feature + "::"] + counter).substring(0, 8)) % (max - min + 1) + min
public class SecureRandTest
{
    [Theory]
    [InlineData(1, 102)]
    [InlineData(2, 116)]
    [InlineData(3, 454)]
    public void SecureRand_GenerateRand_WithFeature_MatchesClientRandPerFeature(int rollCounter, int expected)
    {
        SecureRand.GenerateRand(0, 1000, rollCounter, "333", "lootTables").Should().Be(expected);
    }

    [Fact]
    public void SecureRand_GenerateRand_WithoutFeature_MatchesClientRand()
    {
        SecureRand.GenerateRand(0, 1000, 1, "333").Should().Be(789);
        SecureRand.GenerateRand(0, 99, 1, "333").Should().Be(13);
    }

    [Fact]
    public void SecureRand_GenerateRand_FeatureChangesTheHash()
    {
        SecureRand.GenerateRand(0, 1000, 1, "333", "famousBusinesses").Should().Be(932);
    }
}
