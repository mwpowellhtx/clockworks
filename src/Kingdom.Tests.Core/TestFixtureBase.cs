using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kingdom
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        /// <summary>
        /// One: 1
        /// </summary>
        protected const int One = 1;

        /// <summary>
        /// OneSecondMilliseconds: 1000d
        /// </summary>
        /// <see cref="One"/>
        protected const double OneSecondMilliseconds = One*1000d;

        /// <summary>
        /// 1e-2
        /// </summary>
        public const double Epsilon = 1e-2;

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
        /// Invokes the operator corresponding to the <paramref name="parts"/> and given the <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="parts"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        protected static void InvokeOperator<TObject>(IEnumerable<OperatorPart> parts,
            IEnumerable<Type> argTypes, params object[] args)
        {
            var objType = typeof (TObject);
            Assert.That(parts, Is.Not.Null);

            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

            var name = parts.GetMemberName();
            Assert.That(argTypes, Is.Not.Null);

            // ReSharper disable PossibleMultipleEnumeration
            var method = objType.GetMethod(name, flags, Type.DefaultBinder, argTypes.ToArray(), null);

            Assert.That(method, Is.Not.Null, @"Unable to find method {0} given argument types {1} and flags {2}.",
                name, string.Join(@", ", argTypes.Select(x => x.FullName)), flags);

            method.Invoke(null, args);
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{TObject}"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="parts"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected static Task InvokeOperatorAsync<TObject>(IEnumerable<OperatorPart> parts,
            params object[] args)
        {
            return Task.Run(() => InvokeOperator<TObject>(parts, args.Select(a => a.GetType()), args));
        }

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="parts"/> and <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="parts"></param>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        protected static TResult InvokeOperator<TObject, TResult>(IEnumerable<OperatorPart> parts,
            Func<object, TResult> filter, params object[] args)
        {
            return InvokeOperator<TObject, TResult>(parts, filter, args.Select(a => a.GetType()), args);
        }

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="parts"/> and <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="parts"></param>
        /// <param name="filter"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        protected static TResult InvokeOperator<TObject, TResult>(IEnumerable<OperatorPart> parts,
            Func<object, TResult> filter, IEnumerable<Type> argTypes, params object[] args)
        {
            var objType = typeof (TObject);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;

            var name = parts.GetMemberName();

            var op = objType.GetMethod(name, flags, Type.DefaultBinder, argTypes.ToArray(), null);

            Assert.That(op, Is.Not.Null,
                @"Unable to identify the operator named {0} with {1} binding flags and {2} arguments",
                name, flags, string.Join(@", ", argTypes.Select(t => t.FullName)));

            //TODO: may want to specify the expected argument types...
            //var result2 = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            //var result = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            var result = op.Invoke(null, args);

            return filter(result);
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{TObject, TResult}"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="parts"></param>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected static Task<TResult> InvokeOperatorAsync<TObject, TResult>(IEnumerable<OperatorPart> parts,
            Func<object, TResult> filter, params object[] args)
        {
            return Task.Run(() => InvokeOperator<TObject, TResult>(parts, filter, args.Select(a => a.GetType()), args));
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{TObject, TResult}"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="parts"></param>
        /// <param name="filter"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected static Task<TResult> InvokeOperatorAsync<TObject, TResult>(IEnumerable<OperatorPart> parts,
            Func<object, TResult> filter, IEnumerable<Type> argTypes, params object[] args)
        {
            return Task.Run(() => InvokeOperator<TObject, TResult>(parts, filter, argTypes, args));
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

        //TODO: this might go well as an NUnit extensions...
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="maxLevel"></param>
        /// <param name="includeInner"></param>
        /// <returns></returns>
        public static bool WasException<T>(this Exception exception, int maxLevel = 1, bool includeInner = false)
            where T : Exception
        {
            for (var currentLevel = 0; currentLevel < maxLevel; currentLevel++)
            {
                if (exception == null) return false;

                if (exception is T) return true;

                if (!includeInner) break;

                exception = exception.InnerException;
            }

            return false;
        }
    }
}
