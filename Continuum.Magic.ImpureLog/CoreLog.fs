namespace Continuum.Magic.ImpureLog

type ILogSink =
    abstract member Put : string -> unit

module CoreLog =

    let into (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) =
        if lvl |> Level.atLeast setup then
            sink.Put msg
        ()

    let throughInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        if lvl |> Level.atLeast setup then
            sink.Put msg
        it

    let thatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        into sink setup lvl msg

    let throughThatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        throughInto sink setup lvl msg it
