module SpiralOSS.FsShell.Command.Paste

open System.IO
open System.Text
open SpiralOSS.FsShell.Infrastructure

let paste (encoding:Encoding) (delimiter:string) (paths:string list) =
    seq {
        // OPEN ALL THE FILES
        let srs =
            paths
            |> List.map (fun path -> new StreamReader (path, detectEncodingFromByteOrderMarks=true, encoding=encoding))

        // YIELD EACH LINE PASTED TOGETHER
        while srs |> Seq.exists (fun sr -> not sr.EndOfStream) do
            srs
            |> Seq.map (fun sr ->
                if sr.EndOfStream then
                    ""
                else
                    sr.ReadLine ()
            )
            |> String.concat delimiter

        // CLOSE FILES
        for sr in srs do
            sr.Dispose ()
    }

let xpaste (separatorAndQuantifier:char*char) (contents:(string array) seq) =
    contents |> Seq.map (fun content -> DataWriter.dataToRow separatorAndQuantifier content)