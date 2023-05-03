using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

/// <summary>
/// From https://www.youtube.com/watch?v=LWFzPP8ZbdU
///
/// Distinguishing feature:
/// * Different seeds are not indexes into the same sequence
/// </summary>
public class Squirrel3Random: IRandom
{
    private readonly uint Seed;
    private uint Position;
    
    public Squirrel3Random()
    {
        var seeds = new byte[sizeof(uint)];

        BCrypt.FillBytes(seeds);

        Seed = BitConverter.ToUInt32(seeds);
    }
    
    public Squirrel3Random(uint seed)
    {
        Seed = seed;
    }

    public static IRandom Shared { get; } = new Squirrel3Random();
    
    private const uint BitNoise1 = 0xB5297A4D;
    private const uint BitNoise2 = 0x68E31DA4;
    private const uint BitNoise3 = 0x1B56C4E9;

    public unsafe void FillBytes(Span<byte> buffer)
    {
        var position = Position;

        while (buffer.Length >= sizeof(uint))
        {
            var mangled = position;
            mangled *= BitNoise1;
            mangled += Seed;
            mangled ^= (mangled >> 8);
            mangled += BitNoise2;
            mangled ^= (mangled << 8);
            mangled *= BitNoise3;
            mangled ^= (mangled >> 8);

            Unsafe.WriteUnaligned(
                ref MemoryMarshal.GetReference(buffer),
                mangled
            );

            buffer = buffer.Slice(sizeof(uint));
            position++;
        }

        if(!buffer.IsEmpty)
        {
            var mangled = position;
            mangled *= BitNoise1;
            mangled ^= (mangled >> 8);
            mangled += BitNoise2;
            mangled ^= (mangled << 8);
            mangled *= BitNoise3;
            mangled ^= (mangled >> 8);

            byte* remainingBytes = (byte*)&mangled;
            
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = remainingBytes[i];
            }

            position++;
        }

        Position = position;
    }
}