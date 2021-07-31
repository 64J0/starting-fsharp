#if INTERACTIVE
#r "nuget: HtmlAgilityPack"
#endif

open HtmlAgilityPack

let document = HtmlWeb().Load("https://github.com");

let rec leafNodes (node:HtmlNode) =
    seq {
        if node.HasChildNodes then
            yield! node.ChildNodes |> Seq.collect leafNodes    
        else if node.ParentNode.Name <> "script" && node :? HtmlTextNode then
            yield node :?> HtmlTextNode }

let result = 
    document.DocumentNode |> leafNodes
    |> Seq.map 
        (fun e -> 
            e.Text.Split ' ' 
            |> Array.filter (not << System.String.IsNullOrWhiteSpace))
    |> Seq.sumBy Array.length

printfn "Result: %O" result