// Algo:
//
// Read from the console the user input
// Then, get the correct values from this input, removing the spaces and storing the result in a list, except from the firtst value that must be handle in a other way
// Then, apply the constraints to the first value (0 <= x0 <= 10). If an error is detected then stops the function
// Then, apply the constraints to the list values
    // 1. They must be integer parsable
    // 2. They must be constrained to this region (1 <= ::x <= 100)
// If there's an error then the function must stop and return immediately 
// Then, print every value in the list x0 times in the console

open System

type Input = {n: int; list: List<string>}

let getInput: Input =
    let n = Console.ReadLine() |> int
    let list =
        [
         let mutable key = Console.ReadLine()
         while not (isNull key) do
            yield key
            key <- Console.ReadLine()
        ]
    {n=n; list=list}

let application (args:Input) =
    args.list 
    |> List.iter (fun item -> 
        for _ in [1 .. args.n] do
            Console.WriteLine(item)
        )

let main =
    let input = getInput
    application input
