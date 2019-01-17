namespace Cheetoh.Views

open Giraffe.GiraffeViewEngine
open Layout

module About =
    let root() = [ h1 [] [ str "About Page" ] ] |> layout
