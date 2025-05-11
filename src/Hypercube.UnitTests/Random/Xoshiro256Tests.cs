using Hypercube.Mathematics.Random;

namespace Hypercube.UnitTests.Random;

[TestFixture]
public sealed class Xoshiro256Tests
{
    // Test the initialization of the generator with a seed
    [Test]
    public void TestInitializationWithSeed()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);

        // Ensure that the generated seed is a non-negative long value
        Assert.That(rng.Seed, Is.GreaterThanOrEqualTo(0), "The derived seed should be a non-negative value.");
    
        // If you reinitialize with the derived seed, it should generate the same numbers
        var rng2 = new Xoshiro256(rng.State);
    
        // Compare a few generated values from both instances to ensure they are the same
        Assert.That(rng2.NextInt(), Is.EqualTo(rng.NextInt()), "Generators with the same derived seed should produce identical values.");
    }

    // Test generating a float in the range [0, 1]
    [Test]
    public void TestNextFloatInRange()
    {
        const long seed = 123456789;
        
        var rng = new Xoshiro256(seed);
        var result = rng.NextFloat();

        Assert.That(result is >= 0f and < 1f, Is.True, "Generated float should be in the range [0, 1].");
    }

    // Test generating a float within a custom range [min, max]
    [Test]
    public void TestNextFloatInCustomRange()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);
        
        const float min = 5f;
        const float max = 10f;
        
        var result = rng.NextFloat(min, max);

        Assert.That(result >= min && result < max, Is.True, "Generated float should be in the range [min, max].");
    }

    // Test generating an integer within a range [min, max]
    [Test]
    public void TestNextIntInRange()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);
        
        const int min = 1;
        const int max = 100;
        
        var result = rng.NextInt(min, max);

        Assert.That(result is >= min and < max, Is.True, "Generated int should be in the range [min, max].");
    }

    // Test generating an integer within a range [0, max]
    [Test]
    public void TestNextIntUpToMax()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);
        
        const int max = 50;
        var result = rng.NextInt(max);

        Assert.That(result is >= 0 and <= max, Is.True, "Generated int should be in the range [0, max).");
    }

    // Test the behavior when the max value is less than or equal to the min value in NextInt
    [Test]
    public void TestNextIntWithMaxLessThanMin()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);
        
        const int min = 10;
        const int max = 10;

        // Should return the min value since max <= min
        var result = rng.NextInt(min, max);
        Assert.That(result, Is.EqualTo(min), "When max is less than or equal to min, the function should return min.");
    }

    // Test the behavior when the min value is greater than the max value in NextInt
    [Test]
    public void TestNextIntWithMinGreaterThanMax()
    {
        const long seed = 123456789;
        var rng = new Xoshiro256(seed);
        
        const int min = 20;
        const int max = 10;

        // Should return the min value since min > max
        var result = rng.NextInt(min, max);
        Assert.That(result, Is.EqualTo(min), "When min is greater than max, the function should return min.");
    }

    // Test for edge cases with the seed initialization
    [Test]
    public void TestSeedConsistency()
    {
        const long seed = 987654321;
        
        var rng1 = new Xoshiro256(seed);
        var rng2 = new Xoshiro256(seed);

        Assert.Multiple(() =>
        {
            // The two instances should produce identical sequences given the same seed
            Assert.That(rng2.NextInt(), Is.EqualTo(rng1.NextInt()), "Generators with the same seed should produce identical values.");
            Assert.That(rng2.NextFloat(), Is.EqualTo(rng1.NextFloat()), "Generators with the same seed should produce identical float values.");
        });
    }
}