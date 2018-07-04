module CompleteInformation.Core.FSharp.Application

open Couchbase.Lite

type T = {
    databases: string list;
    extensions: Extension.T list;
}

let createApplication databases extensions =
    { databases = databases; extensions = extensions }

let initialize (application:T) =
    let dict =
        application.extensions
        |> List.fold (fun lst m ->
                List.concat [|m.databases; lst|]
            ) []
        |> List.fold (fun (map: Map<string, Database>) name ->
                let db = new Database (name)
                map.Add(name, db)
        ) Map.empty
    0
