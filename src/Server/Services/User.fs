module User

open Shared
open LiteDB
open DB
open Microsoft.AspNetCore.Http
open Giraffe
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Helper.Common

(**
    Authenticate user based on username and password
    User has to be approved and its email has to be verified for authentication to happen
*)
let authenticate (db : LiteDatabase) (conf : IConfig) (loginInfo : LoginInfo) =
    //Dumb username match
    match loginInfo.Email, loginInfo.Password with
    | "admin@admin.com", "admin" ->
        Success "Dummy Token. Will be created using JWT"
    | _, "admin" -> UsernameDoesNotExist
    | "admin@admin.com", _ -> PasswordIncorrect
    | _, _ -> LoginError "Email and Password both invalid"

let createUserApi (ctx : HttpContext) =
    let env = ctx.GetHostingEnvironment()
    let (conf, db) = getConfAndDatabase env
    let userApi = { authenticate = authenticate db conf >> liftAsync }
    userApi

let createUserApp : HttpFunc -> HttpContext -> HttpFuncResult =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromContext createUserApi
    |> Remoting.buildHttpHandler
