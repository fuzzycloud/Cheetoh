open System
open System.IO

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe

open Server



let publicPath = Path.GetFullPath "../Client/public"
let mutable port = 8085us


let configureApp (app : IApplicationBuilder) =
    let serviceProvider = app.ApplicationServices
    let hostingEnv = serviceProvider.GetService<IHostingEnvironment>()
    printfn "hosting environment ---- %A" hostingEnv.EnvironmentName
    port <- if hostingEnv.IsProduction()
            then
                printfn "port is set to 80"
                80us
            else
                printfn "port is set to 8085"
                8085us
    app.UseDefaultFiles()
       .UseStaticFiles()
       .UseGiraffe (webApp hostingEnv)

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

WebHost
    .CreateDefaultBuilder()
    .UseWebRoot(publicPath)
    .UseContentRoot(publicPath)
    .Configure(Action<IApplicationBuilder> configureApp)
    .ConfigureServices(configureServices)
    .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
    .Build()
    .Run()
