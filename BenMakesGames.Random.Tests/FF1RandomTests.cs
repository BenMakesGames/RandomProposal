using Xunit;

namespace BenMakesGames.Random.Tests;

public class FF1RandomTests
{
    [Fact]
    public void GetInt_DistributesEvenly()
    {
        TestMethods.GetInt_DistributesEvenly(FF1Random.Shared);
    }

    [Fact]
    public void GetByte_ReturnsAllPossibleValues()
    {
        TestMethods.GetByte_ReturnsAllPossibleValues(FF1Random.Shared);
    }

}