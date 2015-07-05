using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="parts"/> and <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="parts"></param>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        protected static TResult InvokeOperator<TObject, TResult>(
            TObject obj, IEnumerable<OperatorPart> parts,
            Func<object, TResult> filter, params object[] args)
        {
            var objType = typeof (TObject);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;

            var name = parts.Select(p => p.ToString()).Aggregate(@"op_", (g, x) => g + x);

            var argTypes = args.Select(a => a.GetType()).ToArray();

            var op = objType.GetMethod(name, flags, Type.DefaultBinder, argTypes, null);

            Assert.That(op, Is.Not.Null,
                @"Unable to identify the operator named {0} with {1} binding flags and {2} arguments",
                name, flags, string.Join(@", ", argTypes.Select(t => t.FullName)));

            //TODO: may want to specify the expected argument types...
            //var result2 = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            //var result = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            var result = op.Invoke(null, args);

            return filter(result);
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
    }
}
