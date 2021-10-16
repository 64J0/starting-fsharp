// https://www.hackerrank.com/challenges/functional-programming-warmups-in-recursion---fibonacci-numbers/problem
open System

let getInput () =
    Console.ReadLine()
    |> int
    
// Fibonacci(n) = Fibonacci(n-1) + Fibonacci(n-2)  
let rec calculateFibonacci (n: int) =
    if n = 1 then 0
    elif n = 2 then 1
    else 
        calculateFibonacci(n - 1) + calculateFibonacci(n - 2)
        
getInput ()
|> calculateFibonacci
|> Console.WriteLine