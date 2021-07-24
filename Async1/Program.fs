// https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/

module example1 =
    open System

    // * A lambda is registered with the Timer.Elapsed event, and when the event is triggered,
    // the AutoResetEvent is signalled
    // * The main thread starts the timer, does something else while waiting, and then blocks
    // until the event is triggered
    // * Finally, the main thread continues, about 2 seconds later

    let userTimerWithCallback = 
        // create an event to wait on
        let event = new System.Threading.AutoResetEvent(false) // synchronization mechanism

        // create a timer and add an event handler that will signal the event
        let timer = new System.Timers.Timer(2000.0)
        timer.Elapsed.Add (fun _ -> event.Set() |> ignore)

        // start
        printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
        timer.Start()

        // keep working
        printfn "Doing something useful while waiting for event"

        // block on the timer via the AutoResetEvent
        event.WaitOne() |> ignore

        // done
        printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

// ======================================
// ASYNCHRONOUS WORKFLOWS
module example2 =
    open System

    // * The AutoResetEvent and lambda have disappeared, and are replaced by let timerEvent =
    // Control.Async.AwaitEvent (timer.Elapsed), which creates an async object directly from the
    // event, without needing a lambda. The ignore is added to ignore the result.
    // * The event.WaitOne() has been replaced by Async.RunSynchronously timerEvent which blocks
    // on the async object until it has completed.

    let userTimerWithAsync =
        // create a timer and associated async event
        let timer = new System.Timers.Timer(2000.0)
        let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore

        // start
        printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
        timer.Start()

        // keep working
        printfn "Doing something useful while waiting for event"

        // block on the timer event now by waiting for the async to complete
        Async.RunSynchronously timerEvent

        // done
        printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

// ======================================
module example3 =

    let fileWriteWithAsync =

        // create a stream to write to
        use stream = new System.IO.FileStream("test.txt", System.IO.FileMode.Create)

        // start
        printfn "Starting async write"
        let asyncResult = stream.BeginWrite(Array.empty, 0, 0, null, null)

        // create an async wrapper around an IAsyncResult
        let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

        // keep working
        printfn "Doing something useful while waiting for write to complete"

        // block on the timer now by waiting for the async to complete
        Async.RunSynchronously async

        // done
        printfn "Async write completed"

// ========================================
// CREATING AND NESTING ASYNCHRONOUS WORKFLOWS
module example4 =
    let sleepWorkflow = async {
        printfn "Starting sleep workflow at %O" System.DateTime.Now.TimeOfDay

        // the code do! Async.Sleep 2000 is similar to Thread.Sleep but designed to work with asynchronous workflows
        do! Async.Sleep 2000

        printfn "Finished sleep workflow at %O" System.DateTime.Now.TimeOfDay
    }

    Async.RunSynchronously sleepWorkflow

    let nestedWorkflow = async {
        printfn "Starting parent"
        let! childWorkflow = Async.StartChild sleepWorkflow

        // give the child a chance and then keep working
        do! Async.Sleep 100
        printfn "Doing something useful while waiting"

        // block on the child
        let! result = childWorkflow

        // done
        printfn "Finished parent!"
    }

    Async.RunSynchronously nestedWorkflow

// ========================================
// CANCELLING WORKFLOWS
module example5 =
    open System
    open System.Threading

    let testLoop = async {
        for i in [1..100] do
            // do something
            printfn "%i before.." i

            // sleep a bit
            do! Async.Sleep 10
            printfn "..after"
    }

    Async.RunSynchronously testLoop

    // create a cancellation source
    let cancellationSource = new CancellationTokenSource()

    // start the task, but this time pass in a cancellation token
    Async.Start (testLoop, cancellationSource.Token)

    // wait a bit
    Thread.Sleep(200)

    // cancel after 200ms
    cancellationSource.Cancel()

// ======================================
// COMPOSING WORKFLOWS IN SERIES AND PARALLEL
module example6 =
    let sleepWorkflowMs ms = async {
        printfn "%i ms workflow started" ms
        do! Async.Sleep ms
        printfn "%i ms workflow finished" ms
    }

    // series
    let workflowInSeries = async {
        do! sleepWorkflowMs 1000
        printfn "Finished one"

        do! sleepWorkflowMs 2000
        printfn "Finished two"
    }

    // #time
    printfn "Started time %O" System.DateTime.Now.TimeOfDay
    Async.RunSynchronously workflowInSeries
    // #time
    printfn "Finished time %O" System.DateTime.Now.TimeOfDay

    // parallel
    let workflowInParallel = 
        let sleep1 = sleepWorkflowMs 1000
        let sleep2 = sleepWorkflowMs 2000

        // run them in parallel
        // #time
        printfn "Started time %O" System.DateTime.Now.TimeOfDay
        [sleep1; sleep2]
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore
        printfn "Finished time %O" System.DateTime.Now.TimeOfDay
        // #time

    // Started time 14:31:27.3634445
    // 1000 ms workflow started
    // 1000 ms workflow finished
    // Finished one
    // 2000 ms workflow started
    // 2000 ms workflow finished
    // Finished two
    // Finished time 14:31:30.3809957
    // Started time 14:31:30.3813663
    // 1000 ms workflow started
    // 2000 ms workflow started
    // 1000 ms workflow finished
    // 2000 ms workflow finished
    // Finished time 14:31:32.3815869

