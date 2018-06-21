namespace Continuum.Tests.ImpureLog

open System
open Continuum.Magic.ImpureLog
open FsUnit
open Xunit

module CoreLogTests =
    let private blackHole = ""

    [<Fact>]
    let ``Should pass anything down`` () =
        let anything = ("Some", DateTimeOffset.Now)
        CoreLog.thatThroughInto blackHole LvlAlways "message" anything
            |> should be (equal anything)

    [<Fact>]
    let ``Should end with unit`` () =
       let anything = [1; 3; 8]
       CoreLog.thatInto blackHole LvlAlways "complicated msg" anything
