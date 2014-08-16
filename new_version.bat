TASKKILL /F /IM "Read4Me.exe"

REM "C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /target:winexe /out:Read4Me.exe Read4Me\bin\x86\Release\Read4Me.exe Read4Me\bin\x86\Release\IdSharp.Common.dll Read4Me\bin\x86\Release\IdSharp.Tagging.dll Read4Me\bin\x86\Release\lame_enc.dll Read4Me\bin\x86\Release\yeti.mmedia.dll Read4Me\bin\x86\Release\yeti.mp3.dll


copy Read4Me\bin\x86\Release\Read4Me.exe C:\portable\Read4Me\Read4Me.exe
START C:\portable\Read4Me\Read4Me.exe 
copy Read4Me\bin\x86\Release\Read4Me.exe release\Read4Me\Read4Me.exe

REM http://superuser.com/questions/110991/can-you-zip-a-file-from-command-prompt
CScript  zip.vbs  D:\projects\software\Windows\Read4Me\release\  D:\projects\software\Windows\Read4Me\Read4Me_0.6.zip
