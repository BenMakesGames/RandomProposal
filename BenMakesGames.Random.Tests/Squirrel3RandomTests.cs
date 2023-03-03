using Xunit;

namespace BenMakesGames.Random.Tests;

public class Squirrel3RandomTests
{
    [Fact]
    public void GetInt_DistributesEvenly()
    {
        TestMethods.GetInt_DistributesEvenly(Squirrel3Random.Shared);
    }

    [Fact]
    public void GetByte_ReturnsAllPossibleValues()
    {
        TestMethods.GetByte_ReturnsAllPossibleValues(Squirrel3Random.Shared);
    }
}