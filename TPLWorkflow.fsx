// Script for TPL part of solution
// dotnet add package System.Threading.Tasks.Dataflow

#r "C:\\Users\\rjmcc\\.nuget\\packages\\system.threading.tasks.dataflow\\4.11.0\\lib\\netstandard2.0\\System.Threading.Tasks.Dataflow.dll"
open System.Threading.Tasks.Dataflow

// Each TPL transform block takes an object of type 'a and returns object(s) of type 'b
// An action block takes an object of type 'a
// Here the objects that will be passed into the transform blocks and action blocks are
// defined.

// Object that describes a newly created file
type ZipFile = {
    fileName : string
}