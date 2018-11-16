module CommonStringValidation

open System

(**
    Map custome error message to default error message
*)
let rMapError (msg : string) (res:Result<'a,'b * 'c>) =
            res |> Result.mapError (fun (x,_) -> (x,msg) )

(**
    Validate if string is null or not
*)
let validateNull (s:string) =
    if String.IsNullOrEmpty s || String.IsNullOrWhiteSpace s then Error (s, "String can't be null")
    else Ok s

(**
    Validate if string is at least more than 4
*)
let validate4 (s: string) =
    if s.Length < 4 then Error(s, "String can't be less than 4 chars")
    else Ok s

(**
    Very basic email validation. As Email should be having @ and . in it.
*)
let validateEmail (s : string) =
    if s.Contains("@") && s.Contains(".") then Ok s else Error (s, "Invalid Email")

(**
    Compare if two strings are equel
*)
let compareString  (destination : string) (source : string) =
    if String.Equals(source, destination) then Ok source
    else Error (source, "String is not same as destination")

(**
    Validate if string is minimum of length 4
*)
let String4 (s:string) (msg : string) =
    (validateNull >> Result.bind validate4) s |> rMapError msg

(**
    Validate if string is minimum of length 4
*)
let Email (s:string) =
    (validateNull >> Result.bind validate4 >> Result.bind validateEmail) s

(**
    Validate if string is of minimum length 4 and also equal to base string
*)
let ComparedString s d msg =
    (validateNull >> Result.bind validate4 >> Result.bind (compareString d)) s |> rMapError msg
