namespace Continuum.Tests.ImpureLog.Tests.Component

open System
open System.IO
open Continuum.Magic.ImpureLog
open Continuum.Magic.ImpureLog.Component
open Continuum.Tests.ImpureLog.Fakes
open FsUnit
open FsUnitTyped.TopLevelOperators
open Microsoft.FSharp.Reflection
open Xunit

module LogsConfigTests =

    module ``The LogsConfig's property 'level'`` =

        [<Fact>]
        let ``equals 'LvlInfo'`` () =
            LogsConfig.level
                |> shouldEqual LvlInfo
