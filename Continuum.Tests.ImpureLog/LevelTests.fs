namespace Continuum.Tests.ImpureLog

open Continuum.Magic.ImpureLog
open FsUnit
open FsUnitTyped.TopLevelOperators
open Xunit

module LevelTests =

    module ``The Level's method 'atLeast'`` =

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
                |> shouldEqual true

        [<Fact>]
        let ``matches 'LvlAlways' when 'LvlAlways' is set`` () =
            LvlAlways
                |> Level.atLeast LvlAlways
                |> shouldEqual true

        [<Fact>]
        let ``matches 'LvlAlways' when 'LvlNever' is set`` () =
            LvlAlways
                |> Level.atLeast LvlNever
                |> shouldEqual true

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``doesn't match any standard level when 'LvlAlways' is set`` givenLvl =
            givenLvl
                |> Level.atLeast LvlAlways
                |> shouldEqual false

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``matches any standard level when 'LvlNever' is set`` givenLvl =
            givenLvl
                |> Level.atLeast LvlNever
                |> shouldEqual true

        [<Theory>]
        [<MemberData("everyStandardLevel")>]
        let ``doesn't match 'LvlNever' when any standard level is set`` currentLvl =
            LvlNever
                |> Level.atLeast currentLvl
                |> shouldEqual false

        [<Fact>]
        let ``doesn't match 'LvlNever' when 'LvlNever' is set`` () =
            LvlNever
                |> Level.atLeast LvlNever
                |> shouldEqual false

        [<Theory>]
        [<MemberData("adjacentHiLoLevels")>]
        let ``matches 'higher' level when 'lower' is set`` (higher, lower) =
            higher
                |> Level.atLeast lower
                |> shouldEqual true

        [<Theory>]
        [<MemberData("adjacentHiLoLevels")>]
        let ``doesn't match 'lower' level when 'higher' is set`` (higher, lower) =
            lower
                |> Level.atLeast higher
                |> shouldEqual false
