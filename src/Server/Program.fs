open System
open System.IO

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe
open Giraffe.Razor

open Server
open Microsoft.Extensions.Hosting



let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us


let configureApp (app : IApplicationBuilder) =
    app
       .UseStaticFiles()
       .UseGiraffe (webApp)

let configureServices (services : IServiceCollection) =
       let sp  = services.BuildServiceProvider()
       let env = sp.GetService<IHostingEnvironment>()
       Path.Combine(env.ContentRootPath, "Views")
       |> services.AddRazorEngine
       |> ignore
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
