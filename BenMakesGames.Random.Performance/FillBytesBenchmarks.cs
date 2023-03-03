using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-03-03, 5:29PM 
 *  |                          Method |      Mean |    Error |   StdDev |
 *  |-------------------------------- |----------:|---------:|---------:|
 *  |     XorShift256ImplGet1000Bytes | 166.47 ns | 0.872 ns | 0.773 ns |
 *  |       FF1RandomImplGet1000Bytes |  57.04 ns | 1.018 ns | 0.850 ns |
 *  | Squirrel3RandomImplGet1000Bytes | 231.06 ns | 2.352 ns | 2.200 ns |
 *  |        SystemRandomGet1000Bytes | 156.53 ns | 0.609 ns | 0.570 ns |
 */
public class FillBytesBenchmarks
{
    private static IRandom XorShift256Impl { get; } = XorShift256Random.Shared;
    private static IRandom FF1RandomImpl { get; } = FF1Random.Shared;
    private static IRandom Squirrel3RandomImpl { get; } = Squirrel3Random.Shared;
    private static IRandom BCryptRandomImpl { get; } = BCryptRandom.Shared;
    private static System.Random SystemRandom { get; } = System.Random.Shared;

    public byte[] buffer = new byte[1000];
    
    [Benchmark]
    public Span<byte> XorShift256ImplGet1000Bytes()
    {
        XorShift256Impl.FillBytes(buffer);

        return buffer;
    }
    
    [Benchmark]
    public Span<byte> FF1RandomImplGet1000Bytes()
    {
        FF1RandomImpl.FillBytes(buffer);

        return buffer;
    }
    
    [Benchmark]
    public Span<byte> Squirrel3RandomImplGet1000Bytes()
    {
        Squirrel3RandomImpl.FillBytes(buffer);

        return buffer;
    }
    
    [Benchmark]
    public Span<byte> BCryptRandomImplGet1000Bytes()
    {
        BCryptRandomImpl.FillBytes(buffer);

        return buffer;
    }
    
    [Benchmark]
    public Span<byte> SystemRandomGet1000Bytes()
    {
        SystemRandom.NextBytes(buffer);

        return buffer;
    }
    
    [Benchmark]
    public Span<byte> CryptographyRandomNumberGeneratorGet1000Bytes()
    {
        RandomNumberGenerator.Fill(buffer);
        System.Random.Shared.Next(100);

        return buffer;
    }
}
