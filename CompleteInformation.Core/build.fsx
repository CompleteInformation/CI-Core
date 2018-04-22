#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO

// Properties
let buildDir = "./bin"
let project = "CompleteInformation.Core.fsproj"

// Targets
Target.create "Clean" (fun _ ->
    Shell.CleanDir buildDir
)

Target.create "Default" (fun _ ->
    DotNet.build id project
)

// Dependencies
open Fake.Core.TargetOperators

"Clean"
    ==> "Default"

// start build
Target.runOrDefault "Default"
