@echo off
cls

.paket\paket.bootstrapper.exe
.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

fake run --target build.fsx %*
