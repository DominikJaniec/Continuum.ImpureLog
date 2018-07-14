namespace Continuum.Magic.ImpureLog.Component.Internal

open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Essential


type BasicLog (sink : ILogSink, setup : Level) =
    let log = CoreLog.into sink setup

    interface ILog with
        member __.UsableFor lvl =
            lvl |> Level.atLeast setup

        member __.Message lvl msg = log lvl msg

        member __.Always msg = log LvlAlways msg
        member __.Panic msg = log LvlPanic msg
        member __.Error msg = log LvlError msg
        member __.Warn msg = log LvlWarn msg
        member __.Info msg = log LvlInfo msg
        member __.Debug msg = log LvlDebug msg
        member __.Trace msg = log LvlTrace msg
