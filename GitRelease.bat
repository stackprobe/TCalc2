IF NOT EXIST .\GitRelease.bat GOTO END
CALL qq
C:\Factory\SubTools\GitFactory.exe /ow . C:\home\GitHub\TCalc2
rem CALL C:\var\go.bat
:END
