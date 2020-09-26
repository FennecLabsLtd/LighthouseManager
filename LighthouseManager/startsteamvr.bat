REM Borrowed from OpenVR Advanced Settings

REM First (optional) parameter is the vr runtime path
REM When no parameter given use default steam install location
IF [%1] == [] (
	SET "steamVRPath=C:\Program Files (x86)\Steam\steamapps\common\SteamVR\"
) ELSE (
	SET "steamVRPath=%~1"
)
echo VR Runtime Path: %steamVRPath%
echo.

start "" "%steamVRPath%\bin\win32\vrstartup.exe"