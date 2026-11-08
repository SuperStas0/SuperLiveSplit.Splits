@echo off
setlocal
cd /d "%~dp0"
set "OUT=%~dp0artifacts\bin\LiveSplit.Splits\release"
set "FIXEROUT=%~dp0artifacts\bin\SplitsFixer\release"
set "INSTALLDIR=C:\Program Files (x86)\LiveSplit\Components"
set "RELDIR=%~dp0release"
set "CLEAN="
set "INSTALL="
set "RELEASE="
set "HELP="
for %%a in (%*) do (
    echo %%a| findstr /r /x /c:"-[cirh][cirh]*" >nul || (echo Unknown flag: %%a. Run build.bat -h for usage.& exit /b 1)
    echo %%a| findstr /c:"c" >nul && set "CLEAN=1"
    echo %%a| findstr /c:"i" >nul && set "INSTALL=1"
    echo %%a| findstr /c:"r" >nul && set "RELEASE=1"
    echo %%a| findstr /c:"h" >nul && set "HELP=1"
)
ver >nul
if defined HELP echo Usage: build.bat [-c] [-i] [-r] [-h]
if defined HELP echo   -c  clean: delete previous build output first
if defined HELP echo   -i  install: copy LiveSplit.Splits.dll to %INSTALLDIR%
if defined HELP echo   -r  release: package LiveSplit.Splits.dll and SplitsFixer.exe into %RELDIR%
if defined HELP echo   -h  show this help
if defined HELP echo Build output: %OUT%
if defined HELP echo Fixer output: %FIXEROUT%
if defined HELP echo Release:      %RELDIR%
if defined HELP exit /b 0
if defined CLEAN if exist "%~dp0artifacts" rmdir /s /q "%~dp0artifacts" & if errorlevel 1 exit /b 1
dotnet build "%~dp0LiveSplit.Splits.sln" -c Release --nologo
if errorlevel 1 exit /b 1
if defined INSTALL copy /y "%OUT%\LiveSplit.Splits.dll" "%INSTALLDIR%\LiveSplit.Splits.dll" >nul & if errorlevel 1 exit /b 1
if defined RELEASE if not exist "%RELDIR%" mkdir "%RELDIR%"
if defined RELEASE copy /y "%OUT%\LiveSplit.Splits.dll" "%RELDIR%\LiveSplit.Splits.dll" >nul & if errorlevel 1 exit /b 1
if defined RELEASE copy /y "%FIXEROUT%\SplitsFixer.exe" "%RELDIR%\SplitsFixer.exe" >nul & if errorlevel 1 exit /b 1
exit /b 0
