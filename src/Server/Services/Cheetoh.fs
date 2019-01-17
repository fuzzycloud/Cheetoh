module Dummy

open Shared
open LiteDB
open DB
open Microsoft.AspNetCore.Http
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Helper.Common
open System.Threading.Tasks
open FSharp.Control.Tasks.V2


let getInitCounter() : Task<Counter> = task { return 42 }

let cheetohApi = { initialCounter = getInitCounter >> Async.AwaitTask }

let cheetohApp : HttpFunc -> HttpContext -> HttpFuncResult =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue cheetohApi
    |> Remoting.buildHttpHandler
