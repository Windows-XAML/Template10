@echo off
echo INSTALLING SNIPPETS
for /f "tokens=3*" %%p in ('REG QUERY "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v Personal') do (
    set DocumentsFolder=%%p
)
set SnippetsFolder="%DocumentsFolder%\Visual Studio 2015\Code Snippets\Visual C#\My Code Snippets\"
set LocalFolder="%~dp0%*T10_*.snippet"
xcopy.exe %LocalFolder% %SnippetsFolder% /y >nul