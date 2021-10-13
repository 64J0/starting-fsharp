// https://www.hackerrank.com/challenges/eval-ex/problem
open System

let rec getInput () =
    seq {
        let consoleValue = Console.ReadLine()
        match String.IsNullOrEmpty consoleValue with
        | false -> 
            yield float consoleValue
            yield! getInput ()
        | true -> ()
    }

let rec fatorial (n: float) =
    if (n > 0.) then
        n * (fatorial (n - 1.))
    else 1. // todo: fix
    
let calculateExpansion (input: float) =
    let mutable result = 1.
    
    for n in [1. .. 9.] do
        let leftSide = Math.Pow(input, n) / (fatorial n)
        result <- result + leftSide

    result

getInput ()
|> Seq.tail
|> Seq.iter
    (fun (i: float) -> 
        let result = sprintf "%.4f" (calculateExpansion i)
        Console.WriteLine(result))