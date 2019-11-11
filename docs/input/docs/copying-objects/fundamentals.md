---
Order: 40
Title: Fundamentals
Description: Introduction to the copy capabilities
---

## Distinction towards .NET Framework interfaces

There is the **[System.ICloneable]** interface which as the documentation states

> a customized implementation that creates a copy of an existing object

While at a first glance this is a promising approach to provide a caller with a clone of an object the [Notes to implementers]
give a first hint as why it's not a good way of cloning your business objects:

> It does not specify whether the cloning operation performs a deep copy, a shallow copy, or something in between.
> Nor does it require all property values of the original instance to be copied to the new instance.

The `ICloneable` interface also falls a bit short when it comes to handling interfaces. If negligently implemented
interface properties may be omitted from cloning by accident or cloned multiple times - Both are not in the interest
of reliably and consistently cloning data. The Copy infrastructure solves this issue by allowing to define copy steps
from registrations for interface types which the can be consumed and applied when registering copy sub operations
for data classes implementing those interfaces. A well written test can also distinct between properties defined
on a data class and those defined on an interface and therefore prohibit copying interface properties within a
data class' registration.

Apart from this the `ICloneable` interface doesn't enforce a unit-testable implementation - So in practice you may
end up extending your data model, but not copying the new property values when cloning your data object. While this
is a valid decision in some cases it must be a decision made by the developer, and not by omission.
The fluent interface, registration-based approach taken within the Copy implementation of BBT.StructureTools
supports this by allowing a unit test to read registrations and compare these with the properties available on
the object.

[System.ICloneable]: https://docs.microsoft.com/en-us/dotnet/api/system.icloneable
[Notes to implementers]: https://docs.microsoft.com/en-us/dotnet/api/system.icloneable#notes-to-implementers