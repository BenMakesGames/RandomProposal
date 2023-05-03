using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

// 
/// <summary>
/// RNG used by NES Tetris, from adapted from https://meatfighter.com/nintendotetrisai
///
/// Distinguishing properties:
/// * An old Linear Feedback Shift Register implementation - same tech as XorShift256, but from an older time
/// </summary>

public class TetrisRandom: IRandom
{
    public static IRandom Shared => new TetrisRandom();
    
    private ushort State = 0x8988;
    
    public unsafe void FillBytes(Span<byte> buffer)
    {
        var state = State;
        
        while (buffer.Length >= sizeof(ushort))
        {
            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(buffer), state);

            state = (ushort)((state >> 1) | ((((state >> 9) ^ (state >> 1)) & 1) << 15));

            buffer = buffer.Slice(sizeof(ushort));
        }

        if (!buffer.IsEmpty)
        {
            byte* remainingBytes = (byte*)&state;
            
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = remainingBytes[i];
            }

            state = (ushort)((state >> 1) | ((((state >> 9) ^ (state >> 1)) & 1) << 15));
        }

        State = state;
    }
}