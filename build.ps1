# QRCreator Build Script (Windows)
# Usage: .\build.ps1 [-Version "1.0.1"]

param(
    [string]$Version = "1.0.1"
)

$ErrorActionPreference = "Stop"
$ProjectDir = "QRCreator.Avalonia"
$ProjectFile = "$ProjectDir/QRCreator.Avalonia.csproj"
$PackId = "QRCreator"
$IconPath = "$ProjectDir/Assets/qrcreator_icon.ico"
$OutputDir = "./releases/win"
$PublishDir = "./publish/win-x64"

Write-Host "=== QRCreator Build (Windows x64) v$Version ===" -ForegroundColor Cyan

# Clean
Write-Host "`n[1/3] Publishing..." -ForegroundColor Yellow
if (Test-Path $PublishDir) { Remove-Item -Recurse -Force $PublishDir }
if (Test-Path $OutputDir) { Remove-Item -Recurse -Force $OutputDir }

dotnet publish $ProjectFile `
    -c Release `
    -r win-x64 `
    --self-contained `
    -o $PublishDir `
    -p:PublishSingleFile=false

if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

# Pack with Velopack
Write-Host "`n[2/3] Packing with Velopack..." -ForegroundColor Yellow
vpk pack `
    -u $PackId `
    -v $Version `
    -p $PublishDir `
    -o $OutputDir `
    -r win-x64 `
    -e "QRCreator.Avalonia.exe" `
    -i $IconPath `
    --packTitle "QR Creator" `
    --packAuthors "rv.ding"

if ($LASTEXITCODE -ne 0) { throw "vpk pack failed" }

# Summary
Write-Host "`n[3/3] Done!" -ForegroundColor Green
Write-Host "Output: $OutputDir" -ForegroundColor Cyan
Get-ChildItem $OutputDir | Format-Table Name, @{N="Size(MB)";E={[math]::Round($_.Length/1MB,1)}}
