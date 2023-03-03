using FluentAssertions;
using Xunit;

namespace BenMakesGames.Random.Tests;

public class TestMethods
{
    public static void GetInt_DistributesEvenly(IRandom random)
    {
        const int numberOfSamples = 1000000;
        const int numberOfBuckets = 10;
        const double epsilon = 0.05; // acceptable deviation from expected value

        // Act
        var buckets = new int[numberOfBuckets];
        for (int i = 0; i < numberOfSamples; i++)
        {
            int randomNumber = random.GetInt(numberOfBuckets);
            
            // the following improves the distribution... not that FF1 RNG needs to have good distribution :P
            if(i % 64 == 0)
                random.GetByte();
            
            buckets[randomNumber]++;
        }

        // Assert
        double expectedCount = numberOfSamples / (double)numberOfBuckets;
        foreach (var count in buckets)
        {
            Assert.InRange(count, expectedCount * (1 - epsilon), expectedCount * (1 + epsilon));
        }
    }
    
    public static void GetByte_ReturnsAllPossibleValues(IRandom random)
    {
        const int numberOfSamples = 1000000;
        
        // Act
        var counts = new int[256];
        
        for(int i = 0; i < numberOfSamples; i++)
            counts[random.GetByte()]++;

        for(int i = 0; i < 256; i++)
            counts[i].Should().BeGreaterThan(0);
    }
}