// ======================================
module example7 =
    open System.Net
    open System
    open System.IO

    // not optmized code
    let fetchUrl url =
        let req = WebRequest.Create(Uri(url))
        use resp = req.GetResponse()
        use stream = resp.GetResponseStream()
        use reader = new IO.StreamReader(stream)
        let html = reader.ReadToEnd()
        printfn "Finished downloading %s" url

    // a list of sites to fetch
    let sites = ["https://google.com";
                 "https://bing.com";
                 "https://microsoft.com";
                 "https://amazon.com";
                 "https://yahoo.com"]

    printfn "Starting time: %O" DateTime.Now.TimeOfDay
    sites
    |> List.iter fetchUrl
    printfn "Finishing time: %O" DateTime.Now.TimeOfDay

    // Starting time: 14:37:40.8510629
    // Finished downloading https://google.com
    // Finished downloading https://bing.com
    // Finished downloading https://microsoft.com
    // Finished downloading https://amazon.com
    // Finished downloading https://yahoo.com
    // Finishing time: 14:37:48.2443263

    // This solution is inefficient - only one web site at a time is visited.
    // The program would be faster if we could visit them all at the same time.

    open Microsoft.FSharp.Control.CommonExtensions
    // adds AsyncGetResponse

    // fetch the contents of a web page asynchronously
    // optimized code
    let fetchUrlAsync url =
        async {
            let req = WebRequest.Create(Uri(url))
            use! resp = req.AsyncGetResponse()
            use stream = resp.GetResponseStream()
            use reader = new IO.StreamReader(stream)
            let html = reader.ReadToEnd()
            printfn "finished downloading %s" url
        }

    printfn "Starting time: %O" DateTime.Now.TimeOfDay
    sites
    |> List.map fetchUrlAsync
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
    printfn "Finishing time: %O" DateTime.Now.TimeOfDay

    // Starting time: 14:46:48.8060561
    // finished downloading https://google.com
    // finished downloading https://bing.com
    // finished downloading https://amazon.com
    // finished downloading https://microsoft.com
    // finished downloading https://yahoo.com
    // Finishing time: 14:46:51.0789237

// =======================================
// PARALLEL COMPUTATION
module example8 =
    let childTask () =
        // chew up some CPU
        for i in [1..500] do
            for j in [1..500] do
                "Hello".Contains("H") |> ignore
                // we don't care about the answer!

    // Test the child task on its own.
    // Adjust the upper bounds as needed
    // to make this run in about 0.2 sec
    printfn "Starting time childTask: %O" System.DateTime.Now.TimeOfDay
    childTask ()
    printfn "Finishing time childTask: %O" System.DateTime.Now.TimeOfDay

    let parentTask =
        childTask
        |> List.replicate 20
        |> List.reduce (>>)

    printfn "Starting time parentTask: %O" System.DateTime.Now.TimeOfDay
    parentTask ()
    printfn "Finishing time parentTask: %O" System.DateTime.Now.TimeOfDay

    // make childTask parallelizable
    let asyncChildTask = async { return childTask() }

    let asyncParentTask = 
        asyncChildTask
        |> List.replicate 20
        |> Async.Parallel

    printfn "Starting time asyncParentTask: %O" System.DateTime.Now.TimeOfDay
    asyncParentTask
    |> Async.RunSynchronously
    |> ignore
    printfn "Finishing time asyncParentTask: %O" System.DateTime.Now.TimeOfDay

    // Starting time childTask: 14:58:56.0180027
    // Finishing time childTask: 14:58:56.0261210
    // Starting time parentTask: 14:58:56.0264111
    // Finishing time parentTask: 14:58:56.2185096
    // Starting time asyncParentTask: 14:58:56.2189016
    // Finishing time asyncParentTask: 14:58:56.3276461

    // https://docs.microsoft.com/pt-br/dotnet/api/system.timespan?view=net-5.0
    // childTask ->
    module childTaskResult = 
        let startingTS = System.TimeSpan.Parse("14:58:56.0180027")
        let finishingTS = System.TimeSpan.Parse("14:58:56.0261210")
        let result = finishingTS - startingTS
        printfn "childTask result is: %O" result
        // val result : TimeSpan = 00:00:00.0081183

    module parentTaskResult =
        let startingTS = System.TimeSpan.Parse("14:58:56.0264111")
        let finishingTS = System.TimeSpan.Parse("14:58:56.2185096")
        let result = finishingTS - startingTS
        printfn "parentTask result is: %O" result
        // val result : TimeSpan = 00:00:00.1920985
    
    module asyncParentTaskResult =
        let startingTS = System.TimeSpan.Parse("14:58:56.2189016")
        let finishingTS = System.TimeSpan.Parse("14:58:56.3276461")
        let result = finishingTS - startingTS
        printfn "asyncParentTask result is: %O" result
        // val result : TimeSpan = 00:00:00.1087445