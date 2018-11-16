namespace DB

open LiteDB
open LiteDB.FSharp
open Types
open System.IO

module Helper =

    let [<Literal>] DatabaseName = "demoapp.db"
    let [<Literal>] DataFolderName = "AppData"
    type DBMode = | Exclusive | ReadOnly | Shared


    let (</>) x y = Path.Combine(x, y)

    (**
        Find datafolder and create if folder does not exist
        Change this logic if you required to put database file at specific location
    *)
    let dataFolder =
        let appDataFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        let folder = appDataFolder </> DataFolderName
        let directoryInfo = DirectoryInfo(folder)
        if not directoryInfo.Exists then
            Directory.CreateDirectory folder |> ignore
        printfn "Using data folder: %s" folder
        folder

    let databaseFilePath = dataFolder </> DatabaseName

    (**
        Create database store either in memory or file based.
        Dev is running in memory database while production will point to file based
    *)
    let createDatabaseUsing store =
        let mapper = FSharpBsonMapper()
        match store with
        | Memory ->
            let memoryStream = new System.IO.MemoryStream()
            new LiteDatabase(memoryStream, mapper)
        | LocalDB ->
            let connString = sprintf "Filename=%s;Mode=%s" databaseFilePath (Exclusive.ToString())
            new LiteDatabase(connString, mapper)
