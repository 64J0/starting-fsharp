// module WordsCounter

#if INTERACTIVE
#r "nuget: HtmlAgilityPack"
#endif

open System
open System.Threading.Tasks
open System.Net
open HtmlAgilityPack

let filterHtmlNodes (node: HtmlNode) =
    (not node.HasChildNodes)
    && not (String.IsNullOrWhiteSpace(node.InnerHtml))

let makeHttpRequest (url: string) =
    async {
        let uri = Uri(url)
        let httpClient = new Http.HttpClient()
        let! rawHtml = httpClient.GetStringAsync(uri) |> Async.AwaitTask
        let html = HtmlDocument()
        html.LoadHtml(rawHtml)

        let documentNode = html.DocumentNode

        // xpath -> select html components
        let singleNode =
            documentNode.SelectSingleNode(@"//*[@id=""main-column""]")

        let descendants = singleNode.Descendants()
        let textNodes = descendants |> Seq.where filterHtmlNodes

        let textNodesWithInnerText =
            textNodes
            |> Seq.where (fun x -> not (String.IsNullOrEmpty(x.InnerText)))

        let fullText =
            String.Join("\n", textNodesWithInnerText)

        let mutable counter = 0

        textNodes
        |> Seq.iter
            (fun x ->
                printfn "Innertext: %s" x.InnerText
                counter <- counter + (x.InnerText.Split(" ").Length))

        // printfn "fullText: %O" fullText
        printfn "counter: %O" counter
    }


let sites =
    [ "https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0" ]

let program =
    sites
    |> List.map makeHttpRequest
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

program
