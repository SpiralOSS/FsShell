module SpiralOSS.FsShell.Command.Paste

open System.IO
open System.Text

let Paste (encoding:Encoding) (delimiter:string) (paths:string seq) =
    seq {
        // OPEN ALL THE FILES
        let srs =
            paths
            |> Seq.map (fun path -> new StreamReader (path, detectEncodingFromByteOrderMarks=true, encoding=encoding))
            |> Seq.toArray

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
