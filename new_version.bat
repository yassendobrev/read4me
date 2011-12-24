

TASKKILL /F /IM "Read4Me.exe"
copy Read4Me\bin\Debug\Read4Me.exe C:\portable\Read4Me\Read4Me.exe
START C:\portable\Read4Me\Read4Me.exe 
copy Read4Me\bin\Debug\Read4Me.exe release\Read4Me\Read4Me.exe

REM http://superuser.com/questions/110991/can-you-zip-a-file-from-command-prompt
CScript  zip.vbs  D:\projects\software\Read4Me\release\  D:\projects\software\Read4Me\Read4Me_0.3.6.zip
