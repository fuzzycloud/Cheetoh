module App.View

open Elmish
open Elmish.Browser.Navigation
open Browser.UrlParser
open Fable.Core
open JsInterop
open Types
open App.State
open Global
open Fulma

importAll "../../node_modules/flatpickr/dist/themes/light.css"
importAll "./../../node_modules/izitoast/dist/css/iziToast.css"
// importAll "./sass/main.sass"

let root model dispatch =

    let pageHtml page pageModel =
        match page, pageModel with
        | AuthenticationPage _ , AuthenticationModel m -> Authentication.View.root m (AuthenticationMsg >> dispatch)
        | ApplicationPage _, ApplicationModel m -> Application.View.root m (ApplicationMsg >> dispatch)
        | _,_ -> failwithf "Wrong View Model Combination %A %A" page pageModel

    pageHtml model.CurrentPage model.PageModel

open Elmish.React
open Elmish.Debug
open Elmish.HMR

// App
Program.mkProgram init update root
|> Program.toNavigable (parseHash pageParser) urlUpdate
#if DEBUG
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
|> Program.withConsoleTrace
#endif
//+:cnd
|> Program.run
