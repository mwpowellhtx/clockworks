using System;
using NUnit.Framework;

namespace Kingdom
{
    public static class ExceptionExtensionMethods
    {
        public static void Throws<TException>(this object anchor, Action action)
            where TException : Exception
        {
            Assert.That(anchor, Is.Not.Null);

            try
            {
                action();
                Assert.Fail("Expected {0} exception.", typeof (TException));
            }
            catch (Exception ex)
            {
                Assert.That(ex.DidExceptionOccur<TException>());
            }
        }

        public static void DoesNotThrow<TException>(this object anchor, Action action)
            where TException : Exception
        {
            Assert.That(anchor, Is.Not.Null);

            try
            {
                action();
            }
            catch (TException tex)
            {
                Assert.Fail("Failed not to throw {0} exception.", typeof (TException));
            }
            catch (Exception ex)
            {
                // Including any of its inner exceptions...
                Assert.That(ex.DidExceptionOccur<TException>(), Is.False);

                // It can throw anything else, however, just not TException.
                throw;
            }
        }

        public static bool DidExceptionOccur<TException>(this Exception exception)
            where TException : Exception
        {
            while (exception != null)
            {
                if (exception is TException) return true;
                exception = exception.InnerException;
            }
            return false;
        }
    }
}
