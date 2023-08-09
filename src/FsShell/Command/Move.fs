module SpiralOSS.FsShell.Command.Move

open System.IO

let move (sourcePath:string) (targetPath:string) =
    File.Move (sourcePath, targetPath)