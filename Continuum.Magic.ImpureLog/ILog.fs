namespace Continuum.Magic.ImpureLog

type ILog =
    inherit IFuncLog

    abstract member UsableFor : Level -> bool

    abstract member Message : Level -> string -> unit

    abstract member Always : string -> unit
    abstract member Panic : string -> unit
    abstract member Error : string -> unit
    abstract member Warn : string -> unit
    abstract member Info : string -> unit
    abstract member Debug : string -> unit
    abstract member Trace : string -> unit
