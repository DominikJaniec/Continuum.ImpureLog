namespace Continuum.Tests.ImpureLog

open System
open Continuum.Magic.ImpureLog
open FsUnit
open Xunit

module LevelTests =
    let everyStandardLevel =
        [ [| LvlPanic |]
        ; [| LvlError |]
        ; [| LvlWarn |]
        ; [| LvlInfo |]
        ; [| LvlDebug |]
        ; [| LvlTrace |]
        ]

    let adjacentHiLoLevels =
        [ [| LvlPanic; LvlError |]
        ; [| LvlError; LvlWarn |]
        ; [| LvlWarn; LvlInfo |]
        ; [| LvlInfo; LvlDebug |]
        ; [| LvlDebug; LvlTrace |]
        ]

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``Method 'atLeast' matches 'LvlAlways' when any standard level is set`` currentLvl =
        LvlAlways
            |> Level.atLeast currentLvl
            |> should be True

    [<Fact>]
    let ``Method 'atLeast' matches 'LvlAlways' when 'LvlAlways' is set`` () =
        LvlAlways
            |> Level.atLeast LvlAlways
            |> should be True

    [<Fact>]
    let ``Method 'atLeast' matches 'LvlAlways' when 'LvlNever' is set`` () =
        LvlAlways
            |> Level.atLeast LvlNever
            |> should be True

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``Method 'atLeast' doesn't match any standard level when 'LvlAlways' is set`` givenLvl =
        givenLvl
            |> Level.atLeast LvlAlways
            |> should be False

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``Method 'atLeast' matches any standard level when 'LvlNever' is set`` givenLvl =
        givenLvl
            |> Level.atLeast LvlNever
            |> should be True

    [<Theory>]
    [<MemberData("everyStandardLevel")>]
    let ``Method 'atLeast' doesn't match 'LvlNever' when any standard level is set`` currentLvl =
        LvlNever
            |> Level.atLeast currentLvl
            |> should be False

    [<Fact>]
    let ``Method 'atLeast' doesn't match 'LvlNever' when 'LvlNever' is set`` () =
        LvlNever
            |> Level.atLeast LvlNever
            |> should be False

    [<Theory>]
    [<MemberData("adjacentHiLoLevels")>]
    let ``Method 'atLeast' matches 'higher' level when 'lower' is set`` (higher, lower) =
        higher
            |> Level.atLeast lower
            |> should be True

    [<Theory>]
    [<MemberData("adjacentHiLoLevels")>]
    let ``Method 'atLeast' doesn't match 'lower' level when 'higher' is set`` (higher, lower) =
        lower
            |> Level.atLeast higher
            |> should be False
