using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-05-03, 01:07 UTC 
 * |---------------------------------- |-----------:|----------:|----------:|
 * | XorShift256ImplWrapperGet1000Ints |   9.540 us | 0.1568 us | 0.1677 us |
 * |          FF1RandomImplGet1000Ints |  14.742 us | 0.2797 us | 0.2872 us |
 * |       TetrisRandomImplGet1000Ints |  19.926 us | 0.3149 us | 0.2945 us |
 * |    Squirrel3RandomImplGet1000Ints |  10.429 us | 0.1114 us | 0.0930 us |
 * |       BCryptRandomImplGet1000Ints | 100.729 us | 0.9985 us | 0.9340 us |
 * |           SystemRandomGet1000Ints |   2.971 us | 0.0150 us | 0.0117 us |
 */
public class GetIntBenchmarks
{
    private static readonly IRandom XorShift256Impl = XorShift256Random.Shared;
    private static readonly IRandom FF1RandomImpl = FF1Random.Shared;
    private static readonly IRandom TetrisRandomImpl = TetrisRandom.Shared;
    private static readonly IRandom Squirrel3RandomImpl = Squirrel3Random.Shared;
    private static readonly IRandom BCryptRandomImpl = BCryptRandom.Shared;
    private static readonly System.Random SystemRandom = System.Random.Shared;

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
    public void TetrisRandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            TetrisRandomImpl.GetInt();
    }

    [Benchmark]
    public void Squirrel3RandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            Squirrel3RandomImpl.GetInt();
    }

    [Benchmark]
    public void BCryptRandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            BCryptRandomImpl.GetInt();
    }

    [Benchmark]
    public void SystemRandomGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            SystemRandom.Next();
    }
}