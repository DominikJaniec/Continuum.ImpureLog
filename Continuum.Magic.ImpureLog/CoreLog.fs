namespace Continuum.Magic.ImpureLog

type ILogSink =
    interface end

module CoreLog =
    let DefaultLevel = LvlInfo

    let throughThatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        it

    let thatInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        ()

    let throughInto (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) it =
        it

    let into (sink: ILogSink) (setup: Level) (lvl: Level) (msg: string) =
        ()
