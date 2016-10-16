set "PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\bin\amd64"
set "PATH=%PATH%;C:\Program Files\7-Zip"
call vcvars64
msbuild PayDay2SaveView.sln /p:Configuration=Release /p:Platform="Any CPU"
cd PayDay2SaveView\bin\Release\
7z a D:\LeOwnCloud\PD2SaveView.7z PayDay2SaveView.exe PayDay2SaveView.exe.config ..\..\Secrets.config.template *.dll
pause