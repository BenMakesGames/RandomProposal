using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

// Here's the problem, as I see it:
// * Random and RandomNumberGenerator are conceptually similar, but have different APIs.
// * Generating random bytes, and what you do with those random bytes, are separate concerns.
//
// Real-world example: Unity has its own `Random`, which they use to provide methods like `InsideUnitCircle`. But it
// DOESN'T have all the other methods C# devs are used to, and WON'T gain any of the new ones added to CLR `Random`.
// It Would be nice if, instead, Unity could write their utility methods as IRandom extension methods. And/or, if
// Unity still wants to provide their own RNG for some Unity performance reasons, or whatever, they could do so
// without losing all the methods provided to IRandom, either by the CLR, or by third-party libraries.
//
// For a less-practical example, see the included `BadRandom` class.
public interface IRandom
{
    static abstract IRandom Shared { get; } // not sure if this should really be part of the interface...??

    void FillBytes(Span<byte> buffer);

    void FillUShort(Span<ushort> buffer)
    {
        var byteSpan = MemoryMarshal.Cast<ushort, byte>(buffer);
        FillBytes(byteSpan);
    }

    void FillUInt(Span<uint> buffer)
    {
        var byteSpan = MemoryMarshal.Cast<uint, byte>(buffer);
        FillBytes(byteSpan);
    }

    void FillULong(Span<ulong> buffer)
    {
        var byteSpan = MemoryMarshal.Cast<ulong, byte>(buffer);
        FillBytes(byteSpan);
    }
}