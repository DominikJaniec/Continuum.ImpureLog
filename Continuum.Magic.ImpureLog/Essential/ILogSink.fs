namespace Continuum.Magic.ImpureLog.Essential

type ILogEntry =
    abstract member Message : string with get
    abstract member Thing : string option with get

type ILogSink =
    abstract member Put : ILogEntry -> unit
