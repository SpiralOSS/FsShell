module SpiralOSS.FsShell.Command.Rmdir

open System.IO

let Rmdir (path:string) =
    Directory.Delete (path, recursive = true)