module Server

open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore

open FSharp.Control.Tasks.V2
open Giraffe
open Shared

open Fable.Remoting.Server
open Fable.Remoting.Giraffe


let getInitCounter () : Task<Counter> = task { return 42 }

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
     webApi env
  ]
