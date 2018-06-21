namespace Continuum.Tests.ImpureLog

open System
open Continuum.Magic.ImpureLog
open FsUnit
open Xunit


type BlackHoleSink () =
    interface ILogSink

module CoreLogTests =
    let currentLvl = CoreLog.DefaultLevel
    let blackHoleSink = new BlackHoleSink()

    [<Fact>]
    let ``Method 'thatThroughInto' passes anything down`` () =
        let anything = ("Some", DateTimeOffset.Now)
        CoreLog.throughThatInto blackHoleSink currentLvl LvlAlways "message" anything
            |> should be (equal anything)

    [<Fact>]
    let ``Method 'thatInto' returns unit`` () =
       let anything = [ 1; 3; 8; 41352 ]
       CoreLog.thatInto blackHoleSink currentLvl LvlAlways "complicated msg" anything

    [<Fact>]
    let ``Method 'throughInto' passes anything down`` () =
        let anything = "something to pass down pipe"
        CoreLog.throughInto blackHoleSink currentLvl LvlAlways "short-msg" anything
            |> should be (equal anything)

    [<Fact>]
    let ``Method 'into' returns unit`` () =
        CoreLog.into blackHoleSink currentLvl LvlAlways "very long log message"
