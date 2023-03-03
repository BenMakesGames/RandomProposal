using Xunit;

namespace BenMakesGames.Random.Tests;

public class BCryptRandomTests
{
    [Fact]
    public void GetInt_DistributesEvenly()
    {
        TestMethods.GetInt_DistributesEvenly(BCryptRandom.Shared);
    }

    [Fact]
    public void GetByte_ReturnsAllPossibleValues()
    {
        TestMethods.GetByte_ReturnsAllPossibleValues(BCryptRandom.Shared);
    }

}