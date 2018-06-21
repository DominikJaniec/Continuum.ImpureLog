namespace Continuum.Tests.ImpureLog

open System
open Continuum.Magic.ImpureLog
open FsUnit
open Xunit
open FsUnit

module LevelTests =
    let everyStandardLevel =
        Seq.map Array.singleton
            [ LvlPanic
            ; LvlError
            ; LvlWarn
            ; LvlInfo
            ; LvlDebug
            ; LvlTrace
            ]

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``'atLeast' matches 'LvlAlways' when any standard`` currentLvl =
        LvlAlways
            |> Level.atLeast currentLvl
            |> should be True

    [<Fact>]
    let ``'atLeast' matches 'LvlAlways' when 'LvlNever'`` () =
        LvlAlways
            |> Level.atLeast LvlNever
            |> should be True

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``'atLeast' matchs any standard when 'LvlNever'`` givenLvl =
        givenLvl
            |> Level.atLeast LvlNever
            |> should be True

    [<Fact>]
    let ``'atLeast' doesn't match 'LvlNever' when 'LvlNever'`` () =
        LvlNever
            |> Level.atLeast LvlNever
            |> should be False
