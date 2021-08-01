#if INTERACTIVE
#r "nuget: HtmlAgilityPack"
#endif

open HtmlAgilityPack

let listOfTargets =
    [ "https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/sequences" // 4007
    ]

let loadDocument (url: string) = 
    HtmlWeb().Load(url)

let rec leafNodes (node:HtmlNode) =
    seq {
        if node.HasChildNodes then
            yield! node.ChildNodes |> Seq.collect leafNodes    
        else if node.ParentNode.Name <> "script" && node :? HtmlTextNode then
            yield node :?> HtmlTextNode }

let getResult (document: HtmlDocument) = 
    document.DocumentNode |> leafNodes
    |> Seq.map 
        (fun e -> 
            e.Text.Split ' ' 
            |> Array.filter (not << System.String.IsNullOrWhiteSpace))
    |> Seq.sumBy Array.length

listOfTargets
|> List.map (loadDocument >> getResult)
|> List.iter (fun x -> printfn "The result is: %i" x)