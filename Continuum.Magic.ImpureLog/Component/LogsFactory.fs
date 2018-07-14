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


type private ConsoleLog (setup : NormalSetup) =
    inherit BasicLog(new ConsoleSink(), setup.Level)

type private TextLog (writer : TextWriter, setup : NormalSetup) =
    inherit BasicLog(new TextWriterSink(writer), setup.Level)


module LogsFactory =
    let private setup = new NormalSetup()
    let normalLevel = setup.Level

    let makeTextLog writer =
        new TextLog(writer, setup) :> ILog

    let makeConsoleLog () =
        new ConsoleLog(setup) :> ILog
