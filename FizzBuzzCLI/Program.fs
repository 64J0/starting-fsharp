// Learn more about F# at http://fsharp.org
open FizzBuzzDLL
open System

[<EntryPoint>]
let main argv =
    Application.application Console.ReadLine Console.WriteLine ()
    0 // return an integer exit code
