---
Order: 30
Title: Configuration
Description: Configure BBT.StructureTools
---

The BBT Structure Tools are entirely configurable by code. This section covers the
correct IoC container registration of the infrastructure components.

# Preconditions

* The library requires an IoC container to resolve components internally.

# IoC Container

The library is usable with any IoC container, but it must support some preconditions:

* Injection to public constructors of `internal` classes
* Registration of unbound generic types

## Initialization

Before the registrations can be done the `IocHandler` resolver must be set.
Implement the resolver using the `IIocResolver`.
Once this is done you can configure the `IocHandler`:

```csharp
var resolver = new YourResolver(); // Your implementation of the IIocResolver
IocHandler.Instance.IocResolver = resolver;
```

Afterwards make sure that the dependencies from the [BBT.StrategyPattern] are registered.
See [BBT.StrategyPattern usage with IoC] for instructions.

In order for the library to work correctly it needs to register some types into the IoC container
itself. As the implementations are private to allow a concise API it needs to do this by itself within
the `IocHandler` (see also [TestIocContainer] where this initialization is made for testing with Ninject).

```csharp
IocHandler.Instance.DoIocRegistrations(this.DoRegistration);

private void DoRegistration(Type impl, Type abstraction)
{
    // Register the implementation for the abstraction
    // within your IoC container here.
}
```

[BBT.StrategyPattern]: https://bbtsoftware.github.io/BBT.StrategyPattern/
[BBT.StrategyPattern usage with IoC]: https://bbtsoftware.github.io/BBT.StrategyPattern/docs/usage/use-with-ioc
[TestIocContainer]: https://github.com/bbtsoftware/BBT.StructureTools/blob/develop/src/BBT.StructureTools.Tests/TestTools/TestIoContainer.cs
