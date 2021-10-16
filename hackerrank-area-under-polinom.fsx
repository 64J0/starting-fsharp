open System

let listOfAs = [| 1.; 2.; 3.; 4. |]
let listOfBs = [| 0.; 1.; 2.; 3. |]
let boundaries = [| 1.; 10. |]

let printResults (area: float, volume: float) =
    Console.WriteLine(area)
    Console.WriteLine(volume)

let step = 0.001

[ boundaries.[0] .. step .. boundaries.[1] ]
|> List.map
    (fun x -> 
        let y =
            Array.fold2
                (fun state a b ->
                    state + a * Math.Pow(x, b))
                0.0
                listOfAs
                listOfBs

        let area = Math.Abs(y * step)
        let volume = Math.Pow(y, 2.) * Math.PI * step
        (area, volume))
|> List.reduce (fun acc (a, v) -> ((fst acc) + a, (snd acc) + v))
|> printResults