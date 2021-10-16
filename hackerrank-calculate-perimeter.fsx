open System

// let getInput () =
//     Console.ReadLine().Split(" ")
//     |> Array.map float
    
// let qntOfPoints = getInput().[0]

// let coords =
//     seq {
//         for _ in [ 1. .. qntOfPoints ] do
//             let input = getInput ()
//             yield input    
//     }
//     |> Seq.toArray

let coords = [| 
    [| 0.; 0. |]
    [| 0.; 1. |]
    [| 1.; 1. |]
    [| 1.; 0. |]
 |]
    
let calculatePerimeter (coords: (float array) array) =
    let mutable partialPerimeter = 0.
    
    for i in [ 0 .. (coords.Length - 1) ] do
        let nextIndex =
            if i = (coords.Length - 1) then 0
            else i + 1
                
        let deltaX = coords.[nextIndex].[0] - coords.[i].[0]
        let deltaY = coords.[nextIndex].[1] - coords.[i].[1]
        partialPerimeter <- 
            partialPerimeter + 
            Math.Sqrt(Math.Pow(deltaX, 2.) + Math.Pow(deltaY, 2.))
            
    partialPerimeter    
    
let perimeter = calculatePerimeter (coords)
Console.WriteLine(perimeter)