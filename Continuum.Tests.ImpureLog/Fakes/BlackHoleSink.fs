namespace Continuum.Tests.ImpureLog.Fakes

open Continuum.Magic.ImpureLog.Essential

type BlackHoleSink () =
    interface ILogSink with
        member __.Put _ = ()
