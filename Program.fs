namespace Basics

  module Starting =
    let myOne: int = 1
    printfn "myOne: %d" myOne
    let hello: string = "hello world"
    let letterA: char = 'a'
    let isEnabled: bool = true

    let add x y = x + y

    // lambda = anonymous function
    let add' = fun x y -> (+) x y

    // currying
    let add'' x = fun y -> x + y