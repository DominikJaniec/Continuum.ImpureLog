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

    module ``The LogsFactory' property 'level'`` =

        [<Fact>]
        let ``equals 'LvlInfo'`` () =
            LogsFactory.normalLevel
                |> shouldEqual LvlInfo


    module ``The LogsFactory' method 'makeTextLog'`` =

        [<Fact>]
        let ``produces 'ILog' with 'UsableFor' method`` () =
            let log = LogsFactory.makeTextLog TextWriter.Null
            log.UsableFor LvlAlways
                |> shouldEqual true

        [<Fact>]
        let ``produces 'ILog' which 'LvlNever' is never 'UsableFor'`` () =
            let log = LogsFactory.makeTextLog TextWriter.Null
            log.UsableFor LvlNever
                |> shouldEqual false

        [<Fact>]
        let ``produces 'ILog' which writes to given 'TextWriter'`` () =
            let givenWriter = new StringWriter()
            let msg = "expected message written to 'TextWriter'"
            let log = LogsFactory.makeTextLog givenWriter
            do log.Always msg
            givenWriter.GetStringBuilder().ToString()
                |> shouldContainText msg


    module ``The LogsFactory' method 'makeConsoleLog'`` =

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
        let ``produces 'ILog' which writes messages to the 'Console'`` () =
            Eavesdropped.acquiredConsole (fun eavesdrop ->
                let msg = "expected message written to the Console"
                let log = LogsFactory.makeConsoleLog ()
                do log.Always msg
                eavesdrop.Messages
                    |> shouldContain msg
            )


        [<Theory>]
        [<MemberData("standardLevelCases")>]
        let ``produces 'ILog' witch is 'UsableFor' for normal 'Level'`` ((lvl, expected) : StandardLevelCase) =
            let expectedUsable =
                expected
                    |> Option.map (fun _ -> true)
                    |> Option.defaultValue false

            let log = LogsFactory.makeConsoleLog ()
            log.UsableFor lvl
                |> shouldEqual expectedUsable


        [<Theory>]
        [<MemberData("standardLevelCases")>]
        let ``produces 'ILog' with support for standard 'Level' cases`` ((lvl, expected) : StandardLevelCase) =
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
