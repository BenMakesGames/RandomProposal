using System;

namespace BenMakesGames.Random;

/// <summary>
/// Thin wrapper for bcrypt.dll
/// </summary>
public class BCryptRandom: IRandom
{
    public static IRandom Shared { get; } = new BCryptRandom();
    
    public void FillBytes(Span<byte> buffer) => BCrypt.FillBytes(buffer);
}