using Xunit;

namespace BenMakesGames.Random.Tests;

public class TetrisRandomTests
{
    [Fact]
    public void GetInt_DistributesEvenly()
    {
        TestMethods.GetInt_DistributesEvenly(TetrisRandom.Shared);
    }

    [Fact]
    public void GetByte_ReturnsAllPossibleValues()
    {
        TestMethods.GetByte_ReturnsAllPossibleValues(TetrisRandom.Shared);
    }

}