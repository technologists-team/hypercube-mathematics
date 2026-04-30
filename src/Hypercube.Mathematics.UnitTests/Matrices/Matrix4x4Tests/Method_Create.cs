using System.Diagnostics.CodeAnalysis;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Mathematics.UnitTests.Matrices.Matrix4x4Tests;

[TestFixture, SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class Method_Create
{
    private const float Delta = 1e-5f;

    [Test, TestCaseSource(nameof(TransformTestData))]
    public void Create_Transform_ShouldMatchSequentialMultiplication(Vector3 translation, Quaternion rotation, Vector3 scale)
    {
        var expected =
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateRotation(rotation) *
            Matrix4x4.CreateTranslation(translation);
        
        var actual =
            Matrix4x4.CreateTransformSRT(translation, rotation, scale);
       
        AssertAreEqual(actual, expected, Delta);
    }

    private static IEnumerable<TestCaseData> TransformTestData()
    {
        yield return new TestCaseData(Vector3.Zero, Quaternion.Identity, Vector3.One)
            .SetName("Transform_Identity");
        yield return new TestCaseData(new Vector3(100, -50, 0.5f), Quaternion.Identity, Vector3.One)
            .SetName("Transform_PureTranslation");
        yield return new TestCaseData(Vector3.Zero, Quaternion.FromEuler(float.Pi / 2, 0, 0), Vector3.One)
            .SetName("Transform_PureRotation_Yaw90");
        yield return new TestCaseData(Vector3.Zero, Quaternion.FromEuler(0, float.Pi / 2, 0), Vector3.One)
            .SetName("Transform_PureRotation_Pitch90");
        yield return new TestCaseData(Vector3.Zero, Quaternion.Identity, new Vector3(2, 0.5f, 1.1f))
            .SetName("Transform_NonUniformScale");
        yield return new TestCaseData(Vector3.Zero, Quaternion.Identity, new Vector3(-1, 1, 1))
            .SetName("Transform_NegativeScale_Mirroring");
        yield return new TestCaseData(new Vector3(10, 20, 30), Quaternion.FromEuler(0.5f, 0.2f, 0.1f), new Vector3(1, 2, 3))
            .SetName("Transform_CombinedSRT");
        yield return new TestCaseData(new Vector3(0.001f, 0.001f, 0.001f), Quaternion.Identity, Vector3.One)
            .SetName("Transform_MicroTranslation");
        yield return new TestCaseData(Vector3.Zero, Quaternion.Identity, Vector3.Zero)
            .SetName("Transform_ZeroScale_Degenerate");
    }

    [Test, TestCaseSource(nameof(RotationXYZTestData))]
    public void Create_Rotation_Axes(Matrix4x4 rotationMatrix, Vector3 input, Vector3 expected)
    {
        AssertAreEqual(input * rotationMatrix, expected, Delta);
    }

    private static IEnumerable<TestCaseData> RotationXYZTestData()
    {
        const float angle = float.Pi / 2;
        var v = new Vector3(0, 1, 0);
        
        yield return new TestCaseData(Matrix4x4.CreateRotationX(angle), v, new Vector3(0, 0, 1)).SetName("Rotation_X_Axis");
        yield return new TestCaseData(Matrix4x4.CreateRotationY(angle), v, new Vector3(0, 1, 0)).SetName("Rotation_Y_Axis");
        yield return new TestCaseData(Matrix4x4.CreateRotationZ(angle), v, new Vector3(1, 0, 0)).SetName("Rotation_Z_Axis");
    }
}