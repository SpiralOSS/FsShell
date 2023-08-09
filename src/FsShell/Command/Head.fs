module SpiralOSS.FsShell.Command.Head

let inline head (count:int) (contents:'a seq) =
    contents |> Seq.takeSafe count

