@echo off

REM Borrowed from OpenVR Advanced Settings

REM Kill vrmonitor.exe and give SteamVR some time for a graceful shutdown
REM When no process was killed then exit (helps to prevent the taskkill clone bug)
(taskkill /im vrmonitor.exe /f >nul 2>nul && echo Give SteamVR some time for a graceful shutdown ... && timeout /t 20) || exit

REM Forcefully kill all remaining processes
taskkill /im vrcompositor.exe /f >nul 2>nul
taskkill /im vrdashboard.exe /f >nul 2>nul
taskkill /im vrserver.exe /f >nul 2>nul