using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-05-03, 01:11 UTC
 * |                                       Method |       Mean |     Error |    StdDev |
 * |--------------------------------------------- |-----------:|----------:|----------:|
 * |            XorShift256ImplWrapperGet1000Ints |  31.449 us | 0.3424 us | 0.3036 us |
 * |                     FF1RandomImplGet1000Ints |  25.479 us | 0.1812 us | 0.1607 us |
 * |                  TetrisRandomImplGet1000Ints |  30.972 us | 0.2292 us | 0.1914 us |
 * |               Squirrel3RandomImplGet1000Ints |  23.561 us | 0.3020 us | 0.2522 us |
 * |                  BCryptRandomImplGet1000Ints | 156.154 us | 1.0611 us | 0.8861 us |
 * |                      SystemRandomGet1000Ints |   7.989 us | 0.0914 us | 0.0810 us |
 * | CryptographyRandomNumberGeneratorGet1000Ints | 145.993 us | 1.3334 us | 1.1820 us |
 */
public class GetIntWithMaxBenchmarks
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
            XorShift256Impl.GetInt(20000);
    }
    
    [Benchmark]
    public void FF1RandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            FF1RandomImpl.GetInt(20000);
    }
    
    [Benchmark]
    public void TetrisRandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            TetrisRandomImpl.GetInt(20000);
    }

    [Benchmark]
    public void Squirrel3RandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            Squirrel3RandomImpl.GetInt(20000);
    }
    
    [Benchmark]
    public void BCryptRandomImplGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            BCryptRandomImpl.GetInt(20000);
    }
    
    [Benchmark]
    public void SystemRandomGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            SystemRandom.Next(20000);
    }
    
    [Benchmark]
    public void CryptographyRandomNumberGeneratorGet1000Ints()
    {
        for(int i = 0; i < 1000; i++)
            RandomNumberGenerator.GetInt32(20000);
    }
}