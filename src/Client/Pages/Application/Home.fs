namespace Home
open System
open Shared
open CommonHelpers



module Types =

    type Model = {
        Token : AuthToken
    }

    type Msg = | HomeLoaded

module State =
    open Elmish
    open Types
    open Fable.Core.JsInterop



    let init (token) : Model * Cmd<Msg> =
      {
        Token = token
      }, Cmd.none

    let update msg model : Model * Cmd<Msg> =
        match msg with
        | HomeLoaded -> model, Cmd.none

module View =
    open Fable.Core.JsInterop
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Types
    open Fulma

    let root model dispatch =
        div [] [
            str "Home screen"
        ]
