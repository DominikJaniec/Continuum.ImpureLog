namespace Continuum.Magic.ImpureLog

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
    let atLeast current given =
        given <> LvlNever
