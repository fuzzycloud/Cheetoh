module rec Fable.Import.izitoast
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.JS
open Fable.Import.Browser

type [<StringEnum>] [<RequireQualifiedAccess>] IziToastPosition =
    | [<CompiledName "bottomRight">] BottomRight
    | [<CompiledName "bottomLeft">] BottomLeft
    | [<CompiledName "topRight">] TopRight
    | [<CompiledName "topLeft">] TopLeft
    | [<CompiledName "topCenter">] TopCenter
    | [<CompiledName "bottomCenter">] BottomCenter
    | [<CompiledName "center">] Center

type [<StringEnum>] [<RequireQualifiedAccess>] IziToastTransitionIn =
    | [<CompiledName "bounceInLeft">] BounceInLeft
    | [<CompiledName "bounceInRight">] BounceInRight
    | [<CompiledName "bounceInUp">] BounceInUp
    | [<CompiledName "bounceInDown">] BounceInDown
    | [<CompiledName "fadeIn">] FadeIn
    | [<CompiledName "fadeInDown">] FadeInDown
    | [<CompiledName "fadeInUp">] FadeInUp
    | [<CompiledName "fadeInLeft">] FadeInLeft
    | [<CompiledName "fadeInRight">] FadeInRight
    | [<CompiledName "flipInX">] FlipInX

type [<StringEnum>] [<RequireQualifiedAccess>] IziToastTransitionOut =
    | [<CompiledName "fadeOut">] FadeOut
    | [<CompiledName "fadeOutUp">] FadeOutUp
    | [<CompiledName "fadeOutDown">] FadeOutDown
    | [<CompiledName "fadeOutLeft">] FadeOutLeft
    | [<CompiledName "fadeOutRight">] FadeOutRight
    | [<CompiledName "flipOutX">] FlipOutX

type [<AllowNullLiteral>] IziToastSettings =
    abstract id: string option with get, set
    abstract ``class``: string option with get, set
    abstract title: string option with get, set
    abstract titleColor: string option with get, set
    abstract titleSize: string option with get, set
    abstract titleLineHeight: string option with get, set
    abstract message: string with get, set
    abstract messageColor: string option with get, set
    abstract messageSize: string option with get, set
    abstract messageLineHeight: string option with get, set
    abstract backgroundColor: string option with get, set
    abstract color: string option with get, set
    abstract icon: string option with get, set
    abstract iconText: string option with get, set
    abstract iconColor: string option with get, set
    abstract image: string option with get, set
    abstract imageWidth: float option option with get, set
    abstract maxWidth: float option option with get, set
    abstract zindex: float option option with get, set
    abstract layout: float option with get, set
    abstract balloon: bool option with get, set
    abstract close: bool option with get, set
    abstract rtl: bool option with get, set
    abstract position: IziToastPosition option with get, set
    abstract target: string option with get, set
    abstract targetFirst: bool option with get, set
    abstract toastOnce: bool option with get, set
    abstract timeout: U2<bool, float> option with get, set
    abstract drag: bool option with get, set
    abstract pauseOnHover: bool option with get, set
    abstract resetOnHover: bool option with get, set
    abstract progressBar: bool option with get, set
    abstract progressBarColor: string option with get, set
    abstract animateInside: bool option with get, set
    abstract buttons: ResizeArray<obj> option with get, set
    abstract transitionIn: IziToastTransitionIn option with get, set
    abstract transitionOut: IziToastTransitionOut option with get, set
    abstract transitionInMobile: IziToastTransitionIn option with get, set
    abstract transitionOutMobile: IziToastTransitionOut option with get, set
    abstract onOpening: (IziToastSettings -> HTMLDivElement -> unit) option with get, set
    abstract onOpened: (IziToastSettings -> HTMLDivElement -> unit) option with get, set
    abstract onClosing: (IziToastSettings -> HTMLDivElement -> string -> unit) option with get, set
    abstract onClosed: (IziToastSettings -> HTMLDivElement -> string -> unit) option with get, set

type [<AllowNullLiteral>] IziToast =
    abstract show: settings: IziToastSettings -> unit
    abstract hide: settings: IziToastSettings * toast: HTMLDivElement * closedBy: string -> unit
    abstract info: settings: IziToastSettings -> unit
    abstract error: settings: IziToastSettings -> unit
    abstract warning: settings: IziToastSettings -> unit
    abstract success: settings: IziToastSettings -> unit
    abstract destroy: unit -> unit
    abstract settings: settings: IziToastSettings -> unit

let [<Import("*","izitoast")>] iziToast: IziToast = jsNative
