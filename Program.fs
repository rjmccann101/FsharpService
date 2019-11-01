// Fsharp Service
open System
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

type Worker(logger : ILogger<Worker>) =
    inherit BackgroundService()
    let _logger = logger
    override bs.ExecuteAsync stoppingToken =
        let f : Async<unit> = async {
            while not stoppingToken.IsCancellationRequested do
                _logger.LogInformation("Worker running at: {time}", DateTime.Now)
                do! Async.Sleep(1000)
        }
        Async.StartAsTask f :> Task

let CreateHostBuilder argv : IHostBuilder =
    let builder = Host.CreateDefaultBuilder(argv)
    builder.UseWindowsService()
        .ConfigureLogging(fun loggerFactory -> 
            (loggerFactory.ClearProviders().AddLog4Net() |> ignore<ILoggingBuilder>))
        .ConfigureServices(fun hostContext services -> services.AddHostedService<Worker>() 
                                                        |> ignore<IServiceCollection>)
[<EntryPoint>]
let main argv =
    let hostBuilder = CreateHostBuilder argv
    hostBuilder.Build().Run()
    0 // return an integer exit code
