namespace Continuum.Magic.ImpureLog

type IFuncLog =
    abstract member stateAs : Level -> string -> unit
    abstract member stateThatAs : Level -> string -> 'a -> unit

    abstract member justAs : Level -> string -> 'a -> 'a
    abstract member justThatAs : Level -> string -> 'a -> 'a

