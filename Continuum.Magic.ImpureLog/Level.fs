namespace Continuum.Magic.ImpureLog

open System

type Level
    = LvlPriority of int
    | LvlAlways
    | LvlPanic
    | LvlError
    | LvlWarn
    | LvlInfo
    | LvlDebug
    | LvlTrace
    | LvlNever

module Level =
    let private asPriority level =
        match level with
        | LvlPriority value -> value
        | LvlAlways -> Int32.MaxValue
        | LvlPanic -> 1000000
        | LvlError -> 10000
        | LvlWarn -> 100
        | LvlInfo -> 0
        | LvlDebug -> -100
        | LvlTrace -> -10000
        | LvlNever -> Int32.MinValue

    let atLeast currentLevel level =
        let currentPriority = (currentLevel |> asPriority)
        let givenPriority = (level |> asPriority)
        givenPriority > (LvlNever |> asPriority)
            && givenPriority >= currentPriority


