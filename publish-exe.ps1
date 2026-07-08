$ErrorActionPreference = "Stop"

$ProjectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $ProjectRoot

$PublishDir = Join-Path $ProjectRoot "publish\win-x64"

if (Test-Path $PublishDir) {
    Remove-Item $PublishDir -Recurse -Force
}

Write-Host "Restoring packages..." -ForegroundColor Cyan
dotnet restore .\ITInventoryManager.csproj

Write-Host "Building Release version..." -ForegroundColor Cyan
dotnet build .\ITInventoryManager.csproj -c Release

Write-Host "Publishing Windows x64 EXE..." -ForegroundColor Cyan
dotnet publish .\ITInventoryManager.csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -p:DebugType=None `
    -p:DebugSymbols=false `
    -o $PublishDir

Write-Host "" 
Write-Host "Publish completed successfully." -ForegroundColor Green
Write-Host "Output folder:" $PublishDir -ForegroundColor Green
Write-Host "Run:" (Join-Path $PublishDir "ITInventoryManager.exe") -ForegroundColor Green
