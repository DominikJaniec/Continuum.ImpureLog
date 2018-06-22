namespace Continuum.Magic.ImpureLog

type ILogSink =
    interface end

module CoreLog =

    let into (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) =
        ()

    let throughInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        it

    let thatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        ()

    let throughThatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        it
