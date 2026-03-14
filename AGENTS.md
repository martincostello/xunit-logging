# Coding Agent Instructions

This file provides guidance to coding agents when working with code in this repository.

## Build, test, and lint commands

- Preferred entry point: run `./build.ps1` from the repository root. This script bootstraps the pinned SDK from `global.json` if needed, packs both library projects, and then runs the test suite in `Release`.
- Package without tests: run `./build.ps1 -SkipTests`.
- Run all tests directly: `dotnet test --configuration Release`.
- Run the xUnit v2 test project only: `dotnet test tests/Logging.XUnit.Tests/MartinCostello.Logging.XUnit.Tests.csproj --configuration Release`.
- Run the xUnit v3 test project only: `dotnet test tests/Logging.XUnit.v3.Tests/MartinCostello.Logging.XUnit.v3.Tests.csproj --configuration Release`.
- Run a single test in the v2 suite: `dotnet test tests/Logging.XUnit.Tests/MartinCostello.Logging.XUnit.Tests.csproj --configuration Release --filter "FullyQualifiedName~HttpApplicationTests.Http_Get_Many"`.
- Run a single test in the v3 suite: `dotnet test tests/Logging.XUnit.v3.Tests/MartinCostello.Logging.XUnit.v3.Tests.csproj --configuration Release --filter "FullyQualifiedName~HttpApplicationTests.Http_Get_Many"`.
- Local linting mirrors GitHub Actions:
  - PowerShell: `pwsh -NoProfile -Command "$settings = @{ IncludeDefaultRules = $true; Severity = @('Error', 'Warning') }; Invoke-ScriptAnalyzer -Path . -Recurse -ReportSummary -Settings $settings"`
  - Markdown: `markdownlint-cli2 "**/*.md"` if `markdownlint-cli2` is installed.
  - Workflow files: CI enforces `actionlint` and `zizmor`; run them locally if installed.

## High-level architecture

- The repository ships two NuGet packages: `src/Logging.XUnit` for xUnit v2 and `src/Logging.XUnit.v3` for xUnit v3.
- The implementation is intentionally shared. Both package projects link `src/Shared/**/*.cs` instead of maintaining separate source trees. Most behavioral changes belong in `src/Shared`, not in the per-package project folders.
- The v3 project sets the `XUNIT_V3` compilation constant. Shared files use `#if XUNIT_V3` only for the API differences between xUnit v2 and v3, such as `ITestOutputHelper` namespace changes and v3-specific test cancellation APIs.
- The main extension surface lives in shared partial types:
  - `XUnitLoggerExtensions.*` adds `AddXUnit(...)` overloads for `ILoggingBuilder` and `ILoggerFactory`.
  - `XUnitLoggerProvider.*` adapts either an `ITestOutputHelper` or an `IMessageSink` into `ILoggerProvider`.
  - `XUnitLogger.*` formats log output, applies filters, adds scopes, and writes to the current output helper or message sink.
  - `XUnitLoggerOptions` carries the configurable filter, scope, timestamp, and message factory behavior.
  - `AmbientTestOutputHelperAccessor`, `TestOutputHelperAccessor`, and `MessageSinkAccessor` are the bridge objects used to hand xUnit output infrastructure into the logger.
- Tests follow the same shared-source pattern as production code. `tests/Logging.XUnit.Tests` and `tests/Logging.XUnit.v3.Tests` both link `tests/Shared/**/*.cs`, so most test edits should be made once in `tests/Shared`.
- `tests/SampleApp` is a small ASP.NET Core app used by the shared integration tests under `tests/Shared/Integration`. If a change affects end-to-end logging behavior, check those integration tests, not just the unit-style logger tests.

## Key conventions

- The v2 and v3 projects deliberately share the same root namespace: `MartinCostello.Logging.XUnit`. Do not infer package identity from namespaces alone; use the project/package and the `XUNIT_V3` symbol.
- When changing library behavior, keep v2 and v3 in sync by editing shared files first and then running both test projects. A change that compiles in one project can still break the other because the same source is built twice with different dependencies and symbols.
- The shared tests are also compiled twice. Use `#if XUNIT_V3` inside shared tests only when the test API surface genuinely differs between xUnit versions.
- `AddXUnit()` without arguments relies on `AmbientTestOutputHelperAccessor`, which stores the current `ITestOutputHelper` in `AsyncLocal`. Tests and fixtures that set or clear the current output helper are exercising this behavior intentionally.
- Formatting behavior in `XUnitLogger` is important: timestamps, short log-level names (`crit`, `fail`, `dbug`, `info`, `warn`, `trce`), multiline indentation, and optional scope rendering are all covered by shared tests.
- The build is expected to stay warning-free. `.editorconfig`, `stylecop.json`, and the repo ruleset enforce CRLF line endings, a required file header in C# files, file-scoped namespaces, 4-space indentation for code, and 2-space indentation for project/config files.
- Outside Visual Studio, `Directory.Build.props` enables coverage collection with a 93% threshold and excludes `SampleApp` and xUnit assemblies from coverage. If you add tests or move code, keep that coverage behavior in mind.
- Tests use `Shouldly` for assertions and `NSubstitute` for doubles. Follow those existing patterns instead of introducing a different assertion or mocking style in shared tests.

## General guidelines

- Always ensure code compiles with no warnings or errors and tests pass locally before pushing changes.
- Do not change the public API unless specifically requested.
- Do not use APIs marked with `[Obsolete]`.
- Bug fixes should **always** include a test that would fail without the corresponding fix.
- Do not introduce new dependencies unless specifically requested.
- Do not update existing dependencies unless specifically requested.
