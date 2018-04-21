module CompleteInformation.Core.FSharp.Database

open Couchbase.Lite

let initDatabase name =
    new Database (name)
