module SpiralOSS.FsShell.Command.Cd

open System.IO

let cd (path:string) =
    Directory.SetCurrentDirectory (path)
    Directory.GetCurrentDirectory ()