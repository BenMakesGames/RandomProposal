using Xunit;

namespace BenMakesGames.Random.Tests;

public class XorShift256RandomTests
{
    [Fact]
    public void GetInt_DistributesEvenly()
    {
        TestMethods.GetInt_DistributesEvenly(XorShift256Random.Shared);
    }

    [Fact]
    public void GetByte_ReturnsAllPossibleValues()
    {
        TestMethods.GetByte_ReturnsAllPossibleValues(XorShift256Random.Shared);
    }

}