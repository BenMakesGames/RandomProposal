using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BenMakesGames.Random;

public static class RandomExtensions
{
    private static void ThrowMinMaxValueSwapped() =>
        throw new ArgumentOutOfRangeException("min", "min must be less than exclusiveMax");
    
    private static void ThrowMaxValueLessThanOne() =>
        throw new ArgumentOutOfRangeException("exclusiveMax", "maxValue must be greater than zero");

    // bools
    public static bool GetBool(this IRandom random)
        => random.GetByte() < 128;

    // bytes
    public static Span<byte> GetBytes(this IRandom random, int length)
    {
        Span<byte> buffer = new byte[length];
        random.FillBytes(buffer);
        return buffer;
    }
    
    public static byte GetByte(this IRandom random)
        => random.GetBytes(1)[0];

    public static byte GetByte(this IRandom random, byte exclusiveMax)
        => (byte)(random.GetByte() % exclusiveMax);

    public static byte GetByte(this IRandom random, byte min, byte exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();
        
        return (byte)(random.GetByte((byte)(exclusiveMax - min)) + min);
    }
    
    // shorts
    public static short GetShort(this IRandom random, short exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();
        
        var value = BitConverter.ToInt16(random.GetBytes(2));
        return (short)(value % exclusiveMax);
    }
    
    public static short GetShort(this IRandom random)
        => BitConverter.ToInt16(random.GetBytes(2));
    
    public static short GetShort(this IRandom random, short min, short exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return (short)(random.GetShort((short)(exclusiveMax - min)) + min);
    }
    
    // unsigned shorts
    public static Span<ushort> GetUShorts(this IRandom random, int length)
    {
        Span<ushort> buffer = new ushort[length];
        random.FillUShort(buffer);
        return buffer;
    }
    
    public static ushort GetUShort(this IRandom random, ushort exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();
        
        var value = BitConverter.ToUInt16(random.GetBytes(2));
        return (ushort)(value % exclusiveMax);
    }

    public static ushort GetUShort(this IRandom random) => random.GetUShorts(1)[0];
    
    public static ushort GetUShort(this IRandom random, ushort min, ushort exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return (ushort)(random.GetUShort((ushort)(exclusiveMax - min)) + min);
    }

    // ints
    public static int GetInt(this IRandom random, int exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();

        if(exclusiveMax == 1)
            return 0;
        
        // Narrow down to the smallest range [0, 2^bits] that contains maxValue.
        // Then repeatedly generate a value in that outer range until we get one within the inner range.
        int bits = BitOperations.Log2((uint)exclusiveMax);
        if (BitOperations.PopCount((uint)exclusiveMax) != 1)
        {
            bits++;
        }

        while (true)
        {
            var result = random.GetUInt() >> (sizeof(uint) * 8 - bits);
            if (result < (uint)exclusiveMax)
            {
                return (int)result;
            }
        }
    }

    public static int GetInt(this IRandom random)
        => (int)random.GetUInts(1)[0] & ~(1 << 31);

    public static int GetInt(this IRandom random, int min, int exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return random.GetInt(exclusiveMax - min) + min;
    }

    // unsigned ints
    public static Span<uint> GetUInts(this IRandom random, int length)
    {
        Span<uint> buffer = new uint[length];
        random.FillUInt(buffer);
        return buffer;
    }
    
    public static uint GetUInt(this IRandom random, uint exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();
        
        return random.GetUInt() % exclusiveMax;
    }
    
    public static uint GetUInt(this IRandom random) => random.GetUInts(1)[0];
    
    public static uint GetUInt(this IRandom random, uint min, uint exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return random.GetUInt(exclusiveMax - min) + min;
    }

    // longs
    public static long GetLong(this IRandom random, long exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();

        return random.GetLong() % exclusiveMax;
    }
    
    public static long GetLong(this IRandom random)
        => (long)random.GetULongs(1)[0] & ~(1L << 63);

    public static long GetLong(this IRandom random, long min, long exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return random.GetLong(exclusiveMax - min) + min;
    }
    
    // unsigned longs
    public static Span<ulong> GetULongs(this IRandom random, int length)
    {
        Span<ulong> buffer = new ulong[length];
        random.FillULong(buffer);
        return buffer;
    }
    
    public static ulong GetULong(this IRandom random, ulong exclusiveMax)
    {
        if(exclusiveMax <= 0)
            ThrowMaxValueLessThanOne();
        
        return random.GetULong() % exclusiveMax;
    }

    public static ulong GetULong(this IRandom random) => random.GetULongs(1)[0];
    
    public static ulong GetULong(this IRandom random, ulong min, ulong exclusiveMax)
    {
        if(min >= exclusiveMax)
            ThrowMinMaxValueSwapped();

        return random.GetULong(exclusiveMax - min) + min;
    }
    
    /// <returns>A random float &gt;= 0 and &lt; 1</returns>
    public static float GetFloat(this IRandom random)
        => (random.GetULong() >> 40) * (1.0f / (1u << 24));

    /// <returns>A random double &gt;= 0 and &lt; 1</returns>
    public static double GetDouble(this IRandom random)
        => (random.GetULong() >> 11) * (1.0 / (1ul << 53));

    // strings
    public static string GetString(this IRandom random, string allowedCharacters, int length)
    {
        var buffer = new char[length];

        // allowedCharacters will usually have length <= 256, so we can use a byte to index into it
        if(allowedCharacters.Length <= 256)
        {
            for (var i = 0; i < length; i++)
                buffer[i] = allowedCharacters[random.GetByte((byte)allowedCharacters.Length)];

            return new string(buffer);
        }

        for (var i = 0; i < length; i++)
            buffer[i] = allowedCharacters[random.GetInt(allowedCharacters.Length)];

        return new string(buffer);
    }

    // arrays
    public static T GetItem<T>(this IRandom random, IReadOnlyList<T> list)
        => list[random.GetInt(list.Count)];

    public static List<T> GetItems<T>(this IRandom random, IReadOnlyList<T> list, int count)
    {
        var results = new List<T>(count);

        for (var i = 0; i < count; i++)
            results.Add(random.GetItem(list));

        return results;
    }

    /// <summary>
    /// Shuffles list in place.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this IRandom random, IList<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = random.GetInt(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // enums
    public static T GetItem<T>(this IRandom random) where T: Enum
        => random.GetItem(Enum.GetValues(typeof(T)).Cast<T>().ToList());

    public static List<T> GetItems<T>(this IRandom random, int count) where T: Enum
        => random.GetItems(Enum.GetValues(typeof(T)).Cast<T>().ToList(), count);
}