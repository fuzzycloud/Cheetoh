module App.Types

open Global
open Shared

type Msg =
    | ApplicationMsg of Application.Types.Msg
    | AuthenticationMsg of Authentication.Types.Msg

type PageModel =
    | ApplicationModel of Application.Types.Model
    | AuthenticationModel of Authentication.Types.Model

type Model = {
    UserToken : AuthToken option
    CurrentPage : Page
    PageModel : PageModel
}
