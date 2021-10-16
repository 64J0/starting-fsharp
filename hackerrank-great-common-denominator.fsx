// https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---gcd/problem
open System

let parseArrayToTuple (input: int array) =
    (input.[0], input.[1])

let getInput () =
    Console.ReadLine().Split(" ")
    |> Array.map int
    |> parseArrayToTuple
    
let rec gcd (input: int * int) =
    let mutable biggerNumber = 0
    let mutable smallerNumber = 0
    
    if (fst input) > (snd input) then
        biggerNumber <- (fst input)
        smallerNumber <- (snd input)
    else
        biggerNumber <- (snd input)
        smallerNumber <- (fst input)
        
    let divisionResult = biggerNumber % smallerNumber
    
    match (divisionResult = 0) with
    | true -> smallerNumber
    | false -> gcd (smallerNumber, divisionResult)
    
getInput ()
|> gcd
|> Console.WriteLine