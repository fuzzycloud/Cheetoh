namespace Cheetoh.Views

open Giraffe.GiraffeViewEngine
open Layout

module Home =
    let root() =
        [ div [ _class "container" ]
              [ h3 [ _title "Some title attribute" ]
                    [ sprintf "Hello, %s" "Kunjan" |> str ]

                a [ _href "https://github.com/giraffe-fsharp/Giraffe" ]
                    [ str "Github" ] ]
          div [] [ Dummy.partial() ] ]
        |> layout
