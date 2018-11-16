module App.State

open Elmish
open Elmish.Browser.Navigation
open Browser.UrlParser
open Global
open Types
open Fable.Import
open Browser
open CommonHelpers


let curry2 f x y = f (x,y) // convert tupled form function of two arguments into curried form
let curry3 f x y z = f(x,y,z)


let pageParser: Parser<Page->Page,_> =
  oneOf [
    map (Login |> AuthenticationPage) (s "login")
    map (SignUp |> AuthenticationPage) (s "signup")
    map (EMail >> AuthenticationPage) (s "email" </> str)
    map (Home |> ApplicationPage) (s "home")
  ]

let urlUpdate (result: Option<Page>) model =
    let resM, resCmd =
        match result with
        | None ->
            console.info(sprintf "Error parsing url for %A" model.CurrentPage)
            model,Navigation.modifyUrl (toHash model.CurrentPage)
        | Some page ->
            match page with
            | AuthenticationPage a ->
                let (authentication, authenticationCmd) = Authentication.State.init(a)
                in
                {model with CurrentPage = AuthenticationPage a; PageModel = AuthenticationModel authentication}, Cmd.map AuthenticationMsg authenticationCmd

            | ApplicationPage a ->
                if model.UserToken.IsNone then model, Navigation.modifyUrl(toHash (AuthenticationPage Login))
                else
                let (application, applicationCmd) = Application.State.init(a,model.UserToken.Value)
                in
                {model with CurrentPage = ApplicationPage a; PageModel = ApplicationModel application}, Cmd.map ApplicationMsg applicationCmd

    resM, resCmd

let init result =
    let (authentication, authenticationCmd) = Authentication.State.init(Login)
    let (model, cmd) =
        urlUpdate result {
            UserToken = None
            CurrentPage = AuthenticationPage Login
            PageModel = AuthenticationModel authentication
        }
    model, Cmd.batch[
        Cmd.map AuthenticationMsg authenticationCmd
        cmd
    ]

let update (msg:Msg) (model:Model) =
    match msg, model.PageModel with
        | AuthenticationMsg msg, AuthenticationModel m ->
            let (authentication, authenticationCmd) = Authentication.State.update msg m
            let (res,resCmd) = {model with PageModel = AuthenticationModel authentication}, Cmd.map AuthenticationMsg authenticationCmd
            (res,resCmd)

        | ApplicationMsg msg, ApplicationModel m ->
            let (application, applicationCmd) = Application.State.update msg m
            let (res, resCmd) = {model with PageModel = ApplicationModel application}, Cmd.map ApplicationMsg applicationCmd
            (res, resCmd)
        | _,_ -> failwith "State Wrong Msg & Model Combination"
