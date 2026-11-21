using AwesomeAssertions;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.GameEntities;
using FluorineFx;

namespace CityVilleDotnet.Test.Integration.Common;

public class AmfConverterTest
{
    [Fact]
    public void AmfConverter_Convert_NullPropertyIgnoredWhenWritingNull_OmitsTheKey()
    {
        var helper = new ViralAckHelperDto { Zid = "101", CrewPos = 0, Ts = 10 };

        var result = AmfConverter.Convert(helper).Should().BeOfType<ASObject>().Subject;

        result.Should().ContainKeys("zid", "crewPos", "ts");
        result.Should().NotContainKey("itemName");
        result.Should().NotContainKey("targetObjectName");
    }

    [Fact]
    public void AmfConverter_Convert_NullPropertyWithoutIgnore_KeepsTheKeyWithNull()
    {
        var friends = new MfsDataDto { AppFriends = null! };

        var result = AmfConverter.Convert(friends).Should().BeOfType<ASObject>().Subject;

        result.Should().ContainKey("appFriends").WhoseValue.Should().BeNull();
    }
}
