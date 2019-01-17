module Server

open Giraffe
open Microsoft.AspNetCore.Http
open User
open Dummy
open Cheetoh.Views

let serverPages : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ GET >=> route "/about" >=> (About.root() |> htmlView)
             GET >=> route "/admin" >=> htmlFile "index.html"
             GET >=> route "/" >=> (Home.root() |> htmlView) ]

let webApp : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ serverPages; cheetohApp; createUserApp ]
