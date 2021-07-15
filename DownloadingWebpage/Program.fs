// Based on https://fsharpforfunandprofit.com/posts/fvsc-download/

open System.Net
open System

let fetchUrl callback url =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    callback reader url

let myCallback (reader: IO.StreamReader) url =
    let html = reader.ReadToEnd()
    let html1000 = html.Substring(0, 1000)
    printfn "Downloaded %s. First 1000 is %s" url html1000
    html // return all the html

let google = fetchUrl myCallback "https://google.com"

// baked parameters
let fetchUrl2 = fetchUrl myCallback
let myBlog = fetchUrl2 "https://gaio.dev"
