ECHO ON
@echo -- NuGet Process Start --

set TARGETDIR=%~1
set TARGETNAME=%~2
set PROJECTDIR=%~3
set NUPKG=%TARGETDIR%nupkg
set CACHE="C:\Users\jerry\.nuget\packages\Template10\"

:clear nuget cache
if exist %CACHE% (
	echo OK Nuget cache will be cleared: %CACHE%
) else (
	echo OK No nuget cache to clear: %CACHE%
)

:clear previous build
if exist "%NUPKG%" (
	echo OK Previous build will be cleared: "%NUPKG%"
) else (
	echo OK No previous build to clear: "%NUPKG%"
)

:copy dll
xcopy.exe "%TARGETDIR%*.dll" "%NUPKG%\lib\" /y >null

:copy xr.xml
xcopy.exe "%TARGETDIR%%TARGETNAME%.xr.xml" "%NUPKG%\lib\%TARGETNAME%\" /y  >null

:copy pri
:https://msdn.microsoft.com/en-us/library/windows/apps/jj552947.aspx
xcopy.exe "%TARGETDIR%%TARGETNAME%.pri" "%NUPKG%\lib\" /y  >null

:copy rd.xml
xcopy.exe "%PROJECTDIR%Properties\%TARGETNAME%.rd.xml" "%NUPKG%\lib\%TARGETNAME%\Properties\" /y  >null

:copy xbf
xcopy.exe "%TARGETDIR%Controls" "%NUPKG%\lib\%TARGETNAME%\Controls\" /y  >null
xcopy.exe "%TARGETDIR%Styles" "%NUPKG%\lib\%TARGETNAME%\Styles\" /y  >null

:copy xaml
xcopy.exe "%PROJECTDIR%Controls\*.xaml" "%NUPKG%\lib\%TARGETNAME%\Controls\" /y  >null
xcopy.exe "%PROJECTDIR%Styles\*.xaml" "%NUPKG%\lib\%TARGETNAME%\Styles\" /y  >null

:copy msbuild
xcopy.exe "%PROJECTDIR%Nuget\*.targets" "%NUPKG%\build\" /y  >null

:copy init
xcopy.exe "%PROJECTDIR%Nuget\init.ps1" "%NUPKG%\Tools\" /y  >null

:copy nuspec
xcopy.exe "%PROJECTDIR%nuget\%TARGETNAME%.nuspec" "%NUPKG%" /y   >null

:execute pack
"%PROJECTDIR%nuget\NuGet.exe" pack "%NUPKG%\%TARGETNAME%.nuspec" -Build -Verbosity normal -OutputDirectory "%NUPKG%" -NonInteractive 

:copy nupkg
xcopy.exe "%NUPKG%\*.nupkg" c:\nuget-local\ /y  >null

echo -- NuGet Process End --