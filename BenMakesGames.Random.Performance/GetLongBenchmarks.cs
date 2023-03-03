using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-03-03, 3:13PM
 *  |                             Method |      Mean |     Error |    StdDev |
 *  |----------------------------------- |----------:|----------:|----------:|
 *  | XorShift256ImplWrapperGet1000Longs |  9.204 us | 0.0928 us | 0.0775 us |
 *  |          FF1RandomImplGet1000Longs | 11.752 us | 0.0616 us | 0.0577 us |
 *  |           SystemRandomGet1000Longs |  9.078 us | 0.0637 us | 0.0596 us |
 */
public class GetLongBenchmarks
{
    private static IRandom XorShift256Impl { get; } = XorShift256Random.Shared;
    private static IRandom FF1RandomImpl { get; } = FF1Random.Shared;
    private static IRandom Squirrel3RandomImpl { get; } = Squirrel3Random.Shared;
    private static System.Random SystemRandom { get; } = System.Random.Shared;

    [Benchmark]
    public void XorShift256ImplWrapperGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            XorShift256Impl.GetLong();
    }
    
    [Benchmark]
    public void FF1RandomImplGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            FF1RandomImpl.GetLong();
    }
    
    [Benchmark]
    public void FF1Squirrel3ImplGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            Squirrel3RandomImpl.GetLong();
    }

    [Benchmark]
    public void SystemRandomGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            SystemRandom.NextInt64();
    }
}