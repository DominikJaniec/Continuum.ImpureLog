namespace Continuum.Tests.ImpureLog.Tests.Component

open System
open System.IO
open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Component
open Continuum.Tests.ImpureLog.Fakes
open FsUnit
open FsUnitTyped.TopLevelOperators
open Microsoft.FSharp.Reflection
open Xunit

module LogsFactoryTests =

    module ``The LogsFactory's property 'level'`` =

        [<Fact>]
        let ``equals 'LvlInfo'`` () =
            LogsFactory.normalLevel
                |> shouldEqual LvlInfo


    module ``The LogsFactory's method 'makeTextLog' produces 'ILog'`` =

        [<Fact>]
        let ``with 'UsableFor' method`` () =
            let log = LogsFactory.makeTextLog TextWriter.Null
            log.UsableFor LvlAlways
                |> shouldEqual true


        [<Fact>]
        let ``which 'LvlNever' is never 'UsableFor'`` () =
            let log = LogsFactory.makeTextLog TextWriter.Null
            log.UsableFor LvlNever
                |> shouldEqual false


        [<Fact>]
        let ``which writes to given 'TextWriter'`` () =
            let givenWriter = new StringWriter()
            let msg = "expected message written to 'TextWriter'"
            let log = LogsFactory.makeTextLog givenWriter

            do log.Always msg
            givenWriter.GetStringBuilder().ToString()
                |> shouldContainText msg


        [<Fact>]
        let ``which is also the 'IFuncLog' type instance`` () =
            let msg = "This should not happen."
            let anything = ("Should pass", 96, LvlAlways)
            let writer = new StringWriter()
            let log = LogsFactory.makeTextLog writer

            do log.stateAs LvlNever msg
            anything
                |> log.justThatAs LvlNever msg
                |> shouldEqual anything
            writer.ToString()
                |> shouldBeEmpty


    module ``The LogsFactory's method 'makeConsoleLog' produces 'ILog'`` =

        type StandardLevelCase = (Level * string option)

        let standardLevelCasesTyped : StandardLevelCase list =
            [ (LvlAlways, Some "Always")
            ; (LvlPanic, Some "Panic")
            ; (LvlError, Some "Error")
            ; (LvlWarn, Some "Warn")
            ; (LvlInfo, Some "Info")
            ; (LvlDebug, None)
            ; (LvlTrace, None)
            ; (LvlPriority 0, Some "Same as normal 'LvlInfo'")
            ; (LvlPriority 1, Some "Above normal level")
            ; (LvlPriority -1, None)
            ]

        let standardLevelCases =
            standardLevelCasesTyped
                |> List.map FSharpValue.GetTupleFields


        [<Fact>]
        let ``which writes messages to the 'Console'`` () =
            Eavesdropped.acquiredConsole (fun eavesdrop ->
                let msg = "expected message written to the Console"
                let log = LogsFactory.makeConsoleLog ()

                do log.Always msg
                eavesdrop.Messages
                    |> shouldContain msg
            )


        [<Theory>]
        [<MemberData("standardLevelCases")>]
        let ``witch is 'UsableFor' for normal 'Level'`` ((lvl, expected) : StandardLevelCase) =
            let expectedUsable =
                expected
                    |> Option.map (fun _ -> true)
                    |> Option.defaultValue false

            let log = LogsFactory.makeConsoleLog ()
            log.UsableFor lvl
                |> shouldEqual expectedUsable


        [<Theory>]
        [<MemberData("standardLevelCases")>]
        let ``with support for standard 'Level' cases`` ((lvl, expected) : StandardLevelCase) =
            let callWithGivenLevel (log : ILog) =
                match lvl with
                | LvlAlways -> log.Always
                | LvlPanic -> log.Panic
                | LvlError -> log.Error
                | LvlWarn -> log.Warn
                | LvlInfo -> log.Info
                | LvlDebug -> log.Debug
                | LvlTrace -> log.Trace
                | LvlPriority _ ->  log.Message lvl
                | _ -> NotSupportedException (sprintf "Given '%A' is not supported." lvl) |> raise

            let logMessage =
                expected
                    |> Option.defaultValue "ignored"

            Eavesdropped.acquiredConsole (fun eavesdrop ->
                let log = LogsFactory.makeConsoleLog ()
                do logMessage |> callWithGivenLevel log

                match expected with
                | Some message ->
                    eavesdrop.Messages
                        |> shouldContain message
                | None ->
                    eavesdrop.Messages
                        |> shouldBeEmpty
            )


        [<Fact>]
        let ``which is also the 'IFuncLog' type instance`` () =
            let msg = "According to standard configuration: Not Printed"
            let anything = [ "Go"; "through"; "the"; "log"; "unseen" ]

            Eavesdropped.acquiredConsole (fun eavesdrop ->
                let log = LogsFactory.makeConsoleLog ()

                do anything |> log.stateThatAs LvlTrace msg
                anything
                    |> log.justAs LvlDebug msg
                    |> shouldEqual anything
                eavesdrop.Messages
                    |> shouldBeEmpty
            )


    module ``The LogsFactory's method 'makeFuncLog' produces 'IFuncLog'`` =

        let sculpter x = sprintf "%A" x

        [<Fact>]
        let ``with method 'stateAs' which writes only message under given level`` () =
            let sink = new TestableSink()
            let sampleMsg = "This message should go along Level and Timestamp, or not?"
            let log = LogsFactory.makeFuncLog sink

            do log.stateAs LvlAlways sampleMsg
            sink.Messages
                |> shouldContain sampleMsg


        [<Fact>]
        let ``with method 'stateThatAs' which writes message and sculpting anything`` () =
            let sink = new TestableSink()
            let sampleMsg = "That dict-like list should be placed under this message"
            let anything = [ ("fst", 18945793); ("snd", 9573) ]
            let log = LogsFactory.makeFuncLog sink

            anything
                |> log.stateThatAs LvlAlways sampleMsg
            sink.Messages
                |> shouldContain sampleMsg
            sink.Things
                |> shouldContain (sculpter anything)


        [<Fact>]
        let ``with method 'justAs' which writes message and passes anything without sculpting it`` () =
            let sink = new TestableSink()
            let sampleMsg = "should be alone"
            let anything = 987654321
            let log = LogsFactory.makeFuncLog sink

            anything
                |> log.justAs LvlAlways sampleMsg
                |> shouldEqual anything
            sink.Messages
                |> shouldContain sampleMsg
            sink.Things
                |> shouldNotContain (sculpter anything)


        [<Fact>]
        let ``with method 'justThatAs' which writes message and passes anything and sculpting it`` () =
            let sink = new TestableSink()
            let sampleMsg = "Very descriptive message for level Always"
            let anything = [ 1; 2; 3; 4; 7; 13 ]
            let log = LogsFactory.makeFuncLog sink

            anything
                |> log.justThatAs LvlAlways sampleMsg
                |> shouldEqual anything
            sink.Messages
                |> shouldContain sampleMsg
            sink.Things
                |> shouldContain (sculpter anything)
