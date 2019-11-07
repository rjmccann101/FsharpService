module ProcessFile

open System.IO
open Microsoft.Extensions.Logging

let simpleFileProcess (logger:ILogger) (changeType:WatcherChangeTypes) (fullPath:string) =
    logger.LogInformation(sprintf "File %s can now be processed for a change of type %A" fullPath changeType)
    ()