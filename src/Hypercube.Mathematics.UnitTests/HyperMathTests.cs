using Hypercube.Mathematics;

namespace Hypercube.Mathematics.UnitTests;

[TestFixture]
public sealed class HyperMathTests
{
    private static IEnumerable<TestCaseData> MoveTowardsCases()
    {
        yield return new TestCaseData(5, 10, 2, 7).SetName("ForwardStepWithinRange");
        yield return new TestCaseData(5, 10, 10, 10).SetName("ForwardStepExceedsTarget");
        yield return new TestCaseData(10, 5, 3, 7).SetName("BackwardStepWithinRange");
        yield return new TestCaseData(10, 5, 10, 5).SetName("BackwardStepExceedsTarget");
        yield return new TestCaseData(10, 10, 5, 10).SetName("AlreadyAtTarget");
        yield return new TestCaseData(5, 10, 0, 5).SetName("ZeroDistance");
    }

    [TestCaseSource(nameof(MoveTowardsCases))]
    public void MoveTowardsReturnsExpectedResult(int current, int target, int distance, int expected)
    {
        Assert.AreEqual(expected, HyperMath.MoveTowards(current, target, distance));
    }
    
    [Test]
    public void MoveTowardsNegativeDistanceThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => HyperMath.MoveTowards(5, 10, -1));
    }
}