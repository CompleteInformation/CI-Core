module CompleteInformation.Core.FSharp.Modules

open Couchbase.Lite
open System.Composition
open System.Composition.Hosting
open System.Collections.Generic
open System.IO
open System.Linq
open System.Reflection
open System.Runtime.Loader

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

let compose () =
    let executableLocation = Assembly.GetEntryAssembly().Location
    let path = Path.Combine(Path.GetDirectoryName(executableLocation), "Plugins")
    let assemblies = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories).Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToList();
    let configuration = new ContainerConfiguration()
    configuration.WithAssemblies(assemblies) |> ignore
    configuration

let initialize () =
    let loader = new ModuleLoader ()

    let configuration = compose()
    use container = configuration.CreateContainer ()
    loader.Modules <- container.GetExports<IModule>()

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
