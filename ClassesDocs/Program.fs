// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/classes
open System

module ClassConstructors = 
    type MyClass1(x: int, y: int) =
        let resultString = sprintf "x: %d, y: %d" x y // constructor
        do printfn "Result string: %s" resultString // same constructor

        new() = MyClass1(0, 0) // additional constructor

    MyClass1(1, 1)
    MyClass1()

module ClassSelf =
    (** The self identifier that is declared with the as keyword is not initialized until
        after the base constructor.
        Therefore, when used before or inside the base constructor, will raise an exception.
    **)

    type MyClass2(x: int, y: int) as self = // self = self identifier for the whole class
        do self.getResultString() // same constructor

        member this.getResultString() =
            printfn "Result string: %s" (sprintf "x: %d, y: %d" x y)

    MyClass2(1, 1)

module GenericTypeParameters =
    type MyGenericClass<'a> (x: 'a) =
        do printfn "%A" x

    let g1 = MyGenericClass( seq { for i in 1 .. 10 -> (i, i*i) } )

module Inheritance =
    // In F# only one direct base class is allowed.
    open ClassSelf
    type InheritedClass (x: int, y: int) =
        inherit MyClass2(x, y)

    let g1 = InheritedClass(2, 2)

(*
    When to Use Classes, Unions, Records, and Structures

    Given the variety of types to choose from, you need to have a good understanding of what each type is designed 
    for to select the appropriate type for a particular situation. Classes are designed for use in object-oriented 
    programming contexts. Object-oriented programming is the dominant paradigm used in applications that are written 
    for the .NET Framework. If your F# code has to work closely with the .NET Framework or another object-oriented 
    library, and especially if you have to extend from an object-oriented type system such as a UI library, classes 
    are probably appropriate.

    If you are not interoperating closely with object-oriented code, or if you are writing code that is self-contained 
    and therefore protected from frequent interaction with object-oriented code, you should consider using records and
    discriminated unions. A single, well thought–out discriminated union, together with appropriate pattern matching 
    code, can often be used as a simpler alternative to an object hierarchy. For more information about discriminated 
    unions, see Discriminated Unions.

    Records have the advantage of being simpler than classes, but records are not appropriate when the demands of a 
    type exceed what can be accomplished with their simplicity. Records are basically simple aggregates of values, 
    without separate constructors that can perform custom actions, without hidden fields, and without inheritance or 
    interface implementations. Although members such as properties and methods can be added to records to make their 
    behavior more complex, the fields stored in a record are still a simple aggregate of values. For more information 
    about records, see Records.

    Structures are also useful for small aggregates of data, but they differ from classes and records in that they 
    are .NET value types. Classes and records are .NET reference types. The semantics of value types and reference 
    types are different in that value types are passed by value. This means that they are copied bit for bit when 
    they are passed as a parameter or returned from a function. They are also stored on the stack or, if they are 
    used as a field, embedded inside the parent object instead of stored in their own separate location on the 
    heap. Therefore, structures are appropriate for frequently accessed data when the overhead of accessing the 
    heap is a problem. For more information about structures, see Structures.
*)
