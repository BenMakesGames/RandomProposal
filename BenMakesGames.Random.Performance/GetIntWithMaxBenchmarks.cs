using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2021-03-03, 5:52PM
 *  |                                       Method |      Mean |    Error |   StdDev |
 *  |--------------------------------------------- |----------:|---------:|---------:|
 *  |            XorShift256ImplWrapperGet1000Ints |  30.20 us | 0.516 us | 0.483 us |
 *  |                     FF1RandomImplGet1000Ints |  24.07 us | 0.468 us | 0.415 us |
 *  |               Squirrel3RandomImplGet1000Ints |  35.88 us | 0.699 us | 0.620 us |
 *  |                  BCryptRandomImplGet1000Ints | 158.62 us | 2.342 us | 2.076 us |
 *  |                      SystemRandomGet1000Ints |  18.09 us | 0.075 us | 0.067 us |
 *  | CryptographyRandomNumberGeneratorGet1000Ints | 146.15 us | 2.273 us | 2.126 us |
 */
public class GetIntWithMaxBenchmarks
{
    private static IRandom XorShift256Impl { get; } = XorShift256Random.Shared;
    private static IRandom FF1RandomImpl { get; } = FF1Random.Shared;
    private static IRandom Squirrel3RandomImpl { get; } = Squirrel3Random.Shared;
    private static IRandom BCryptRandomImpl { get; } = BCryptRandom.Shared;
    private static System.Random SystemRandom { get; } = System.Random.Shared;

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