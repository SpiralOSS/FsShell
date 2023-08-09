module SpiralOSS.FsShell.Command.Remove

open System.IO

let remove (path:string) =
    File.Delete (path)