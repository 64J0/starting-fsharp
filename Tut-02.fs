// Namespaces contain types and modules
// Modules contains values, functions and some types

namespace FSharpBasics

module Arithmetic =
  module Addition = 
    let add x y = x + y

module Other =
  open Arithmetic
  // open Basics

  let program =
    printfn "The result is %d" (Addition.add 5 3)
    // printfn "myOne: %d" Starting.myOne