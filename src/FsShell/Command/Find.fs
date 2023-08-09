module SpiralOSS.FsShell.Command.Find

open System.IO

let find (startPath:string) (searchPattern:string) =
    seq (Directory.EnumerateFiles (startPath, searchPattern, SearchOption.AllDirectories))

let findPaths (startPath:string) (searchPattern:string) =
    seq (Directory.EnumerateDirectories (startPath, searchPattern, SearchOption.AllDirectories))
