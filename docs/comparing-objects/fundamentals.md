# Fundamentals

Introduction to the compare capabilities.

## Distinction towards .NET Framework interfaces

The .NET Framework comes with multiple interfaces which allow for comparison of objects:

* [System.IComparable]
* [System.IEquatable]

While *comparing* an object is something used for example to order a list, *equating* an object is the act
of defining if `objectA` is the same as `objectB`.
As an example, if you sort a list of people, two may be named *John Doe* but they're not necessarily the same person.
To make this even more complicated, an object being reference-compared may also yield different results.

The compare infrastructure in BBT.StructureTools is aiming at the comparison in a business case centered view
and leaves the technical comparison of objects (aka [Object.ReferenceEquals]) to the framework, or leverages it
if a reference comparison is desired from a business use case. *Euqating* is not supported and must be achieved
using the aforementioned .NET Framework functionality.

As in the other operations of BBT.StructureTools the comparison works per class or interface by registering the
comparison on this level per attribute and using other registrations for parent, child or otherwise related objects.

[System.IComparable]: https://docs.microsoft.com/en-us/dotnet/api/system.icomparable
[System.IEquatable]: https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1
[Object.ReferenceEquals]: https://docs.microsoft.com/en-us/dotnet/api/system.object.referenceequals
