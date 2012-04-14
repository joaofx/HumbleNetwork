@echo off
set /p version=[version]
tools\nant\nant.exe -buildfile:script\main.build package -D:version=%version%
pause





