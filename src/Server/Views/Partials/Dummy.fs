namespace Cheetoh.Views
open Giraffe.GiraffeViewEngine

module Dummy =
    let partial() = p [] [ str "Some partial text." ]
