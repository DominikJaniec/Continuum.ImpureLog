namespace Continuum.Tests.ImpureLog

open Continuum.Magic.ImpureLog
open FsUnit
open Xunit

module LevelTests =

    module ``The method 'atLeast'`` =

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
        let ``matches 'LvlAlways' when any standard level is set`` currentLvl =
            LvlAlways
                |> Level.atLeast currentLvl
                |> should be True

        [<Fact>]
        let ``matches 'LvlAlways' when 'LvlAlways' is set`` () =
            LvlAlways
                |> Level.atLeast LvlAlways
                |> should be True

        [<Fact>]
        let ``matches 'LvlAlways' when 'LvlNever' is set`` () =
            LvlAlways
                |> Level.atLeast LvlNever
                |> should be True

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``doesn't match any standard level when 'LvlAlways' is set`` givenLvl =
            givenLvl
                |> Level.atLeast LvlAlways
                |> should be False

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``matches any standard level when 'LvlNever' is set`` givenLvl =
            givenLvl
                |> Level.atLeast LvlNever
                |> should be True

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``doesn't match 'LvlNever' when any standard level is set`` currentLvl =
            LvlNever
                |> Level.atLeast currentLvl
                |> should be False

        [<Fact>]
        let ``doesn't match 'LvlNever' when 'LvlNever' is set`` () =
            LvlNever
                |> Level.atLeast LvlNever
                |> should be False

        [<Theory>]
        [<MemberData("adjacentHiLoLevels")>]
        let ``matches 'higher' level when 'lower' is set`` (higher, lower) =
            higher
                |> Level.atLeast lower
                |> should be True

        [<Theory>]
        [<MemberData("adjacentHiLoLevels")>]
        let ``doesn't match 'lower' level when 'higher' is set`` (higher, lower) =
            lower
                |> Level.atLeast higher
                |> should be False
