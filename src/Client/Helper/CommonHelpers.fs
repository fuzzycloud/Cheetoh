namespace CommonHelpers



    [<RequireQualifiedAccess>]
    module Constants =
        let IsHidden = "is-hidden"

    module ValidationTypes =

        (**
            Validation types to hold validation for input
        *)
        type Validate = {
           IsValid : bool
           ErrMsg : string
        }

        module Validate =
            let Success = {
                IsValid = true
                ErrMsg = ""
            }
            let Failure (msg : string) = {
                IsValid = false
                ErrMsg = msg
            }

            let InitialValidate = {
                IsValid = false
                ErrMsg = ""
            }

    (**
        Wrapper around iziToast library. So, there is no need to pass IziToastSetting everytime
    *)
    [<RequireQualifiedAccess>]
    module Toast =
        open Fable.Import.izitoast
        open Fable.Core.JsInterop

        let error (msg : string)  =
            iziToast.error(jsOptions<IziToastSettings>(fun x -> x.message <- msg))

        let info (msg : string) =
            iziToast.info(jsOptions<IziToastSettings>(fun x -> x.message <- msg))

        let success (msg : string) =
            iziToast.success(jsOptions<IziToastSettings>(fun x -> x.message <- msg))

        let warning (msg : string) =
            iziToast.warning(jsOptions<IziToastSettings>(fun x -> x.message <- msg))
