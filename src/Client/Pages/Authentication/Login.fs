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
        UserEmailErr : Validate
        PasswordErr : Validate
        IsValid : bool
    }


    let newModel = {
        UserEmail = ""
        Password = ""
        UserEmailErr = Validate.InitialValidate
        PasswordErr = Validate.InitialValidate
        IsValid = false
    }


    type Msg =
    | Login
    | LoggedIn of AuthToken
    | ChangeUrl
    | LoggedInFailed of string
    | CheckUsername of string
    | CheckPassword of string
    | ValidateData



module State =
    open Elmish
    open Types
    open Server
    open CommonStringValidation



    let init () : Model * Cmd<Msg> =
        newModel, Cmd.none

    let update msg model : Model * Cmd<Msg> =
        match msg with
        | ChangeUrl _
        | LoggedIn _ ->
            model, Cmd.none
        | Login ->
            if model.IsValid then
                let loggedInDetail = {
                    Email = model.UserEmail
                    Password = model.Password
                }

                let respHandeler loginResult =
                    match loginResult with
                    | LoginResult.Success t -> LoggedIn (AuthToken t)
                    | UsernameDoesNotExist -> LoggedInFailed "Username does not exist"
                    | PasswordIncorrect -> LoggedInFailed "Password incorrect"
                    | LoginError  s -> LoggedInFailed s

                model, Cmd.ofAsync userApi.authenticate loggedInDetail respHandeler (fun _ -> LoggedInFailed "Unknown Error while trying authenticated")
            else model, Cmd.none
        | LoggedInFailed msg ->
            Toast.error msg
            model,Cmd.none
        | CheckUsername s ->
            let r = String4 s "Invalid Username"
            match r with
            | Ok s -> {model with UserEmail = s; UserEmailErr = Validate.Success}, Cmd.ofMsg ValidateData
            | Error (a,b) -> {model with UserEmail = a; UserEmailErr = Validate.Failure b}, Cmd.ofMsg ValidateData
        | CheckPassword s ->
            let r = String4 s "Invalid Password"
            match r with
            | Ok s -> {model with Password = s; PasswordErr = Validate.Success}, Cmd.ofMsg ValidateData
            | Error (a,b) -> {model with Password = a; PasswordErr = Validate.Failure b}, Cmd.ofMsg ValidateData
        |  ValidateData ->
            {model with IsValid = model.UserEmailErr.IsValid && model.PasswordErr.IsValid },Cmd.none

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
        Hero.hero [ Hero.Color IsSuccess;Hero.IsFullHeight ] [
                Hero.body [] [
                    Container.container [Container.IsFluid;Container.CustomClass TextAlignment.Classes.HasTextCentered] [
                            Column.column [Column.Width (Screen.All,Column.Is4);Column.Offset (Screen.All, Column.Is4)] [
                                    Heading.h3 [Heading.CustomClass "has-text-grey"] [str "Login"]
                                    Heading.p [ Heading.CustomClass "has-text-grey"
                                                Heading.IsSubtitle] [str "Please login to proceed."]
                                    Box.box' [] [
                                        figure [ClassName "avatar"] [
                                            img [Src "https://via.placeholder.com/128x128"]
                                        ]
                                        form [] [
                                            Field.div [] [
                                                Control.div [] [
                                                    Input.email [
                                                        Input.Size IsLarge
                                                        Input.Placeholder "Your Email"
                                                        Input.Value model.UserEmail
                                                        Input.Props [
                                                            AutoFocus true
                                                            OnChange (fun ev -> !!ev.target?value |> CheckUsername |> dispatch)
                                                            onEnter Login dispatch;
                                                        ]
                                                    ]
                                                ]
                                                Help.help [
                                                    Help.Color IsDanger
                                                    Help.CustomClass (if model.UserEmailErr.IsValid then Constants.IsHidden else "")
                                                ] [
                                                    str model.UserEmailErr.ErrMsg
                                                ]
                                            ]
                                            Field.div [] [
                                                Control.div [] [
                                                    Input.password [
                                                        Input.Size IsLarge
                                                        Input.Value model.Password
                                                        Input.Placeholder "Your Password"
                                                        Input.Props [
                                                            OnChange (fun ev -> !!ev.target?value |> CheckPassword |> dispatch)
                                                            onEnter Login dispatch;
                                                        ]
                                                    ]
                                                ]
                                                Help.help [
                                                    Help.Color IsDanger
                                                    Help.CustomClass (if model.PasswordErr.IsValid then Constants.IsHidden else "")
                                                ] [
                                                    str model.PasswordErr.ErrMsg
                                                ]
                                            ]

                                            Button.a [
                                                Button.Disabled (not model.IsValid)
                                                Button.Size IsLarge
                                                Button.Color IsInfo
                                                Button.CustomClass "is-Block"
                                                Button.OnClick (fun _ -> Login |> dispatch )]
                                                [str "Login"]
                                        ]
                                    ]
                                    p [ClassName "has-text-grey"] [
                                        a [Href (toHash (SignUp |> AuthenticationPage))] [str "Sign Up"]
                                    ]
                                ]
                            ]

                        ]
                    ]
