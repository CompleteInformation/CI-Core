module CompleteInformation.Core.FSharp.Modules

open Couchbase.Lite
open System
open System.Composition
open System.Collections.Generic

type IModule =
    abstract member GetDatabaseNames: unit -> string list
    abstract member Initialize: Dictionary<string, Database> -> unit

type ModuleLoader () = class
    let mutable modules: IEnumerable<IModule> = Seq.empty

    [<ImportMany>]
    member __.Modules
       with get() : IEnumerable<IModule> = modules
       and  set (f : IEnumerable<IModule>) = modules <- f
end

let initialize () =
    // TODO:

    let loader = new ModuleLoader ()
    let dict =
        loader.Modules
        |> List.ofSeq
        |> List.fold (fun lst m ->
                List.concat [|m.GetDatabaseNames (); lst|]
            ) []
        |> List.fold (fun (dict: Dictionary<string, Database>) name ->
                let db = new Database (name)
                dict.Add(name, db)
                dict
            ) (new Dictionary<string, Database> ())
    loader.Modules
    |> List.ofSeq
    |> List.iter (fun m -> m.Initialize dict)
    ()
