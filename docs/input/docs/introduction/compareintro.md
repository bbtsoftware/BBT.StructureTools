---
Order: 20
Title: Introduction Compare
Description: Introduction to the compare capabilities
---

## Distinction towards .Net interfaces

The .Net framework knows multiple interfaces which should allow for comparison of objects. Namely those are

* [System.IComparable]
* [System.IEquatable]

While *comparing* an object is something used for example to order a list, *equating* an object is the act
of defining if `objectA` is the same as `objectB`.
As an example, if you sort a list of people two may be named *John Doe* but they're not necessarily the same person.
To make this even more complicated, an object being reference-compared may also yield different results.

The Compare infrastructure from BBT.StructureTools is aiming at the comparison in a business case centered view
and leaves the technical comparison of objects (aka [Object.ReferenceEquals]) to the framework, or leverages it
if a reference comparison is desired from a business use case.

As in the other two of the 3Cs the comparison works per class or interface by registering the comparison on this level per attribute
and using other registrations for parent, child or otherwise related objects.

[System.IComparable]: https://docs.microsoft.com/en-us/dotnet/api/system.icomparable
[System.IEquatable]: https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1
[Object.ReferenceEquals]: https://docs.microsoft.com/en-us/dotnet/api/system.object.referenceequals
