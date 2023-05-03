using System;

namespace BenMakesGames.Random;

/// <summary>
/// Thin wrapper for bcrypt.dll
///
/// Distinguishing properties:
/// * Asks the OS for random numbers
/// * Suitable for cryptographic use
/// </summary>
public class BCryptRandom: IRandom
{
    public static IRandom Shared => new BCryptRandom();
    
    public void FillBytes(Span<byte> buffer) => BCrypt.FillBytes(buffer);
}