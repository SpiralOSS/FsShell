module SpiralOSS.FsShell.Command.Cat

open System.Text

let cat (encoding:Encoding) (paths:string seq) =
    seq {
        for path in paths do
            yield! SpiralOSS.FsShell.Infrastructure.Utility.readFile encoding path
    }