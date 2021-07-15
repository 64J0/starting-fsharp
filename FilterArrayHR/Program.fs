// Resolution of https://www.hackerrank.com/challenges/fp-filter-array/problem

open System

let getInputDelimiter =
    Console.ReadLine()
    |> int

let getInputList: List<int> =
    let result = [
        let mutable item = Console.ReadLine()
        while not (isNull item) do
            yield item |> int
            item <- Console.ReadLine()
    ]
    result

// let builtinFilter (delimiter: int) (inputList: List<int>) =
//     inputList
//     |> List.filter (fun i -> i < delimiter)

let customFilter (delimiter: int) (inputList: List<int>) =
    let mutable (result: List<int>) = []
    List.iter (fun item ->
        Console.WriteLine(item < delimiter)
        if (item < delimiter) then result <- result @ [item]
    ) inputList
    result

let main =
    let delimiter = getInputDelimiter
    let list = getInputList
    customFilter delimiter list
    |> List.iter (fun i -> Console.WriteLine(i))
