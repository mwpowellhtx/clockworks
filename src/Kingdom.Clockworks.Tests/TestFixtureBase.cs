using NUnit.Framework;

namespace Kingdom.Clockworks
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
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
