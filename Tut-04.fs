module Tutorial4
  type Day = {DayOfMonth: int; Month: int }
  type Person = {Name: string; Age: int}
  type Person2 = {Name: string; Age: int}

  let ben: Person = { Name = "Ben"; Age = 26 }

  printfn "This person's name is %s and his age is %d" ben.Name ben.Age

  type NormalRectangle = {Base: double; Height: double}

  // This works with Single Case Pattern Matches
  // Record types
  // Tuples
  // Single Case DU
  let area {Base = b; Height = h} = 
    b * h

  let area' (b,h) = b * h

  type Rectangle =
    | Normal of NormalRectangle
    | Square of side:double
  
  module Rectangle = 
    let area = function
      | Normal {Base = b; Height = h} -> b * h
      | Square s -> s ** 2.

  type Shape =
    | Rectangle of Rectangle
    | Triangle of height:double * _base:double
    | Circle of radius:double
    | Dot

  let circle = Circle 1.
  let triangle = Triangle (2.,4.)

  module Shape =
    let area shape =
      match shape with
      | Rectangle rect -> Rectangle.area rect
      | Triangle (h,b) -> h * b / 2.
      | Circle r -> r ** 2. * System.Math.PI
      | Dot -> 0.

  printfn "The circle area is %f" (Shape.area circle)
  printfn "The triangle area is %f" (Shape.area triangle)

  // type Option<'a> =
  //   | Some of 'a
  //   | None

  let translateFizzBuzz' = function
    | "Fizz" -> Some 3
    | "Buzz" -> Some 5
    | "FizzBuzz" -> Some 15
    | _ -> None

  translateFizzBuzz' "Fizz" // Some 3
  translateFizzBuzz' "Buzz" // Some 5
  translateFizzBuzz' "FizzBuzz" // Some 15
  translateFizzBuzz' "Tomato" // None

  let hasValue = function
    | Some _ -> true
    | None -> false

  // type LikedList<'a> =
  //   | ([])
  //   | (::) of head:'a * tail:'a list

  // let empty = []

  let addToList x xs =
    x::xs

  let sampleList = [2;3;4]
  addToList 1 sampleList

  let getFirstItem = function
    | x::_ -> Some x
    | _ -> None

  let getFirstItem' list =
    List.head list

  // let x: int = List.head []

  // Recursive function
  let rec printEveryItem = function
    | x::xs -> 
      printf "%O" x
      printEveryItem xs
    | [] -> ()

  let rec doWithEveryItem f = function
    | x::xs -> 
      f x
      doWithEveryItem f xs
    | [] -> ()

  let printEveryItem' list =
    doWithEveryItem (printfn "%O") list

  let printEveryItem'' list =
    list
    |> List.iter (printfn "%O")

  printEveryItem'' [2;3;4]

  let stringifyList (list: int list) =
    list
    |> List.map string

  [1 .. 10]
  |> stringifyList

  Some 42 |> Option.map string

  let sumList list =
    list
    |> List.fold (+) 0

  sumList [1;2;3] // 6

  let reduceList list =
    list
    |> List.reduce (+) 