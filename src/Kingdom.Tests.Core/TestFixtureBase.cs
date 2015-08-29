using System;
using NUnit.Framework;

namespace Kingdom
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        protected const double Epsilon = 1e-3;

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

    /// <summary>
    /// Invoke extension methods.
    /// </summary>
    public static class InvokeExtensionMethods
    {
        /// <summary>
        /// Verifies the <paramref name="obj"/> using the <paramref name="verify"/> action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static T Verify<T>(this T obj, Action<T> verify)
        {
            verify = verify ?? (x => { });
            verify(obj);
            return obj;
        }

        ////TODO: this might go well as an NUnit extensions...
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="exception"></param>
        ///// <param name="maxLevel"></param>
        ///// <param name="includeInner"></param>
        ///// <returns></returns>
        //public static bool WasException<T>(this Exception exception, int maxLevel = 1, bool includeInner = false)
        //    where T : Exception
        //{
        //    for (var currentLevel = 0; currentLevel < maxLevel; currentLevel++)
        //    {
        //        if (exception == null) return false;

        //        if (exception is T) return true;

        //        if (!includeInner) break;

        //        exception = exception.InnerException;
        //    }

        //    return false;
        //}
    }
}
