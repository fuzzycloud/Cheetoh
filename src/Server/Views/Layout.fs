namespace Cheetoh.Views

open Giraffe.GiraffeViewEngine

module Layout =
    let navigation =
        nav [] [ a [ _href "/" ] [ str "Home" ]
                 a [ _href "/about" ] [ str "About" ] ]

    let layout (content : XmlNode list) =
        html [] [ head [] [ title [] [ str "Giraffe" ] ]
                  body [] [ navigation
                            div [] content ] ]
