$ErrorActionPreference = "Stop"

$ProjectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $ProjectRoot

$SubmissionDir = Join-Path $ProjectRoot "submission"
if (Test-Path $SubmissionDir) {
    Remove-Item $SubmissionDir -Recurse -Force
}
New-Item -ItemType Directory -Path $SubmissionDir | Out-Null

$SourceZip = Join-Path $SubmissionDir "Testview_ITInventory_Source_Yehuda_Benisti.zip"
$ExeZip = Join-Path $SubmissionDir "Testview_ITInventory_EXE_Yehuda_Benisti.zip"

$TempSourceDir = Join-Path $SubmissionDir "source_temp"
New-Item -ItemType Directory -Path $TempSourceDir | Out-Null

$ExcludedDirectories = @("bin", "obj", ".git", "publish", "Data", "submission", ".vs")
$ExcludedFiles = @("*.user", "*.suo")

Write-Host "Preparing source files..." -ForegroundColor Cyan

Get-ChildItem -Path $ProjectRoot -Recurse -File | Where-Object {
    $fullName = $_.FullName
    $relative = $fullName.Substring($ProjectRoot.Length).TrimStart('\', '/')
    $parts = $relative -split '[\\/]'

    $isExcludedDirectory = $false
    foreach ($dir in $ExcludedDirectories) {
        if ($parts -contains $dir) {
            $isExcludedDirectory = $true
            break
        }
    }

    $isExcludedFile = $false
    foreach ($pattern in $ExcludedFiles) {
        if ($_.Name -like $pattern) {
            $isExcludedFile = $true
            break
        }
    }

    -not $isExcludedDirectory -and -not $isExcludedFile
} | ForEach-Object {
    $relative = $_.FullName.Substring($ProjectRoot.Length).TrimStart('\', '/')
    $target = Join-Path $TempSourceDir $relative
    $targetDir = Split-Path -Parent $target
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    }
    Copy-Item $_.FullName $target -Force
}

Compress-Archive -Path (Join-Path $TempSourceDir "*") -DestinationPath $SourceZip -Force
Remove-Item $TempSourceDir -Recurse -Force

Write-Host "Source ZIP created:" $SourceZip -ForegroundColor Green

$PublishDir = Join-Path $ProjectRoot "publish\win-x64"
if (Test-Path $PublishDir) {
    Compress-Archive -Path (Join-Path $PublishDir "*") -DestinationPath $ExeZip -Force
    Write-Host "EXE ZIP created:" $ExeZip -ForegroundColor Green
} else {
    Write-Host "Publish folder was not found. Run .\publish-exe.ps1 first if you want an EXE ZIP." -ForegroundColor Yellow
}

$InfoFile = Join-Path $SubmissionDir "SUBMISSION_CONTENTS.txt"
@"
Submission package contents

1. Testview_ITInventory_Source_Yehuda_Benisti.zip
   Source code, README, AI usage documentation and helper files.

2. Testview_ITInventory_EXE_Yehuda_Benisti.zip
   Ready-to-run Windows EXE, created only if publish\win-x64 exists.

Recommended email contents:
- GitHub link
- Video link
- Source ZIP
- EXE ZIP if needed
"@ | Set-Content -Path $InfoFile -Encoding UTF8

Write-Host "" 
Write-Host "Submission folder ready:" $SubmissionDir -ForegroundColor Green
