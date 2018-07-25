namespace Continuum.Magic.ImpureLog.Component.Internal

open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Essential


type BasicLog (sink : ILogSink, setup : Level) =
    inherit FuncLog(sink, setup)

    member private this.LogAs lvl msg =
        (this :> IFuncLog).stateAs lvl msg

    interface ILog with
        member this.UsableFor lvl =
            lvl |> Level.atLeast setup

        member this.Message lvl msg = this.LogAs lvl msg

        member this.Always msg = this.LogAs LvlAlways msg
        member this.Panic msg = this.LogAs LvlPanic msg
        member this.Error msg = this.LogAs LvlError msg
        member this.Warn msg = this.LogAs LvlWarn msg
        member this.Info msg = this.LogAs LvlInfo msg
        member this.Debug msg = this.LogAs LvlDebug msg
        member this.Trace msg = this.LogAs LvlTrace msg
