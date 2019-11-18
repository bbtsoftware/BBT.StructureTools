---
Order: 10
Title: Why does BBT.StructureTools exist?
Description: Introduction to BBT.StructureTools, what problems are solved and how it is distinctive from the known .NET functions
---

## Solve business problems

BBT.StructureTools is a toolset intended to help you handle a tree-like business object structure in relation to the
following tasks:

* Comparing of business objects
* Conversion of business objects
* Copying business objects

Each of the above use cases is admittedly already covered, to some extent, by interfaces available from the .NET Framework.
The structure tools is an advanced tool set, though the same requirements apply on a first glance: The .NET Framework's
classes for instance are mainly designed to perform the operations for a single object, and are therefore not aware of any
business case. Mind you, that's _data_ objects here - They are the same object for any business case, yet they may
need to be treated differently depending on the context - A comparison in a business case is for instance never a comparison
on an object reference but on the object's properties (which in turn can contain child or parent objects important for the
specific comparison) and therefore not something the data object should know. A very simple example is the business case of
checking for duplicate orders in a web store: There may be two completely identical orders, but if they are not ordered by
the same person they may not qualify as duplicate.

BBT.StructureTools has the following goals:

* Testable for completeness
* Easily extendable (both the infrastructure and the user code)
* Customizable depending on the business case
* An unit- and integration-tested infrastructure
