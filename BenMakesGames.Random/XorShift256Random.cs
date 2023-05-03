using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

/// <summary>
/// Uses XorShift algorithm, which is super-fast.
///
/// It is suitable when a large number of random numbers is needed, or in other contexts where it is desirable to keep
/// CPU usage to a minimum.
///
/// This RNG is NOT suitable for use by cryptographic methods, or other contexts where security is a priority. Use
/// .NET's built-in RandomNumberGenerator, instead.
///
/// Distinguishing properties:
/// * Very modern implementation of a Linear Feedback Shift Register with a good balance of performance
/// </summary>
public sealed class XorShift256Random: IRandom
{
    public static IRandom Shared => new XorShift256Random();

    public unsafe void FillBytes(Span<byte> buffer)
    {
        ulong s0 = _s0, s1 = _s1, s2 = _s2, s3 = _s3;

        while (buffer.Length >= sizeof(ulong))
        {
            Unsafe.WriteUnaligned(
                ref MemoryMarshal.GetReference(buffer),
                BitOperations.RotateLeft(s1 * 5, 7) * 9);

            // Update PRNG state.
            ulong t = s1 << 17;
            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;
            s2 ^= t;
            s3 = BitOperations.RotateLeft(s3, 45);

            buffer = buffer.Slice(sizeof(ulong));
        }

        if (!buffer.IsEmpty)
        {
            ulong next = BitOperations.RotateLeft(s1 * 5, 7) * 9;
            byte* remainingBytes = (byte*)&next;
            Debug.Assert(buffer.Length < sizeof(ulong));
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = remainingBytes[i];
            }

            // Update PRNG state.
            ulong t = s1 << 17;
            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;
            s2 ^= t;
            s3 = BitOperations.RotateLeft(s3, 45);
        }

        _s0 = s0;
        _s1 = s1;
        _s2 = s2;
        _s3 = s3;
    }

    private ulong _s0, _s1, _s2, _s3;

    public XorShift256Random(ulong seed1, ulong seed2, ulong seed3, ulong seed4)
    {
        _s0 = seed1;
        _s1 = seed2;
        _s2 = seed3;
        _s3 = seed4;
    }
    
    public XorShift256Random()
    {
        var seeds = new byte[4 * sizeof(ulong)];

        do
        {
            BCrypt.FillBytes(seeds);

            _s0 = BitConverter.ToUInt64(seeds.AsSpan()[..8]);
            _s1 = BitConverter.ToUInt64(seeds.AsSpan()[8..16]);
            _s2 = BitConverter.ToUInt64(seeds.AsSpan()[16..24]);
            _s3 = BitConverter.ToUInt64(seeds.AsSpan()[24..32]);
        } while(_s0 == 0 && _s1 == 0 && _s2 == 0 && _s3 == 0);
    }
}