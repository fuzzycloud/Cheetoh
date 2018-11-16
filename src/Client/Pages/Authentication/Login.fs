namespace Login
open Global
open CommonHelpers.ValidationTypes
open CommonHelpers
open Shared


(**
    Typical most Login page to authenticate user based on credentials
*)

module Types =

    type Model = {
        UserEmail : string
        Password : string
    }


    let newModel = {
        UserEmail = ""
        Password = ""
    }


    type Msg =
    | Login



module State =
    open Elmish
    open Types



    let init () : Model * Cmd<Msg> =
        newModel, Cmd.none

    let update msg model : Model * Cmd<Msg> =
        match msg with
        | Login -> model, Cmd.none
module View =
    open Types
    open Fable.Helpers.React
    open Props
    open Fable.Core.JsInterop
    open Fulma
    open Fable.Import


    let [<Literal>] ENTER_KEY = 13.
    let internal onEnter msg dispatch =
        function
        | (ev:React.KeyboardEvent) when ev.keyCode = ENTER_KEY ->
            dispatch msg
        | _ -> ()
        |> OnKeyDown


    let root (model : Model) dispatch =
        div [] [
            str "Login"
        ]
