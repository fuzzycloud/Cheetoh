module Server

    open Shared
    open Fable.Remoting.Client

    /// A proxy you can use to talk to server directly
    let demoApi : ICheetohApi =
      Remoting.createApi()
      |> Remoting.withRouteBuilder Route.builder
      |> Remoting.buildProxy<ICheetohApi>()
