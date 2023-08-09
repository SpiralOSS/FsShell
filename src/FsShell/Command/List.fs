module SpiralOSS.FsShell.Command.List

open System.IO

let list (path:string) (searchPattern:string) =
    seq (Directory.EnumerateFiles (path, searchPattern))

let listDirectory (path:string) (searchPattern:string) =
    seq (Directory.EnumerateDirectories (path, searchPattern))

let listLong (path:string) (searchPattern:string) =
    list path searchPattern
    |> Seq.map (fun filepath ->
        let fileinfo = FileInfo (filepath)
        (
            filepath,
            fileinfo.Length,
            fileinfo
        )
    )