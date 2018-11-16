namespace Authentication
open Global

(**
    Authentication module. App pages which can be accessed without authentication comes here
*)

module Types =

    type Msg =
        | LoginMsg of Login.Types.Msg
        | SignUpMsg of SignUp.Types.Msg
        | EMailMsg of EMail.Types.Msg

    type PageModel =
        | LoginModel of Login.Types.Model
        | SignUpModel of SignUp.Types.Model
        | EMailModel of EMail.Types.Model


    type Model = {
        CurrentPage : AuthenticationPage
        PageModel : PageModel
    }

module State =
    open Elmish
    open Types

    let init (page : AuthenticationPage) : Model * Cmd<Msg> =
        let (res, resCmd) =
            match page with
            | Login ->
                let (a, aCmd) = Login.State.init()
                LoginModel a, Cmd.map LoginMsg aCmd
            | SignUp ->
                let (a,aCmd) = SignUp.State.init()
                SignUpModel a, Cmd.map SignUpMsg aCmd
            | EMail c ->
                let (a,aCmd) = EMail.State.init(c)
                EMailModel a, Cmd.map EMailMsg aCmd
        {
            CurrentPage = page
            PageModel = res
        }, resCmd

    let update msg model : Model * Cmd<Msg> =
      match msg, model.PageModel with
        | LoginMsg msg, LoginModel m ->
            let (a, aCmd) = Login.State.update msg m
            {model with PageModel = LoginModel a}, Cmd.map LoginMsg aCmd
        | SignUpMsg msg, SignUpModel m ->
            let (a, aCmd) = SignUp.State.update msg m
            {model with PageModel = SignUpModel a}, Cmd.map SignUpMsg aCmd
        | EMailMsg msg, EMailModel m ->
            let (a, aCmd) = EMail.State.update msg m
            {model with PageModel = EMailModel a}, Cmd.map EMailMsg aCmd
        | _,_ -> failwith "Authentication wrong msg and model"

module View =
    open Types

    let root model dispatch =
        match model.CurrentPage, model.PageModel with
            | Login, LoginModel m -> Login.View.root m (LoginMsg >> dispatch)
            | SignUp, SignUpModel m -> SignUp.View.root m (SignUpMsg >> dispatch)
            | EMail _, EMailModel m -> EMail.View.root m (EMailMsg >> dispatch)
            | _,_ -> failwith "authentication wrong page model combination"
