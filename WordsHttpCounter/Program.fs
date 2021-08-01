// module WordsCounter

#if INTERACTIVE
#r "nuget: HtmlAgilityPack"
#endif

open System
open System.Net
open HtmlAgilityPack

let fetchHtmlContent (uri: Uri) =
    let httpClient = new Http.HttpClient()
    httpClient.GetStringAsync(uri)

let htmlNodeIsLeaf (node: HtmlNode) =
    (not node.HasChildNodes)
    && not (String.IsNullOrWhiteSpace(node.InnerHtml))

let countWords (textNodes: seq<HtmlNode>) =
    Seq.fold 
        (fun (acc) (node: HtmlNode) -> acc + node.InnerText.Split(" ").Length) 
        0 // acc initial value
        textNodes

let printResults url quantityOfWords =
    printfn "\nUrl: %s \nQuantity of words: %i" url quantityOfWords

let program (url: string) =
    async {
        let uri = Uri(url)
        let! rawHtml = fetchHtmlContent(uri) |> Async.AwaitTask
        let html = HtmlDocument()
        html.LoadHtml(rawHtml)

        let documentNode = html.DocumentNode

        // xpath -> select html components
        let singleNode =
            documentNode.SelectSingleNode(@"//*[@id=""main-column""]")

        let descendants = singleNode.Descendants()
        let textNodes = descendants |> Seq.where htmlNodeIsLeaf

        let quantityOfWords = countWords textNodes

        printResults url quantityOfWords
    }


let listOfTargetSites =
    [ "https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/literals" (* 576 *)
      "https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/sequences" (* 4675 *) ]

listOfTargetSites
|> List.map program
|> Async.Parallel
|> Async.RunSynchronously
|> ignore
