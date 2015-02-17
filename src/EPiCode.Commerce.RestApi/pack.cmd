@echo off
setlocal
SET CurrentDir=%~dp0
set version=%1
if [%1] == [] set version=1.0.0
nuget pack "%CurrentDir%EPiCode.Commerce.RestApi.nuspec" -Version %version%

