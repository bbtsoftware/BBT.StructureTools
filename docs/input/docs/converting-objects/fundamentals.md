---
Order: 30
Title: Fundamentals
Description: Introduction to the convert capabilities
---

## Distinction towards .NET Framework interfaces

The .NET Framework, as of version 4.8, doesn't offer any conversion capabilities other than
converting reference and value types into their CLR type of an equivalent value using [System.IConvertible].

The Compare infrastructure in BBT.StructureTools is aiming at the conversion of business objects, specifically
data, which has a different meaning depending on the context.

This is an important distinction, since the CLR types don't exhibit any business specific behavior.
Let's take a web shop for example.
If the address to which the order is being shipped is determined by looking
up the address via the `User` who placed the order the issue where
the user may change the address at any given time without considering the impact on the `Order` isn't accounted for.

<div class="mermaid">
classDiagram
    class User
    class Address
    class Order
    class OrderAddress
    Address <|-- OrderAddress
    User "1" --> "1" Address
    User "1" --> "*" Order
    Order "1" --> "1" OrderAddress
</div>

If in turn the `Order` knows it's own address to which it needs to be shipped this issue is solved, but
we stand at another issue: An address may need different conversion on various use cases:

* Order shipping
* Import from an external system (delta or full import)

In addition, if any properties are added to the `Address` you would want to have some sort of automated
test telling you that this new property potentially needs to be added to a `OrderAddress`, or vice-versa.

[System.IConvertible]: https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible
