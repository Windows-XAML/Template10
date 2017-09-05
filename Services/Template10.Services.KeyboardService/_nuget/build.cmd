echo -- NuGet Process Start --

set TARGETDIR=%~1
set TARGETNAME=%~2
set PROJECTDIR=%~3
set NugetFolder=%PROJECTDIR%..\_nuget\
set WorkingDirectory=%TARGETDIR%nupkg

echo TARGETDIR=%TARGETDIR%
echo TARGETNAME=%TARGETNAME%
echo PROJECTDIR=%PROJECTDIR%
echo NugetFolder=%NugetFolder%
echo WorkingDirectory=%WorkingDirectory%

echo.%WorkingDirectory%|findstr /r "Release" >nul
if %errorlevel%==1 (
   echo CANCELLING Debug build - nuget must be Release
   goto end
)

echo.%WorkingDirectory%|findstr /C:"x86" >nul 2>&1
if %errorlevel%==0 (
   echo CANCELLING targets x86 - nuget must target AnyCPU
   goto end
)

echo.%WorkingDirectory%|findstr /C:"x64" >nul 2>&1
if %errorlevel%==0 (
   echo CANCELLING targets x64 - nuget must target AnyCPU
   goto end
)

echo.%WorkingDirectory%|findstr /C:"ARM" >nul 2>&1
if %errorlevel%==0 (
   echo CANCELLING targets ARM - nuget must target AnyCPU
   goto end
)

echo.
echo Step 1

:determine correct cache location
if "%NuGetCachePath%"=="" (  
    set NuGetCachePath=%USERPROFILE%\.nuget\packages\
)
set NuGetCachePath=%NuGetCachePath%Template10.Controls.ImageEx\

:clear nuget cache
if exist "%NuGetCachePath%" (
	echo OK Nuget cache will be cleared: %NuGetCachePath%
	rmdir "%NuGetCachePath%" /S/Q >nul
) else (
	echo OK No nuget cache to clear: %NuGetCachePath%
)

echo.
echo Step 2
:clear previous build
if exist "%WorkingDirectory%" (
	echo OK Previous build will be cleared: "%WorkingDirectory%"
	rmdir "%WorkingDirectory%" /S/Q >nul
) else (
	echo OK No previous build to clear: "%WorkingDirectory%"
) 

echo.
echo Step 3: Copy DLL (in root)

set DllFile="%TARGETDIR%%TARGETNAME%.dll"
set DllTarget="%WorkingDirectory%\lib\"

echo DllFile=%DllFile%
echo DllTarget=%DllTarget%
echo xcopy.exe %DllFile% %DllTarget% /y (.dll(s))

if exist %DllFile% (
    xcopy.exe %DllFile% %DllTarget% /y 
) else (
    echo OK DLL not found
    goto end
) 

echo.
echo Step 4: Copy PRI (in root)

set PriFile="%TARGETDIR%%TARGETNAME%.pri"
set PriTarget="%WorkingDirectory%\lib\"

echo PriFile=%PriFile%
echo PriTarget=%PriTarget%

echo xcopy.exe %PriFile% %PriTarget% /y 
if exist %PriFile% (
    xcopy.exe %PriFile% %PriTarget% /y 
) else (
    echo OK PRI not found
    goto end
) 

echo.
echo Step 5: Copy Library subfolder (requires Project/Properties (editor)/Build (tab)/[x]Generate library layout)

set LibSource="%TARGETDIR%%TARGETNAME%\*.*"
set LibTarget="%WorkingDirectory%\lib\%TARGETNAME%\"

echo LibSource=%LibSource%
echo LibTarget=%LibTarget%

echo xcopy.exe %LibSource% %LibTarget% /s/e/y (incl. XAML)
xcopy.exe %LibSource% %LibTarget% /s/e/y 

echo.
echo Step 6 (Copy MSBUild targets [optional])

set TargetFile="%NugetFolder%nuget.targets"
set TargetTarget="%WorkingDirectory%\build\"

echo TargetFile=%TargetFile%
echo TargetTarget=%TargetTarget%

echo xcopy.exe %TargetFile% %TargetTarget% /y 
if exist %TargetFile% (
    xcopy.exe %TargetFile% %TargetTarget% /y 
) else (
    echo OK targets not found
)    

echo.
echo Step 7 (Copy Init.ps1 [optional])
echo xcopy.exe xcopy.exe "%NugetFolder%init.ps1" "%WorkingDirectory%\Tools\" /y 
if exist "%NugetFolder%\init.ps1" (
    xcopy.exe "%NugetFolder%init.ps1" "%WorkingDirectory%\Tools\" /y 
) else (
    echo OK ps1 not found
)    

echo.
echo Step 8

set NuspecFile="%NugetFolder%nuget.nuspec"
set NuspecTarget="%WorkingDirectory%\"
set NuspecTargetFile="%WorkingDirectory%\nuget.nuspec"

echo NuspecFile=%NuspecFile%
echo NuspecTarget=%NuspecTarget%

echo xcopy.exe %NuspecFile% %NuspecTarget% /y 
if exist %NuspecFile% (
    xcopy.exe %NuspecFile% %NuspecTarget% /y 
) else (
    echo ERR nuspec source not found
    goto end
) 

if not exist %NuspecTarget% (
    echo ERR nuspec target not found
    goto end
)

echo.
echo Step 9

set NuspecFile=%NuspecTargetFile%
set NugetExe="%NugetFolder%nuget.exe"

echo NuspecFile=%NuspecFile%
echo NugetExe=%NugetExe%

echo %NugetExe% pack %NuspecFile% -Verbosity normal -OutputDirectory "%WorkingDirectory%" -NonInteractive
%NugetExe% pack %NuspecFile% -Verbosity normal -OutputDirectory "%WorkingDirectory%" -NonInteractive 

echo.
echo Step 10

set NupkegFile="%WorkingDirectory%\*.nupkg"
echo NupkegFile=%NupkegFile%

echo xcopy.exe %NupkegFile% "c:\nuget-local\" /y >nul
xcopy.exe %NupkegFile% "c:\nuget-local\" /y >nul

:end
echo -- NuGet Process End --
exit /b 0