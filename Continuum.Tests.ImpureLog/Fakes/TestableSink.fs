namespace Continuum.Tests.ImpureLog.Fakes

open Continuum.Magic.ImpureLog.Essential

type TestableSink () =
    let mutable entries : ILogEntry list = []

    member __.Entires =
        entries
            |> Seq.map (fun e -> (e.Message, e.Thing))

    member __.Messages =
        entries
            |> Seq.map (fun e -> e.Message)

    member __.Things =
        entries
            |> Seq.choose (fun e -> e.Thing)

    interface ILogSink with
        member __.Put entry =
            entries <- entry :: entries
