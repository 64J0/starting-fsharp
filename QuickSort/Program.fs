// algorithm from https://fsharpforfunandprofit.com/posts/fvsc-quicksort/

// recursive function more verbose to increase the readability for newcomers:
let rec quicksort (list: List<int>) =
    match list with
    | [] ->
        []
    | firstElem::otherElements ->
        let smallerElements =
            otherElements
            |> List.filter (fun e -> e < firstElem)
            |> quicksort
        let largerElements =
            otherElements
            |> List.filter (fun e -> e >= firstElem)
            |> quicksort
        List.concat [smallerElements; [firstElem]; largerElements]

printfn "%A" (quicksort [1;5;23;18;9;1;3])

// optimized version:
let rec quicksort2 = function
   | [] -> []
   | first::rest ->
        let smaller,larger = List.partition ((>=) first) rest
        List.concat [quicksort2 smaller; [first]; quicksort2 larger]

// test code
printfn "%A" (quicksort2 [1;5;23;18;9;1;3])