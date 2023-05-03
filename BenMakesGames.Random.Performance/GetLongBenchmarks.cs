using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-05-03, 01:13 UTC
 * |                             Method |       Mean |     Error |    StdDev |
 * |----------------------------------- |-----------:|----------:|----------:|
 * | XorShift256ImplWrapperGet1000Longs |   9.655 us | 0.0986 us | 0.0824 us |
 * |          FF1RandomImplGet1000Longs |  14.893 us | 0.2038 us | 0.1806 us |
 * |       TetrisRandomImplGet1000Longs |  19.614 us | 0.1575 us | 0.1396 us |
 * | FF1Squirrel3RandomImplGet1000Longs |  10.578 us | 0.1351 us | 0.1197 us |
 * |       BCryptRandomImplGet1000Longs | 102.641 us | 1.4966 us | 1.3267 us |
 * |           SystemRandomGet1000Longs |   2.973 us | 0.0295 us | 0.0262 us |
 */
public class GetLongBenchmarks
{
    private static readonly IRandom XorShift256Impl = XorShift256Random.Shared;
    private static readonly IRandom FF1RandomImpl = FF1Random.Shared;
    private static readonly IRandom TetrisRandomImpl = TetrisRandom.Shared;
    private static readonly IRandom Squirrel3RandomImpl = Squirrel3Random.Shared;
    private static readonly IRandom BCryptRandomImpl = BCryptRandom.Shared;
    private static readonly System.Random SystemRandom = System.Random.Shared;

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
    public void TetrisRandomImplGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            TetrisRandomImpl.GetLong();
    }

    [Benchmark]
    public void FF1Squirrel3RandomImplGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            Squirrel3RandomImpl.GetLong();
    }

    [Benchmark]
    public void BCryptRandomImplGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            BCryptRandomImpl.GetLong();
    }

    [Benchmark]
    public void SystemRandomGet1000Longs()
    {
        for(int i = 0; i < 1000; i++)
            SystemRandom.NextInt64();
    }
}