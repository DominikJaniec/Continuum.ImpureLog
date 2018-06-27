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

    let private stringify it =
        sprintf "%A" it
            |> Some

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
            sink.Put { message = msg; thing = stringify it}
        ()

    let throughThatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        if lvl |> Level.atLeast setup then
            sink.Put { message = msg; thing = stringify it}
        it
