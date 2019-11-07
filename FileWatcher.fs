module FileWatcher

open System.IO
open Microsoft.Extensions.Logging

type FileWatchEventFunction = FileInfo -> unit

type FileWatcherParameters = {
    logger : ILogger;
    directory : DirectoryInfo;
    includeSubDirs : bool;
    filter : string option;
    actionFunction : FileWatchEventFunction
}

let createFileWatcher (parms : FileWatcherParameters) =
    let mutable watcher = new FileSystemWatcher(parms.directory.FullName)
    
    let onChangeFunc(src:obj)(evt:FileSystemEventArgs) =
        do parms.logger.LogInformation(sprintf "File: %s %A" evt.FullPath evt.ChangeType)
        do parms.actionFunction(new FileInfo(evt.FullPath))
        ()
    
    let onRenameFunc(src:obj)(evt:RenamedEventArgs) =
        do parms.logger.LogInformation(sprintf "File: %s renamed to %s" evt.OldFullPath evt.FullPath)
        do parms.actionFunction(new FileInfo(evt.FullPath))
        ()

    let onChangeFuncDelegate = new FileSystemEventHandler(onChangeFunc)
    let onRenameFuncDelegate = new RenamedEventHandler(onRenameFunc)

    do watcher.NotifyFilter <- NotifyFilters.LastAccess
                                ||| NotifyFilters.LastWrite
                                ||| NotifyFilters.FileName
                                ||| NotifyFilters.DirectoryName

    do watcher.Filter <- match parms.filter with
                         | Some filterStr -> filterStr
                         | None -> @"*.*"

    do watcher.IncludeSubdirectories <- parms.includeSubDirs
    do watcher.Created.AddHandler(onChangeFuncDelegate)
    do watcher.Changed.AddHandler(onChangeFuncDelegate)
    do watcher.Deleted.AddHandler(onChangeFuncDelegate)
    do watcher.Renamed.AddHandler(onRenameFuncDelegate)
    do watcher.EnableRaisingEvents <- true
    watcher