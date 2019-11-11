---
Order: 20
Title: Technical core principles
Description: Technical core principles of BBT.StructureTools
---

The compare and convert operations consists of an object performing the operation coupled with an _Intention_.
This intention is specific for a business case and allows distinction between renewing and creating a contract for instance.
In case of a contract renewal, there may be less data being converted from the master data than when creating a new contract.

The copy operations do not have such an intention since the current copy design is focused on cloning objects, and the convert
functionality can be used for a business case-selective copy where some of the data is copied selectively.

Each of the operations has it's very own object, abstracted using an interface which cares about resolving the needed sub operations
according to the user-made registrations.

| Operation     | Technical                                                      | Base intention          |
| ------------- |---------------------------------------------------------------:| -----------------------:|
| Comparison    | `IComparer<in TModelToCompare, TComparerIntention>`            | `IBaseCompareIntention` |
| Conversion    | `IConvert<in TSourceClass, in TTargetClass, TConvertIntention>`| `IBaseConvertIntention` |
| Copy          | `ICopy<in TClassToCopy>`                                       | no intention            |

### Sub operations

The paragraph above states the technical difference between the operations regarding the business case specific intention.
This is a rather minor difference - As mentioned before, the base objects do the job or resolving the sub operations
defined in user-code by the responsible developer on a defined structure.
Basically, one registration is needed for each data class type for a specific intention. The registrations
can be seen as walking through the tree and deciding for each node which properties and child / parent objects need
to be handled in a certain way which can be done by using what is generally called a _sub operation_. These
_sub operations_ are specific to each of the operation, though some can be mixed (for example, while converting an object implementing
the same interface as the master data it is being converted from the interface's properties can be copied using a sub copy).
