namespace Continuum.Magic.ImpureLog.Component.Internal

open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Essential


type FuncLog (sink : ILogSink, setup : Level) =

    interface IFuncLog with

        member __.stateAs lvl msg =
            CoreLog.into sink setup lvl msg

        member __.stateThatAs lvl msg x =
            CoreLog.thatInto sink setup lvl msg x

        member __.justAs lvl msg x =
            CoreLog.throughInto sink setup lvl msg x

        member __.justThatAs lvl msg x =
            CoreLog.throughThatInto sink setup lvl msg x
