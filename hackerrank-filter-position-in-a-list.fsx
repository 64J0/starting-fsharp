let rec getInput () =
    seq {
        let v = System.Console.ReadLine()

        match System.String.IsNullOrEmpty v with
        | false ->
            yield int v 
            yield! getInput ()
        | true -> ()
    }

getInput ()
|> Seq.mapi (fun index value -> (index, value))
|> Seq.filter (fun (index, value) -> index % 2 <> 0)
|> Seq.iter (fun (index, value) -> System.Console.WriteLine (value))