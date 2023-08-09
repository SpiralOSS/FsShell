module SpiralOSS.FsShell.Command.Copy

open System.IO

let copy (sourcePath:string) (targetPath:string) =
    File.Copy (sourcePath, targetPath)

