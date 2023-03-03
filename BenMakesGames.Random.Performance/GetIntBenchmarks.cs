using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

public class GetIntBenchmarks
{
    private static IRandom XorShift256Impl { get; } = XorShift256Random.Shared;
    private static IRandom FF1RandomImpl { get; } = FF1Random.Shared;
    private static IRandom Squirrel3RandomImpl { get; } = Squirrel3Random.Shared;
    private static System.Random SystemRandom { get; } = System.Random.Shared;

    [Benchmark]
    public void XorShift256ImplWrapperGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            XorShift256Impl.GetInt();
    }
    
    [Benchmark]
    public void FF1RandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            FF1RandomImpl.GetInt();
    }
    
    [Benchmark]
    public void SystemRandomGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            SystemRandom.Next();
    }
}