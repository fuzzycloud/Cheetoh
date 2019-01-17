namespace Helper

open DB.Helper
open DB.Types
open System.Threading.Tasks
open Microsoft.AspNetCore.Hosting

module Common =

    let liftAsync x = async { return x }
    let liftTask (x : Task<'a>) = x |> Async.AwaitTask


    let getConfAndDatabase (env : IHostingEnvironment) =
        let conf =
            if (env.IsDevelopment()) then Config.Dev
            else Config.Prod

        let database =
            if (env.IsDevelopment()) then createDatabaseUsing Store.Memory
            else createDatabaseUsing Store.LocalDB

        (conf, database)
