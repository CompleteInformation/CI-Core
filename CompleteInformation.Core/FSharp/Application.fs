module CompleteInformation.Core.FSharp.Application

open Couchbase.Lite

type T = {
    databases: Map<string, Couchbase.Lite.Database>;
    extensions: Extension.T list;
}

type Setup = {
    databases: string list;
    extensions: string list;
}

let createSetup databases extensions =
    { Setup.databases = databases; extensions = extensions }

let setUpApplication (setup:Setup) =
    Couchbase.Lite.Support.NetDesktop.Activate()
    let databases =
        setup.databases
        |> List.fold (fun (map:Map<string,Database>) d -> map.Add(d, new Database (d))) Map.empty
    { T.databases=databases; extensions=[] }
