module SpiralOSS.FsShell.Infrastructure.Utility

open System.IO
open System.Text

let ReadFile (encoding:Encoding) (path:string) =
    seq {
        use fs = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        use sr = new StreamReader (fs, detectEncodingFromByteOrderMarks=true, encoding=encoding)
        while not sr.EndOfStream do
            yield sr.ReadLine ()
    }

let MapContent func contents =
    Seq.map func contents
