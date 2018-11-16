module Global

type ApplicationPage =
    | Home

type AuthenticationPage =
    | Login
    | SignUp
    | EMail of string

type Page =
    | ApplicationPage of ApplicationPage
    | AuthenticationPage of AuthenticationPage


let toHash page =
    match page with
    | ApplicationPage p ->
        match p with
        | Home -> "#home"

    | AuthenticationPage p ->
        match p with
        | Login -> "#login"
        | SignUp -> "#signup"
        | EMail s -> "#email/" + s
