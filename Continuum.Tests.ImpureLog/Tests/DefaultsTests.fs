namespace Continuum.Tests.ImpureLog.Tests

open Continuum.Magic.ImpureLog
open FsUnit
open FsUnitTyped.TopLevelOperators
open Xunit

module DefaultsTests =

    module ``The Defaults' property 'level'`` =

        [<Fact>]
        let ``equals 'LvlInfo'`` () =
            Defaults.level
                |> shouldEqual LvlInfo


    module ``The Defaults' method 'sculp'`` =

        [<Fact>]
        let ``is stringifier which behaves like 'sprintf' with format "%A"`` () =
            let complexThing = ("this-is", [ "very"; "complex" ], 42, 13, 7)
            let formatted = sprintf "%A" complexThing
            Defaults.sculp complexThing
                |> shouldEqual formatted
