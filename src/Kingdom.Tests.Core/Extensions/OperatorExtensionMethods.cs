using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kingdom
{
    public static class OperatorExtensionMethods
    {
        /// <summary>
        /// Invokes the operator corresponding to the <paramref name="op"/> and given the <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        public static object InvokeOperator<THost>(this THost host, OperatorPart op,
            IEnumerable<Type> argTypes, params object[] args)
        {
            Assert.That(host, Is.Not.Null);

            var hostType = typeof(THost);

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;

            var memberName = op.GetMemberName();
            Assert.That(argTypes, Is.Not.Null);

            // ReSharper disable PossibleMultipleEnumeration
            var methodInfo = hostType.GetMethod(memberName, flags, Type.DefaultBinder, argTypes.ToArray(), null);

            Assert.That(methodInfo, Is.Not.Null, @"Unable to find method {0} given argument types {1} and flags {2}.",
                memberName, string.Join(@", ", argTypes.Select(x => x.FullName)), flags);

            return methodInfo.Invoke(null, args);
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{TObject}"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task InvokeOperatorAsync<THost>(this THost host,
            OperatorPart op , IEnumerable<Type> argTypes, params object[] args)
        {
            return Task.Run(() => host.InvokeOperator<THost>(op, argTypes, args));
        }

        public static TResult InvokeOperator<THost, TResult>(this THost host, OperatorPart op,
            params object[] args)
        {
            return host.InvokeOperator<THost, TResult>(op, from a in args select a.GetType(),
                x =>
                {
                    Assert.That(x, Is.Not.Null);
                    Assert.That(x, Is.TypeOf<TResult>());
                    return (TResult) x;
                }, args);
        }

        public static bool TryInvokeOperator<THost, TResult>(this THost host, OperatorPart op,
            out TResult result, params object[] args)
        {
            return host.TryInvokeOperator(op, x => x is TResult, x => (TResult) x, out result, args);
        }

        public static bool TryInvokeOperator<THost, TResult>(this THost host, OperatorPart op,
            Func<object, bool> evaluate, Func<object, TResult> convert, out TResult result,
            params object[] args)
        {
            var argTypes = from a in args select a.GetType();

            var local = host.InvokeOperator<THost>(op, argTypes, args);

            result = default(TResult);

            if (!evaluate(local)) return false;

            result = convert(local);

            return true;
        }

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="op"/> and <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        public static TResult InvokeOperator<THost, TResult>(this THost host, OperatorPart op,
            Func<object, TResult> filter, params object[] args)
        {
            return host.InvokeOperator<THost, TResult>(op, from a in args select a.GetType(), filter, args);
        }

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="op"/> and <paramref name="args"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="filter"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        public static TResult InvokeOperator<THost, TResult>(this THost host, OperatorPart op,
            IEnumerable<Type> argTypes, Func<object, TResult> filter, params object[] args)
        {
            var result = host.InvokeOperator(op, argTypes, args);
            return filter(result);
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{THost, TResult}"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="filter"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task<TResult> InvokeOperatorAsync<THost, TResult>(this THost host,
            OperatorPart op, Func<object, TResult> filter, params object[] args)
        {
            return Task.Run(() => host.InvokeOperator<THost, TResult>(op, filter, args));
        }

        /// <summary>
        /// Asynchronously invokes the operator according to <see cref="InvokeOperator{THost, TResult}"/>.
        /// </summary>
        /// <typeparam name="THost"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="host"></param>
        /// <param name="op"></param>
        /// <param name="filter"></param>
        /// <param name="argTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task<TResult> InvokeOperatorAsync<THost, TResult>(this THost host,
            OperatorPart op, IEnumerable<Type> argTypes, Func<object, TResult> filter, params object[] args)
        {
            return Task.Run(() => host.InvokeOperator<THost, TResult>(op, argTypes, filter, args));
        }
    }
}
