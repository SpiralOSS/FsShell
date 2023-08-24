module SpiralOSS.FsShell.Command.Join

open SpiralOSS.FsShell.Infrastructure

let inline xjoin (separatorAndQuantifier:char*char) (contents:(string array) seq) =
    contents |> Seq.map (fun content -> DataWriter.dataToRow separatorAndQuantifier content)