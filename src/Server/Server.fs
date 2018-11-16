module Server

open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore

open FSharp.Control.Tasks.V2
open Giraffe
open Shared

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Microsoft.AspNetCore.Hosting
open DB.Helper
open DB.Types
open User


let liftAsync x = async {return x}
let liftTask (x:Task<'a>) = x |> Async.AwaitTask

let getInitCounter () : Task<Counter> = task { return 42 }

let getConfAndDatabase (env : IHostingEnvironment) =
   let conf = if (env.IsDevelopment()) then Config.Dev else Config.Prod
   let database = if (env.IsDevelopment()) then createDatabaseUsing Store.Memory else  createDatabaseUsing Store.LocalDB
   (conf,database)


let createUserApi (env : IHostingEnvironment) : HttpFunc -> Http.HttpContext -> HttpFuncResult =
   let (conf, db) = getConfAndDatabase env
   let userApi = {
      authenticate = authenticate db conf >> liftAsync
   }
   Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue userApi
    |> Remoting.buildHttpHandler


let cheetohApi = {
    initialCounter = getInitCounter >> Async.AwaitTask
}

let webApi env: HttpFunc -> Http.HttpContext -> HttpFuncResult =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue cheetohApi
    |> Remoting.buildHttpHandler

let webApp env =
    choose [
      GET >=> route "/about" >=> htmlFile "about.html"
      GET >=> route "/admin" >=> htmlFile "admin.html"
   //   webApi env
      createUserApi env
  ]
