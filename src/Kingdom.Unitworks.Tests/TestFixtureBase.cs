using NUnit.Framework;

namespace Kingdom.Unitworks
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        protected const double Value = 999d;

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
        }

        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}
