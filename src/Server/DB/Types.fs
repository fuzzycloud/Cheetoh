namespace DB
open System

[<RequireQualifiedAccess>]
module DBNames =
    let Users = "users"
    let Verifications = "verifications"

(**
    Module to hold types. How they look like in context of litedb
*)
module Types =

    type Store = Memory | LocalDB

    (**
        UserType goes like Admin > Local
    *)
    type UserType =
        Admin  | Local // as there are many user word in code, local is used here for local user
        with
            static member FromString (s : string) =
                match s with
                | "Admin" -> Admin
                | "Local" -> Local
                | _ -> Local


    (**
        User holding all user related property
    *)
    [<CLIMutable>]
    type User = {
        Id : int //key
        UserType : UserType
        Name : string
        PasswordHash : string
        PasswordSalt : string
        Email : string
        EmailVerified : bool
        IsApproved : bool
        IsDeleted : bool
        OrganizationName : string
    }



    (**
        Verification id that is sent via mail to verify email id of user
    *)
    [<CLIMutable>]
    type Verification = {
        Id : int //key
        ConfId : string
    }
