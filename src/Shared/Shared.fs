namespace Shared

type Counter = int

type AuthToken = AuthToken of string

type LoginInfo = {
    Email: string
    Password: string
}

type LoginResult =
    | Success of token: string
    | UsernameDoesNotExist
    | PasswordIncorrect
    | LoginError of error: string




module Route =
    /// Defines how routes are generated on server and mapped from client
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

/// A type that specifies the communication protocol between client and server
/// to learn more, read the docs at https://zaid-ajaj.github.io/Fable.Remoting/src/basics.html
type ICheetohApi =
    { initialCounter : unit -> Async<Counter> }

type IUserApi = {
    authenticate : LoginInfo -> Async<LoginResult>
}
