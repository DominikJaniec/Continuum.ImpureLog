namespace Continuum.Tests.ImpureLog

open Continuum.Magic.ImpureLog
open FsUnit
open Xunit

module LibraryTests =

    [<Fact>]
    let ``My test`` () =
        Assert.True(true)

    [<Fact>]
    let ``Fail every time`` () =
        Assert.True(false)

    [<Fact>]
    let ``From FsUnit`` () =
        "ships" |> should not' (haveSubstring "pip")

    [<Fact>]
    let ``Fail by FsUnit`` () =
        2 |> should not' (equal 2)

    [<Fact>]
    let ``Uses Continuum.Magic.ImpureLog library`` () =
        Say.hello "magic"
            |> should endWith "magic!"
