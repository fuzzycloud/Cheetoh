module Server

open Giraffe
open Microsoft.AspNetCore.Http
open User
open Dummy
open Giraffe.Razor

let serverPages : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ GET >=> route "/about" >=> (razorHtmlView "About" None None)
             GET >=> route "/admin" >=> htmlFile "index.html"
             GET >=> route "/" >=> (razorHtmlView "Home" None None) ]

let webApp : HttpFunc -> HttpContext -> HttpFuncResult =
    choose [ serverPages; cheetohApp; createUserApp ]
