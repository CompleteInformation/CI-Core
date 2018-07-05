module CompleteInformation.Core.FSharp.Application

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
        |> List.fold (fun (map:Map<string,Couchbase.Lite.Database>) d -> map.Add(d, new Couchbase.Lite.Database (d))) Map.empty
    { T.databases=databases; extensions=[] }
