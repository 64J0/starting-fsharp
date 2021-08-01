// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/sequences
//
// A sequence is a logical series of elements all of one type.
// Sequences are particularly useful when you have a large, ordered collection of data but do now necessarily expect
// to use all of the elements. Individual sequence elements are computed only as required, so a sequence can provide
// better performance than a list in situations in which not all the elements are used.
// Sequences are represented by the seq<'T> type, which is an alias for IEnumerable<T>. Therefore, any .NET type that
// implements IEnumerable<T> interface can be used as a sequence.

// INTRO
//
// Sequence that has an increment
let seq1 = seq { 0 .. 10 .. 100}
Seq.iter (fun x -> printfn "%i" x) seq1

// Generate values programatically
// You can use -> if every part of the code that follows it returns a value
let seq2 = seq { for i in 1 .. 10 -> i * i }
Seq.iter (fun x -> printfn "%i" x) seq2

// Alternatively you can specify the do keyword, with an optional yield that follows
let seq3 = seq { for i in 1 .. 10 do yield i * i }
Seq.iter (fun x -> printfn "%i" x) seq3

// The yield can be implicit and doesn't need to be specified in most cases
let seq3' = seq { for i in 1 .. 10 do i * i }
Seq.iter (fun x -> printfn "%i" x) seq3'

let (height, width) = (3, 3)

let seq4 =
    seq {
        for row in 0 .. width - 1 do
            for col in 0 .. height - 1 ->
                (row, col, row*width + col)
        }
Seq.iter (fun x -> printfn "%O" x) seq4

// FILTER ...................................................
// An if expression used in a sequence is a filter
// Example:
//
// Prime numbers sequence
// Recursive isprime function.
let isprime n =
    let rec check i =
        printfn "Value of i: %i" i
        i > n/2 || (n % i <> 0 && check (i + 1))
    check 2

let aSequence =
    seq { 
        for n in 1..100 do if isprime n then n }

for x in aSequence do
    printfn "%i" x

// Sometimes you may wish to include a sequence of elements into another sequence.
// Example:
let seq5 = seq {
    for _ in 1 .. 10 do
        yield! seq { 1; 2; 3; 4; 5 }
}
Seq.iter (fun x -> printfn "%O" x) seq5 // repeats '1 2 3 4 5' ten times

// Another way of thinking of yield! is that it flattens an inner sequence and then includes that in the containing sequence.
let seq5' = seq {
    for x in 1 .. 10 do
        yield x
        yield! seq { for i in 1..x -> i }
}
Seq.iter (fun x -> printfn "Value: %i" x) seq5'

// EXAMPLES ..............................................
//
module exampleTree =
    // Yield the values of a binary tree in a sequence.
    type Tree<'a> =
       | Tree of 'a * Tree<'a> * Tree<'a>
       | Leaf of 'a

    // inorder : Tree<'a> -> seq<'a>
    let rec inorder tree =
        seq {
          match tree with
              | Tree(x, left, right) ->
                   yield! inorder left
                   yield x
                   yield! inorder right
              | Leaf x -> yield x
        }

    let mytree = Tree(6, Tree(2, Leaf(1), Leaf(3)), Leaf(9))
    let seq1 = inorder mytree
    printfn "%A" seq1

// Convert an array to a sequence by using a cast
let seqFromArray1 = [| 1..10 |] :> seq<int> // cast operator

// Convert an array to a sequence by using Seq.ofArray
let seqFromArray2 = [| 1..10 |] |> Seq.ofArray

let fib =
    (1, 1) // initial state
    |> Seq.unfold (fun state ->
        if (snd state > 1000) then
            None
        else
            Some (fst state + snd state, (snd state, fst state + snd state)) )

printfn "\nThe sequence fib contains Fibonacii numbers."
for x in fib do printfn "%d " x
// The sequence fib contains Fibonacci numbers.
// 2 3 5 8 13 21 34 55 89 144 233 377 610 987 1597