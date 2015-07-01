using NUnit.Framework;

namespace Kingdom
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        /// <summary>
        /// 1e-2
        /// </summary>
        protected const double Epsilon = 1e-2;

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
        }

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }
    }
}
