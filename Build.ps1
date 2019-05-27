param(
    [Parameter(Mandatory = $false)][string] $Configuration = "Release",
    [Parameter(Mandatory = $false)][string] $VersionSuffix = "",
    [Parameter(Mandatory = $false)][string] $OutputPath = "",
    [Parameter(Mandatory = $false)][switch] $SkipTests,
    [Parameter(Mandatory = $false)][switch] $DisableCodeCoverage
)

$ErrorActionPreference = "Stop"

$solutionPath = Split-Path $MyInvocation.MyCommand.Definition
$solutionFile = Join-Path $solutionPath "Logging.XUnit.sln"
$sdkFile = Join-Path $solutionPath "global.json"

$libraryProject = Join-Path $solutionPath "src\Logging.XUnit\MartinCostello.Logging.XUnit.csproj"

$testProjects = @(
    (Join-Path $solutionPath "tests\Logging.XUnit.Tests\MartinCostello.Logging.XUnit.Tests.csproj")
)

$dotnetVersion = (Get-Content $sdkFile | Out-String | ConvertFrom-Json).sdk.version

if ($OutputPath -eq "") {
    $OutputPath = Join-Path "$(Convert-Path "$PSScriptRoot")" "artifacts"
}

$installDotNetSdk = $false;

if (($null -eq (Get-Command "dotnet.exe" -ErrorAction SilentlyContinue)) -and ($null -eq (Get-Command "dotnet" -ErrorAction SilentlyContinue))) {
    Write-Host "The .NET Core SDK is not installed."
    $installDotNetSdk = $true
}
else {
    Try {
        $installedDotNetVersion = (dotnet --version 2>&1 | Out-String).Trim()
    }
    Catch {
        $installedDotNetVersion = "?"
    }

    if ($installedDotNetVersion -ne $dotnetVersion) {
        Write-Host "The required version of the .NET Core SDK is not installed. Expected $dotnetVersion."
        $installDotNetSdk = $true
    }
}

if ($installDotNetSdk -eq $true) {
    $env:DOTNET_INSTALL_DIR = Join-Path "$(Convert-Path "$PSScriptRoot")" ".dotnetcli"
    $sdkPath = Join-Path $env:DOTNET_INSTALL_DIR "sdk\$dotnetVersion"

    if (!(Test-Path $sdkPath)) {
        if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
            mkdir $env:DOTNET_INSTALL_DIR | Out-Null
        }
        $installScript = Join-Path $env:DOTNET_INSTALL_DIR "install.ps1"
        Invoke-WebRequest "https://dot.net/v1/dotnet-install.ps1" -OutFile $installScript -UseBasicParsing
        & $installScript -Version "$dotnetVersion" -InstallDir "$env:DOTNET_INSTALL_DIR" -NoPath
    }

    $env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"
    $dotnet = Join-Path "$env:DOTNET_INSTALL_DIR" "dotnet.exe"
}
else {
    $dotnet = "dotnet"
}

function DotNetBuild {
    param([string]$Project)

    if ($VersionSuffix) {
        & $dotnet build $Project --output $OutputPath --configuration $Configuration --version-suffix "$VersionSuffix"
    }
    else {
        & $dotnet build $Project --output $OutputPath --configuration $Configuration
    }
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build failed with exit code $LASTEXITCODE"
    }
}

function DotNetPack {
    param([string]$Project)

    if ($VersionSuffix) {
        & $dotnet pack $Project --output $OutputPath --configuration $Configuration --version-suffix "$VersionSuffix" --include-symbols --include-source
    }
    else {
        & $dotnet pack $Project --output $OutputPath --configuration $Configuration --include-symbols --include-source
    }
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet pack failed with exit code $LASTEXITCODE"
    }
}

function DotNetTest {
    param([string]$Project)

    if ($DisableCodeCoverage -eq $true) {
        if ($null -ne $env:TF_BUILD) {
            & $dotnet test $Project --output $OutputPath --framework $framework --no-build --logger trx
        }
        else {
            & $dotnet test $Project --output $OutputPath --framework $framework --no-build
        }
    }
    else {

        if ($installDotNetSdk -eq $true) {
            $dotnetPath = $dotnet
        }
        else {
            $dotnetPath = (Get-Command "dotnet.exe").Source
        }

        $nugetPath = Join-Path $env:USERPROFILE ".nuget\packages"

        $openCoverVersion = "4.7.922"
        $openCoverPath = Join-Path $nugetPath "OpenCover\$openCoverVersion\tools\OpenCover.Console.exe"

        $reportGeneratorVersion = "4.1.8"
        $reportGeneratorPath = Join-Path $nugetPath "ReportGenerator\$reportGeneratorVersion\tools\netcoreapp2.0\ReportGenerator.dll"

        $coverageOutput = Join-Path $OutputPath "code-coverage.xml"
        $reportOutput = Join-Path $OutputPath "coverage"

        if ($null -ne $env:TF_BUILD) {
            & $openCoverPath `
                `"-target:$dotnetPath`" `
                `"-targetargs:test $Project --output $OutputPath --logger trx`" `
                -output:$coverageOutput `
                `"-excludebyattribute:System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage*`" `
                -hideskipped:All `
                -mergebyhash `
                -mergeoutput `
                -oldstyle `
                -register:user `
                -skipautoprops `
                `"-filter:+[MartinCostello.Logging.XUnit]* -[MartinCostello.Logging.XUnit.Tests]*`"
        }
        else {
            & $openCoverPath `
                `"-target:$dotnetPath`" `
                `"-targetargs:test $Project --output $OutputPath`" `
                -output:$coverageOutput `
                `"-excludebyattribute:System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage*`" `
                -hideskipped:All `
                -mergebyhash `
                -mergeoutput `
                -oldstyle `
                -register:user `
                -skipautoprops `
                `"-filter:+[MartinCostello.Logging.XUnit]* -[MartinCostello.Logging.XUnit.Tests]*`"
        }

        $dotNetTestExitCode = $LASTEXITCODE

        & $dotnet `
            $reportGeneratorPath `
            `"-reports:$coverageOutput`" `
            `"-targetdir:$reportOutput`" `
            -reporttypes:HTML`;Cobertura `
            -verbosity:Warning
    }

    if ($dotNetTestExitCode -ne 0) {
        throw "dotnet test failed with exit code $dotNetTestExitCode"
    }
}

Write-Host "Building solution..." -ForegroundColor Green
DotNetBuild $solutionFile

Write-Host "Packaging library..." -ForegroundColor Green
DotNetPack $libraryProject

Write-Host "Running tests..." -ForegroundColor Green
ForEach ($testProject in $testProjects) {
    DotNetTest $testProject
}
