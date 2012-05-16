@echo off

echo .
echo . NAO execute em projetos existentes.
echo .
echo . VAI DAR MERDA!!!
echo .
echo . Caso esse nao seja um repositorio novo, feche o bat
echo .    NAO CONTINUE!!!
echo .

pause

cd..

xcopy tools\build\template\*.* /v /c /d

md config
md script
md src

svn add *.* --quiet
svn commit -m "commit inicial" --quiet

tools\build\nant\nant.exe -buildfile:main.build setup.ignore

svn commit -m "commit inicial" --quiet

tools\build\nant\nant.exe -buildfile:main.build setup

pause
