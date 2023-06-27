using NUnit.Framework;
using RobinBird.Utilities.Unity.Helper;

namespace RobinBird.Utilities.Unity.Tests
{
    [TestFixture]
    public class MathHelperTests
    {
        [TestCase(1, 0, 3, 1)]
        [TestCase(0, 0, 3, 0)]
        [TestCase(0, 1, 3, 3)]
        [TestCase(4, 0, 3, 0)]
        [TestCase(2, -1, 5, 2)]
        [TestCase(5, 0, 5, 5)]
        [TestCase(10, 0, 3, 2)]
        public void WrapAroundTest(int i, int min, int max, int result)
        {
            int testResult = MathHelper.WrapInt(i, min, max);
            
            Assert.AreEqual(result, testResult);
        }
    }
}