mkdir tmp_nupack
cd tmp_nupack
mkdir lib
copy ..\LibCSV\bin\Release\LibCSV.dll lib
copy ..\LibCSV.nuspec .
nuget pack LibCSV.nuspec
move *.nupkg ..
cd ..
REM rmdir /S /Q tmp_nupack
