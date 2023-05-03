using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BenMakesGames.Random.Performance;

/**
 * 2023-05-03, 0:32 UTC
 * |---------------------------------------------- |------------:|----------:|----------:|
 * |                   XorShift256ImplGet1000Bytes |   173.48 ns |  1.813 ns |  1.695 ns |
 * |                     FF1RandomImplGet1000Bytes |    57.88 ns |  0.940 ns |  0.785 ns |
 * |                  TetrisRandomImplGet1000Bytes | 1,680.28 ns | 11.841 ns | 10.497 ns |
 * |               Squirrel3RandomImplGet1000Bytes |   368.98 ns |  4.939 ns |  4.620 ns |
 * |                  BCryptRandomImplGet1000Bytes |   651.76 ns | 11.480 ns | 10.738 ns |
 * |                      SystemRandomGet1000Bytes |   162.63 ns |  1.635 ns |  1.449 ns |
 * | CryptographyRandomNumberGeneratorGet1000Bytes |   659.03 ns |  5.031 ns |  4.706 ns |
 */
public class FillBytesBenchmarks
{
    private static readonly IRandom XorShift256Impl = XorShift256Random.Shared;
    private static readonly IRandom FF1RandomImpl = FF1Random.Shared;
    private static readonly IRandom TetrisRandomImpl = TetrisRandom.Shared;
    private static readonly IRandom Squirrel3RandomImpl = Squirrel3Random.Shared;
    private static readonly IRandom BCryptRandomImpl = BCryptRandom.Shared;
    private static readonly System.Random SystemRandom = System.Random.Shared;

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
    public Span<byte> TetrisRandomImplGet1000Bytes()
    {
        TetrisRandomImpl.FillBytes(buffer);

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
