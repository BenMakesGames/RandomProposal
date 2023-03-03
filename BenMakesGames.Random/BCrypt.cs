using System;
using System.Runtime.InteropServices;

namespace BenMakesGames.Random;

// Basically copy-pasted these out of Cryptography.RandomNumberGenerator source code 
public static class BCrypt
{
    public static unsafe void FillBytes(Span<byte> data)
    {
        if (data.Length > 0)
        {
            fixed (byte* ptr = data) FillBytes(ptr, data.Length);
        }
    }

    public static unsafe void FillBytes(byte* buffer, int bufferLength)
    {
        var result = BCryptGenRandom(IntPtr.Zero, buffer, bufferLength, 2);
        if(result != 0)
            throw new Exception("BCryptGenRandom failed");
    }
    
    [DllImport("bcrypt.dll", EntryPoint = "BCryptGenRandom")]
    private static extern unsafe int BCryptGenRandom(IntPtr hAlgorithm, byte* pbBuffer, int cbBuffer, int dwFlags);
}