---
Order: 40
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

## Registration interfaces

The paragraph above states the technical difference between the operations regarding the business case specific intention.
This is a rather minor difference - As mentioned before, the base objects do the job or resolving the sub operations
defined in user-code by the responsible developer on a defined structure.
One registration is needed for each data class type for a specific intention. The registrations
can be seen as walking through the tree and deciding for each node which properties and child / parent objects need
to be handled in a certain way which can be done by using what is generally called a _sub operation_. These
_sub operations_ are specific to each of the operation, though some can be mixed (for example, while converting an object implementing
the same interface as the master data it is being converted from the interface's properties can be copied using a sub copy).

There is one registration interface per each operation supported by the library. All of these registration
interfaces share the same raison d'être and work as containers which hold the definition for each operation.
They are internally retrieved from the IoC, shall be stateless and shall exist per data class or interface which is
being processed.

| Operation     | Technical                                                     |
| ------------- |--------------------------------------------------------------:|
| Comparison    | `ICompareRegistrations<TModelToCompare, TCompareIntention>`   |
| Conversion    | `IConvertRegistrations<TSource, TTarget, TConvertIntention>`  |
| Copy          | `ICopyRegistrations<T>`                                       |

:::{.alert .alert-info}
Consider these tips when writing registrations
___

* Register your implementation of the registration in the IoC container.
* Do not traverse over multiple tree leafs when registering properties, except for reference level sub operations.
* It's usually better to unit test each registration, not the entire tree being copied.
* Keep your implementation stateless
* Do not mix registrations for multiple types into one implementation - See [Single Responsibility Principle]
:::

## Controlling object initialization

The copy and convert registrations can create a new instance of a target object if neccessary.
How an object is initialized is defined by a registration of an `IInstanceCreator`
implementation into the IoC container.
The Copy and Convert infrastructure resolves the correct instance creator when it needs
to create a new object while copying or converting an object.

:::{.alert .alert-info}
Consider these tips when instantiating objects
___

* Overwrite the `IInstanceCreator` with the matching generic type parameters in the IoC
registrations so the library can detect it while converting or copying objects.
* An empty constructor on the target class is mandatory if the infrastructure resorts to use the
default generic `InstanceCreator` because the default constructor is used there.
:::

## Processings and Interceptions

Copy, Convert, and Compare can be controlled depending on the objects being processed by using
post-, preprocessings, or interceptors.
Please note that these operations are not taken from the IoC but passed into the operation
as additional argument.

:::{.alert .alert-info}
If possible, use the generic implementations supplied with the libary.
:::

### Preprocessings

Preprocessings are executed before the object is processed.

| Operation     | Interface                                                     |
| ------------- |--------------------------------------------------------------:|
| Comparison    | not supported                                                 |
| Conversion    | `IConvertPreProcessing<TSoureClass, TTargetClass>`            |
| Copy          | not supported                                                 |

### Postprocessings

Postprocessings are executed after the object is processed.

| Operation     | Interface                                                     |
| ------------- |--------------------------------------------------------------:|
| Comparison    | `IComparePostProcessing<T, TIntention>`                       |
| Conversion    | `IConvertPostProcessing<TSoureClass, TTargetClass>`           |
| Copy          | `ICopyPostProcessing<TClassToCopy>`                           |

### Interceptors

Interceptors analyze a given object and stop the copy or convert process from continuing
if a specified condition is met.

| Operation     | Interface                                                                         |
| ------------- |----------------------------------------------------------------------------------:|
| Comparison    | not supported                                                                     |
| Conversion    | `IConvertInterception<TSoureClass, TTargetClass>`                                 |
| Copy          | `IGenericContinueCopyInterception<TType>`, `ICopyChildInterception<TClassToCopy>` |

[Single Responsibility Principle]: https://scotch.io/bar-talk/s-o-l-i-d-the-first-five-principles-of-object-oriented-design#toc-single-responsibility-principle
[S.O.L.I.D]: https://scotch.io/bar-talk/s-o-l-i-d-the-first-five-principles-of-object-oriented-design#toc-single-responsibility-principle
