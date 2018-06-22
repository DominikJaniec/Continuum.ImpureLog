namespace Continuum.Tests.ImpureLog

open System
open Continuum.Magic.ImpureLog
open FsUnit
open Xunit

module CoreLogTests =

    type private BlackHoleSink () =
        interface ILogSink


    let private setupLvl = LvlWarn
    let private higherLvl = LvlError
    let private nullSink = new BlackHoleSink()


    module ``The method 'into'`` =

        [<Fact>]
        let ``returns unit`` () =
            do CoreLog.into nullSink setupLvl higherLvl "very long log message"


    module ``The method 'throughInto'`` =

        [<Fact>]
        let ``passes anything down`` () =
            let anything = "something to pass down pipe"
            anything
                |> CoreLog.throughInto nullSink setupLvl higherLvl "short.msg"
                |> should be (equal anything)


    module ``The method 'thatInto'`` =

        [<Fact>]
        let ``returns unit`` () =
            [ 1; 3; 8; 41352; Int32.MaxValue ]
                |> CoreLog.thatInto nullSink setupLvl higherLvl "complicated msg"


    module ``The method 'thatThroughInto'`` =

        [<Fact>]
        let ``passes anything down`` () =
            let anything = ("Some", DateTimeOffset.Now)
            anything
                |> CoreLog.throughThatInto nullSink setupLvl higherLvl "message"
                |> should be (equal anything)
