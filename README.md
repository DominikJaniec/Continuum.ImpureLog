# Continuum

What if the universe is an astronomical Lead Squid?

## Continuum Magic ImpureLog

Functional logability

----

## How to

* Build it: `$ dotnet build`
* Test it: `$ dotnet test Continuum.Tests.ImpureLog/`

## How it's made

1. Required: `dotnet` _".NET Core command-line interface"_ | [install](https://chocolatey.org/packages/dotnetcore-sdk) | [documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) |
2. Could be useful to execute: `$ export DOTNET_CLI_UI_LANGUAGE=en-US`
3. Steps executed in order:

```console
#// In the root directory:
dotnet new globaljson
dotnet new sln --name Continuum
dotnet new lib --language F# --name Continuum.Magic.ImpureLog
dotnet new xunit --language F# --name Continuum.Tests.ImpureLog
dotnet sln add Continuum.*/

#// In directory of: `Continuum.Tests.ImpureLog`
dotnet add package FsUnit
dotnet add package FsUnit.xUnit
dotnet add reference ../Continuum.Magic.ImpureLog/
```
