module User
open Helper.Security
open DB.Types
open Shared
open LiteDB
open LiteDB.FSharp
open LiteDB.FSharp.Extensions
open System.Linq
open System
open DB
open Helper
open System.Linq
open Giraffe.HttpStatusCodeHandlers
open System.Net




(**
    Authenticate user based on username and password
    User has to be approved and its email has to be verified for authentication to happen
*)
let authenticate (db : LiteDatabase) (conf : IConfig) (loginInfo : LoginInfo) =
    //Dumb username match
    match loginInfo.Email, loginInfo.Password with
    | "admin@admin.com", "admin" -> Success "Dummy Token. Will be created using JWT"
    | _ , "admin" -> UsernameDoesNotExist
    | "admin@admin.com", _ -> PasswordIncorrect
    | _, _ -> LoginError "Email and Password both invalid"
