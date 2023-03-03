using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

public class Squirrel3Random: IRandom
{
    private UInt64 Position;
    
    public Squirrel3Random()
    {
        var seeds = new byte[sizeof(ulong)];

        BCrypt.FillBytes(seeds);

        Position = BitConverter.ToUInt64(seeds);
    }
    
    public Squirrel3Random(UInt64 seed)
    {
        Position = seed;
    }

    public static IRandom Shared { get; } = new Squirrel3Random();
    
    private const UInt64 BitNoise1 = 0xB5297A4DB5297A4D;
    private const UInt64 BitNoise2 = 0x68E31DA468E31DA4;
    private const UInt64 BitNoise3 = 0x1B56C4E91B56C4E9;

    public unsafe void FillBytes(Span<byte> buffer)
    {
        var position = Position;

        while (buffer.Length >= sizeof(ulong))
        {
            var mangled = position;
            mangled *= BitNoise1;
            mangled ^= (mangled >> 8);
            mangled += BitNoise2;
            mangled ^= (mangled << 8);
            mangled *= BitNoise3;
            mangled ^= (mangled >> 8);

            Unsafe.WriteUnaligned(
                ref MemoryMarshal.GetReference(buffer),
                mangled
            );

            buffer = buffer.Slice(sizeof(ulong));
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