module SpiralOSS.FsShell.Command.Output

open System.IO

let inline stdout (contents:'a seq) =
    contents
    |> Seq.iter (fun (content) -> Microsoft.FSharp.Core.Operators.stdout.WriteLine (content))

let inline stderr (contents:'a seq) =
    contents
    |> Seq.iter (fun (content) -> Microsoft.FSharp.Core.Operators.stderr.WriteLine (content))

let inline write (encoding:System.Text.Encoding) (append:bool) (path:string) (contents:'a seq) =
    use sw = new StreamWriter (path, append, encoding)
    contents
    |> Seq.iter (fun (content) -> sw.WriteLine (content))