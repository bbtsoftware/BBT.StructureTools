---
Order: 10
Title: Why StructureTools?
Description: Introduction to StructureTools, what problems are solved and how it is distinctive from the known .Net functions
---

## Solve business problems

The StructureTools is a toolset intended to help you handle a tree-like business object structure in relation to the
following tasks:

* Comparison
* Conversion
* Copy

The operations above are referenced throughout this documentation as the _3C_.

Each of the above use cases is admittedly already covered, to some extent, by interfaces available from the .Net framework.
We're using our own advanced toolset even if we have the same requirements on a first glance : The .Net framework's
classes for instance are mainly designed to perform the 3C operations for a single object, and are therefore not aware of any
business case. Mind you, we're talking about _data_ objects here - They are the same object for any business case, yet they may
need to be treated differently depending on the context - A comparison in a business case is for instance never a comparison
on an object reference but on the object's properties (which in turn can contain child or parent objects important for the
specific comparison) and therefore not something the data object should know. A very simple example is the business case of
checking for duplicate orders in a web store: There may be two completely identical orders, but if they are not ordered by
the same person they may not qualify as duplicate.

Our goal within this library is simple, we want (for all 3C operations):

* Testable for completeness
* Easily extendable (both the infrastucture and the user code)
* Customizable depending on the business case
* An unit- and integration-tested infrastucture

## Technical core principles

The compare and convert operations consists of an object performing the operation  coupled with an _Intention_.
This intention is specific for a business case and allows distinction between renewing and creating a contract for instance.
In case of a contract renewal, there may be less data being converted from the master data than when creating a new contract.

The copy operations do not have such an intention since the current copy design is focused on cloning objects, and the convert
functionality can be used for a business case-selective copy where some of the data is copied selectively.

Each of the 3C has it's very own object, abstracted using an interface which cares about resolving the needed sub operations
according to the user-made registrations.

| Operation     | Technical                                                      | Base intention          |
| ------------- |---------------------------------------------------------------:| -----------------------:|
| Comparison    | `IComparer<in TModelToCompare, TComparerIntention>`            | `IBaseCompareIntention` |
| Conversion    | `IConvert<in TSourceClass, in TTargetClass, TConvertIntention>`| `IBaseConvertIntention` |
| Copy          | `ICopy<in TClassToCopy>`                                       | no intention            |

### 3C sub operations

The paragraph above states the technical difference between the 3C regarding the busincess case specific intention.
This is a rather minor difference - As mentioned before, the base objects do the job or resolving the sub operations
defined in user-code by the responsible developer on a defined structure.
Basically, one registration is needed for each data class type for a specific intention. The registrations
can be seen as walking through the tree and deciding for each node which properties and child / parent objects need
to be handled in a certain way which can be done by using what is generally called a _sub operation_. These
_sub operations_ are specific to each of the 3C, though some can be mixed (for example, while converting an object implementing
the same interface as the master data it is being converted from the interface's properties can be copied using a sub copy).
