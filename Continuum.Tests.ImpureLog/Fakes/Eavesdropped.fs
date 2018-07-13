namespace Continuum.Tests.ImpureLog.Fakes

open System
open System.IO

module Eavesdropped =

    type IEavesdrop =
        abstract member Messages : string seq with get
        abstract member Flush : unit -> unit

    type private Wired (baseWriter: TextWriter) =
        inherit TextWriter(baseWriter.FormatProvider)
        let baseEncoding = baseWriter.Encoding
        let mutable messages : string list = []

        interface IEavesdrop with
            member __.Messages = messages |> Seq.rev
            member __.Flush () = messages <- []

        override __.Encoding = baseEncoding
        override __.Write (value: string) =
            messages <- value :: messages
        override __.Write (_: char) : unit =
            failwith "Single Character Write is not supported."
        override this.Write (buffer : char array) =
            let printable c =
                match Char.IsControl c with
                | true -> sprintf "0x%02X" (int c)
                | false -> string c

            let characters = Seq.map printable buffer
            do this.Write(String.Join("|", characters))


    let private consoleLocker = new Object()

    let acquiredConsole (job: IEavesdrop -> unit) =
        let wired = new Wired (Console.Out)

        lock consoleLocker (fun () ->
            let stdConsole = Console.Out
            try
                Console.SetOut wired
                job wired
            finally
                if Console.Out <> stdConsole then
                    Console.SetOut(stdConsole)
        )
