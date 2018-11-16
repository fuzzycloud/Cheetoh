namespace SignUp
open Global
open CommonHelpers.ValidationTypes
open CommonHelpers
open CommonHelpers.ValidationTypes

open Shared
open System.Collections.Generic

(**
    Sign up page to sign up new users.
*)

module Types =

    type Model = {
        Email : string
    }


    let newModel = {
        Email = ""
    }


    type Msg =
        | Nothing


module State =
    open Elmish
    open Types
    open Fable.Core.JsInterop



    let init () : Model * Cmd<Msg> =
      newModel, Cmd.none

    let update msg model : Model * Cmd<Msg> =
        match msg with
        | Msg.Nothing -> model, Cmd.none

module View =
    open Types
    open Fable.Helpers.React
    open Props
    open Fable.Core.JsInterop
    open Fulma
    open Fable.Import

    let root (model : Model) dispatch =
        div [] [
            str "Sign Up page"
        ]
