namespace Continuum.Tests.ImpureLog.Tests.Essential

open System
open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Essential
open Continuum.Tests.ImpureLog.Fakes
open FsUnit
open FsUnitTyped.TopLevelOperators
open Xunit

module CoreLogTests =
    let private setupLvl = LvlWarn
    let private higherLvl = LvlError
    let private lowerLvl = LvlInfo
    let private nullSink = new BlackHoleSink()
    let private makeTestSink () = TestableSink()


    module ``The CoreLog's method 'into'`` =

        [<Fact>]
        let ``returns unit`` () =
            do CoreLog.into nullSink setupLvl higherLvl "very long log message"

        [<Fact>]
        let ``puts message when given level is 'atLeast' current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "very unique message"
            do CoreLog.into sink setupLvl higherLvl givenMessage
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``puts message when given level is exactly as current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "expected message in log"
            do CoreLog.into sink setupLvl setupLvl givenMessage
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``doesn't put message when given level isn't 'atLeast' current setup`` () =
            let sink = makeTestSink()
            do CoreLog.into sink setupLvl lowerLvl "missing message"
            sink.Messages
                |> shouldBeEmpty


    module ``The CoreLog's method 'throughInto'`` =

        [<Fact>]
        let ``passes anything down`` () =
            let anything = "something to pass down through the pipe"
            anything
                |> CoreLog.throughInto nullSink setupLvl higherLvl "short.msg"
                |> shouldEqual anything

        [<Fact>]
        let ``puts message when given level is 'atLeast' current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "STOP will be STOP put in the log STOP"
            [| "DATA"; "ARRAY"; "MAGIC"; "OMEGA" |]
                |> CoreLog.throughInto sink setupLvl higherLvl givenMessage
                |> ignore
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``puts message when given level is exactly as current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "very important message to log"
            ("list-of_letters", [ 9; 8; 7 ], [| "magic"; "array" |])
                |> CoreLog.throughInto sink setupLvl setupLvl givenMessage
                |> ignore
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``doesn't put message when given level isn't 'atLeast' current setup`` () =
            let sink = makeTestSink()
            (TimeSpan.FromDays 42.0, "m.o.r.e. DATA")
                |> CoreLog.throughInto sink setupLvl lowerLvl "this_message_should_be_ignored"
                |> ignore
            sink.Messages
                |> shouldBeEmpty

        [<Fact>]
        let ``doesn't put stringified thing with message when logged through`` () =
            let sink = makeTestSink()
            let message = "this message should be logged alone"
            [ (1, "very"); (2, "complex)"); (3, "thing") ]
                |> CoreLog.throughInto sink setupLvl higherLvl message
                |> ignore
            sink.Entires
                |> shouldContain (message, None)


    module ``The CoreLog's method 'thatInto'`` =

        [<Fact>]
        let ``returns unit`` () =
            [ 1; 3; 8; 41352; Int32.MaxValue ]
                |> CoreLog.thatInto nullSink setupLvl higherLvl "complicated msg"

        [<Fact>]
        let ``puts message when given level is 'atLeast' current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "Are you carbon based lifeforms?"
            (True, False, None)
                |> CoreLog.thatInto sink setupLvl higherLvl givenMessage
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``doesn't put message when given level isn't 'atLeast' current setup`` () =
            let sink = makeTestSink()
            [ 0.1; 0.9; 0.7; 13.31 ]
                |> CoreLog.thatInto sink setupLvl lowerLvl "message not expected"
            sink.Messages
                |> shouldBeEmpty

        [<Fact>]
        let ``puts stringified thing with message when logged`` () =
            let sink = makeTestSink()
            let message = "How string could be more stringified?"
            let thing = "When stringify a string you got it back in quotes."
            let stringified = "\"" + thing + "\""
            thing
                |> CoreLog.thatInto sink setupLvl higherLvl message
            sink.Entires
                |> shouldContain (message, Some stringified)


    module ``The CoreLog's method 'thatThroughInto'`` =

        [<Fact>]
        let ``passes anything down`` () =
            let anything = ("Some", DateTimeOffset.Now)
            anything
                |> CoreLog.throughThatInto nullSink setupLvl higherLvl "message"
                |> shouldEqual anything

        [<Fact>]
        let ``puts message when given level is 'atLeast' current setup`` () =
            let sink = makeTestSink()
            let givenMessage = "_/ very important system log message \_"
            (DateTime.UtcNow, DateTimeOffset.Now)
                |> CoreLog.throughThatInto sink setupLvl higherLvl givenMessage
                |> ignore
            sink.Messages
                |> shouldContain givenMessage

        [<Fact>]
        let ``doesn't put message when given level isn't 'atLeast' current setup`` () =
            let sink = makeTestSink()
            "<{{| it is just magic string, do not bother here, it doesn't matter |}}>"
                |> CoreLog.throughThatInto sink setupLvl lowerLvl "not logged message"
                |> ignore
            sink.Messages
                |> shouldBeEmpty

        [<Fact>]
        let ``puts stringified thing with message when logged through`` () =
            let sink = makeTestSink()
            let givenMessage = "this message should be with stringified thing..."
            let thing = [ (LvlNever, "Lowest level"); (LvlInfo, "Default one") ]
            let stringified = sprintf "%A" thing
            thing
                |> CoreLog.throughThatInto sink setupLvl higherLvl givenMessage
                |> ignore
            sink.Entires
                |> shouldContain (givenMessage, Some stringified)
