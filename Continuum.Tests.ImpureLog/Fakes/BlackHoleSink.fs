namespace Continuum.Tests.ImpureLog.Fakes

open Continuum.Magic.ImpureLog

type BlackHoleSink () =
    interface ILogSink with
        member __.Put _ = ()
