module SpiralOSS.FsShell.Command.Mkdir

open System.IO

let mkdir (path:string) =
    let createdDirInfo = Directory.CreateDirectory (path)
    createdDirInfo.FullName
