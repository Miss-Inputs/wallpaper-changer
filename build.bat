@echo off
REM If you are me, you might have to uncomment this line the first time running this, if not ignore this
REM C:\Windows\Microsoft.NET\Framework\*\CasPol.exe -m -ag 1.2 -url file://\\VBOXSVR/Ubuntu_One/* FullTrust
REM csc /target:winexe /out:foo.exe /nologo /optimize /main:MainClass /define:NON_NATIVE_CONTEXT_MENU /win32icon:icon.ico /debug *.cs
REM TODO: Detect .NET versions >= 3.5 instead of hardcoding (note: it requires at least that due to usage of HashSet and object initializers)
C:\WINDOWS\Microsoft.NET\Framework\V3.5\csc /target:winexe /out:"Wallpaper changer.exe" /nologo /optimize /debug /win32icon:icon.ico /main:MainClass *.cs
