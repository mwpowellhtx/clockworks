# Clockworks

Sometimes it is necessary to override the system clock during controlled, simulated experiments. It's really quite simple. Clockworks offers the following features.

## Basic Features

The ``SystemClock`` overrides the default [DateTime](http://msdn.microsoft.com/en-gb/system.datetime), providing an easy to use alternative to [DateTime.Now](http://msdn.microsoft.com/en-us/library/system.datetime.now.aspx) and [DateTime.UtcNow](http://msdn.microsoft.com/en-us/library/system.datetime.utcnow.aspx).

## Disposable System Clock

``SystemClock`` is [IDisposable](http://msdn.microsoft.com/en-us/library/system.idisposable.aspx), so it allows you to wrap a using statement around an instance when you require control over the clock.

```C#
public interface ISystemClock : IDisposable
{
   // ...
}
```

## Stacking System Clock

``SystemClock`` installations may be stacked. That is, you can stack instances of clock overrides using the ``Install`` feature. This returns the same disposable instance as creating a new instance yourself. When it disposes, the disposing instance is popped off the stack, thus exposing either the previously installed instance, or the bare system [DateTime](http://msdn.microsoft.com/en-gb/system.datetime).

```C#
using (var installed = SystemClock.Install(new DateTime(1999, 1, 1)))
{
   // ...
}
```

## Static Properties

The ``SystemClock.Now`` and ``SystemClock.UtcNow`` properties expose the bare system clock, or the overridden system clock after calling ``Install``.

```C#
using (var installed = SystemClock.Install(...)
{
    Console.WriteLine("This is the new Now: {0}", SystemClock.Now);
    Console.WriteLine("This is the new UtcNow: {0}", SystemClock.UtcNow);
}
```

## Instance Properties

You may use an ``ISystemClock`` instance to feed your [DateTime](http://msdn.microsoft.com/en-gb/system.datetime) concerns, but using the ``ISystemClock.Now`` and ``ISystemClock.UtcNow``, respectively, after creating a ``new`` instance.

```C#
using (ISystemClock instance = new SystemClock(...))
{
    Console.WriteLine("This is the new Now: {0}", instance.Now);
    Console.WriteLine("This is the new UtcNow: {0}", instance.UtcNow);
}
```

## The Nitty Gritty

Because the instance properties are implemented via the interface, you do need to "see" the instance as an interface. This is due to the fact that there are static properties by the same name for the ``SystemClock`` class, which precludes the same instance properties from being named.

While it feels like a limitation, this can prove advantageous, especially in more robust applications where you require some level of [Dependency Injection](http://en.wikipedia.org/wiki/Dependency_injection), [Inversion of Control](http://en.wikipedia.org/wiki/Inversion_of_control), or other such factories and life cycle managers.

## Conclusion and future works

That's it. Pretty straightforward.

I am considering adding in some stopwatch and timer features if time permits and necessity dictates.

Enjoy!
