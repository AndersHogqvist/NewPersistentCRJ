'-----------------------------------------------------------------
'This script used to read the UserCfg.opt filename
'Match "InstalledPackagesPath" string and extract the path value
'-----------------------------------------------------------------

result = MsgBox("Do you want to copy the FSUIPC LVar Module to your community folder?", vbYesNo)
If result = vbYes Then
    'Variable declaration
    store_filename = CreateObject("WScript.Shell").ExpandEnvironmentStrings("%USERPROFILE%") & "\AppData\Local\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalCache\UserCfg.opt"
    steam_filename = CreateObject("WScript.Shell").ExpandEnvironmentStrings("%USERPROFILE%") & "\AppData\Roaming\Microsoft Flight Simulator\UserCfg.opt"
    instpath = "InstalledPackagesPath"

    'Create a file
    Const wr = 2
    Set objFso = CreateObject("Scripting.FileSystemObject")

    'Open the file
    Set fso = CreateObject("Scripting.FileSystemObject")

    Dim f
    If fso.FileExists(store_filename) Then
      Set f = fso.OpenTextFile(store_filename)
    ElseIf fso.FileExists(store_filename) Then
      Set f = fso.OpenTextFile(steam_filename)
    Else
        MsgBox "Unable to 'UserCfg.opt'. Please copy 'fsuipc-lvar-module' to your community folder manually.", vbOKOnly + vbExclamation
        wscript.quit
    End If

    'Read the file line by line
    Dim path

    Do Until f.AtEndOfStream
        Textline = f.ReadLine
        If Instr(Textline, instpath) then            'Match the install path string
            path = Replace(Textline, instpath, "")   'Remove the InstalledPackagesPath from main string
            path = Replace(path, """", "")
        end if
    Loop
    f.Close 'Close file

    If Len(path) = 0 Then
        MsgBox "Unable to find your community folder. Please copy 'fsuipc-lvar-module' to your community folder manually.", vbOKOnly + vbExclamation
        wscript.quit
    End If

    ' Copy LVar module folder to the community folder
    path = fso.BuildPath(path, "Community\fsuipc-lvar-module")

    Set objShell = CreateObject("Wscript.Shell")
    objShell.Run "cmd /c mkdir " & path

    install_path = Session.Property("CustomActionData")

    source_path = fso.BuildPath(install_path, "\fsuipc-lvar-module")

    objShell.Run "cmd /c Xcopy /E /I /Q /Y " & source_path & " " & path

End If