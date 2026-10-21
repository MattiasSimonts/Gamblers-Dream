@echo off
setlocal
echo Begin Build

set UNITY="C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe"
set PROJECT=%~dp0.

%UNITY% -batchmode -quit -nographics ^
  -projectPath "%PROJECT%" ^
  -executeMethod BuildScript.BuildWindows ^

if %ERRORLEVEL% neq 0 (
  echo Build FAILED - see build.log
  exit /b %ERRORLEVEL%
)
echo Build succeeded.

echo Done.
pause
endlocal
