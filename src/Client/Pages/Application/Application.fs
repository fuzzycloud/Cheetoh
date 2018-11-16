namespace Application
open Global

(**
    Application module. Can be accessed by Authenticated user only
*)

module Types =

    type Msg =
        | HomeMsg of Home.Types.Msg

    type PageModel =
        | HomeModel of Home.Types.Model


    type Model = {
        CurrentPage : ApplicationPage
        PageModel : PageModel
    }

module State =
    open Elmish
    open Types
    open Fable.Core.JsInterop

    let init (page: ApplicationPage, token) : Model * Cmd<Msg> =
        let (res, resCmd) =
            match page with
            | Home ->
                let (a, aCmd) = Home.State.init(token)
                HomeModel a, Cmd.map HomeMsg aCmd


        {
            CurrentPage = page
            PageModel = res
        }, Cmd.batch [
            resCmd
        ]

    let update msg model : Model * Cmd<Msg> =
        match msg, model.PageModel with
        | HomeMsg msg, HomeModel m ->
            let (a, aCmd) = Home.State.update msg m
            {model with PageModel = HomeModel a}, Cmd.map HomeMsg aCmd
        // | _,_ -> failwith "Application wrong msg and model"

module View =
    open Fable.Helpers.React
    open Fable.Helpers.React.Props
    open Types
    open Fulma




    let root model dispatch =

        let pageHtml page pageModel =
            match page, pageModel with
                | Home, HomeModel m -> Home.View.root m (HomeMsg >> dispatch)
                // | _,_ -> failwith "Application wrong page model combination"

        div [] [

            pageHtml model.CurrentPage model.PageModel
        ]
