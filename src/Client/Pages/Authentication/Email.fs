namespace EMail
open Shared
open CommonHelpers

(**
    Simple most basic email verification page. It will verify email based on hash id passed via url
*)

module Types =
    type Model = {
        ConfId : string
    }

    type Msg = | EmailVerified

module State =
    open Elmish
    open Types


    let init (confId) : Model * Cmd<Msg> =
        {
            ConfId = confId
        }, Cmd.none

    let update msg model : Model * Cmd<Msg> =
        match msg with
        | EmailVerified ->
            model, Cmd.none

module View =
    open Types
    open Fable.Helpers.React
    open Props
    open Fulma

    let root (model : Model) dispatch =
        div [] [
            str "Email verification"
        ]
