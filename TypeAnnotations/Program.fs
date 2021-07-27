// https://fsharpforfunandprofit.com/posts/type-extensions/

module Person =
    type T = {First:string; Last:string} with
        // member defined with type declarations
        member this.FullName =
            this.First + " " + this.Last

    // constructor
    let create first last =
        {First=first; Last=last}

    type T with
        member this.SortableName =
            this.Last + ", " + this.First

module test1 =
    // test
    let person = Person.create "John" "Doe"
    let fullname = person.FullName
    let sortableName = person.SortableName

// =======================================
module PersonExtensions =
    type Person.T with
        member this.UppercaseName =
            this.FullName.ToUpper()

module test2 =
    // test
    open PersonExtensions

    let person = Person.create "John" "Doe"
    let uppercaseName = person.UppercaseName

// ========================================
// Extending system types
type System.Int32 with
    member this.IsEven = this % 2 = 0

let i = 20
if i.IsEven 
    then printfn "'%i' is even" i 
    else printfn "'%i' is not even" i

match i.IsEven with
| true -> printfn "'%i' is even" i
| false -> printfn "'%i' is not even" i

// ==========================================
// Static members
module Person2 =
    type T = {First:string; Last:string} with
        // member defined with type declaration
        member this.FullName =
            this.First + " " + this.Last

        // static constructor
        static member Create first last =
            {First=first; Last=last}

module test3 =
    open Person2

    let person = T.Create "John" "Doe"
    let fullname = person.FullName

// =========================================
module Person3 =
    // type with no members initially
    type T = {First:string; Last:string}

    // constructor
    let create first last =
        {First=first; Last=last}

    // standalone function
    let fullName {First=first; Last=last} =
        first + " " + last

    // attach preexisting function as a member
    type T with
        member this.FullName = fullName this

module test4 =
    let person = Person3.create "John" "Doe"
    let fullname = Person3.fullName person // functional style
    let fullname2 = person.FullName // OO style

// ==================================
// Attaching existing functions with multiple parameters
module Person4 =
    // type with no members initially
    type T = {First:string; Last:string}

    // constructor
    let create first last =
        {First=first; Last=last}

    // standalone function
    let hasSameFirstAndLastName (person: T) otherFirst otherLast =
        person.First = otherFirst && person.Last = otherLast

    // attach preexisting function as a member
    type T with
        member this.HasSameFirstAndLastName = hasSameFirstAndLastName this

// test
module test5 =
    let person = Person4.create "John" "Doe"
    let result1 = Person4.hasSameFirstAndLastName person "Bob" "Smith"
    let result2 = person.HasSameFirstAndLastName "Bob" "Smith"

// ====================================
// Tuple-form methods
module TupleFormMethods =
    type Product = {SKU:string; Price:float} with

        // curried style
        member this.CurriedTotal qnty discount =
            (this.Price * float qnty) - discount

        // tupple style
        member this.TupleTotal(qnty, discount) =
            (this.Price * float qnty) - discount

    let product = {SKU="ABC"; Price=2.0}
    let total1 = product.CurriedTotal 10 1.0
    let total2 = product.TupleTotal(10, 1.0)

    // when tuple parameters are used the order of the parameters can change
    let total3 = product.TupleTotal(qnty=10,discount=1.0)
    let total4 = product.TupleTotal(discount=1.0, qnty=10)

module TupleFormMethodsOptional =
    type Product = {SKU:string; Price:float} with

        member this.TupleTotalOptional(qnty, ?discount) =
            let extPrice = this.Price * float qnty
            match discount with
            | None -> extPrice
            | Some discount -> extPrice - discount

        member this.TupleTotalOptionalDefaultArg(qnty, ?discount) =
            let extPrice = this.Price * float qnty
            let discount = defaultArg discount 0.0
            // return
            extPrice - discount

    let product = {SKU="ABC"; Price=2.0}

    // discount not specified
    let total1 = product.TupleTotalOptional(10)
    // discount specified
    let total2 = product.TupleTotalOptional(10,1.0)

    // discount not specified
    let total3 = product.TupleTotalOptionalDefaultArg(10)
    // discount specified
    let total4 = product.TupleTotalOptionalDefaultArg(10,1.0)

// =============================================
// Method overloading
module MethodOverloading =
    type Product = {SKU:string; Price:float} with

        // no discount
        member this.TupleTotal(qnty) = 
            printfn "Using non-discount method"
            this.Price * float qnty

        // with discount
        member this.TupleTotal(qnty, discount) = 
            printfn "Using discount method"
            (this.Price * float qnty) - discount

    let product = {SKU="ABC"; Price=2.0}
    let total1 = product.TupleTotal(10)
    let total2 = product.TupleTotal(10, 1.0)