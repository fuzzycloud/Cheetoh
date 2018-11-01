open System
open System.IO

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe

open Server



let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us


let configureApp (app : IApplicationBuilder) =
    let serviceProvider = app.ApplicationServices
    let hostingEnv = serviceProvider.GetService<IHostingEnvironment>()

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
