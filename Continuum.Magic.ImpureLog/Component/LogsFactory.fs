namespace Continuum.Magic.ImpureLog.Component

open System.IO
open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Component.Internal
open Continuum.Magic.ImpureLog.Essential


type private NormalSetup () =
    member __.Level = LvlInfo


type private ConsoleSink () =
    interface ILogSink with
        member __.Put entry =
            do printf "%s" entry.Message

type private TextWriterSink (writer : TextWriter) =
    interface ILogSink with
        member __.Put entry =
            do writer.WriteLine entry.Message


module LogsFactory =
    let private setup = new NormalSetup()

    let normalLevel = setup.Level

    let private makeBasicLog sink =
        new BasicLog(sink, setup.Level) :> ILog

    let makeFuncLog sink =
        new FuncLog(sink, setup.Level) :> IFuncLog

    let makeTextLog writer =
        new TextWriterSink(writer)
            |> makeBasicLog

    let makeConsoleLog () =
        new ConsoleSink()
            |> makeBasicLog
