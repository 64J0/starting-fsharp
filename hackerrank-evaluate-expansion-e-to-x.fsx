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
    let initialState = 1.

    [1. .. 9.]
    |> Seq.fold 
        (fun (state: float) (n: float) ->
            let rightSide = Math.Pow(input, n) / (fatorial n)
            state + rightSide)
        initialState

getInput ()
|> Seq.tail
|> Seq.iter
    (fun (i: float) -> 
        let result = sprintf "%.4f" (calculateExpansion i)
        Console.WriteLine(result))