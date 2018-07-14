namespace Continuum.Magic.ImpureLog.Essential

open Continuum.Magic.ImpureLog

module CoreLog =

    type private LogEntry =
        { message: string
        ; thing: string option
        }
        interface ILogEntry with
            member this.Message = this.message
            member this.Thing = this.thing

    let sculp it =
        sprintf "%A" it

    let into (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) =
        if lvl |> Level.atLeast setup then
            sink.Put { message = msg; thing = None }
        ()

    let throughInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        if lvl |> Level.atLeast setup then
            sink.Put { message = msg; thing = None }
        it

    let thatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        if lvl |> Level.atLeast setup then
            sink.Put { message = msg; thing = sculp it |> Some }
        ()

    let throughThatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        if lvl |> Level.atLeast setup then
            sink.Put { message = msg; thing = sculp it |> Some }
        it
