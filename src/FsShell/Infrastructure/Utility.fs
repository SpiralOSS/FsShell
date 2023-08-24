module SpiralOSS.FsShell.Infrastructure.Utility

open System.IO
open System.Text

let readFile (encoding:Encoding) (path:string) =
    seq {
        use fs = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        use sr = new StreamReader (fs, detectEncodingFromByteOrderMarks=true, encoding=encoding)
        while not sr.EndOfStream do
            yield sr.ReadLine ()
    }

let mapContent func contents =
    Seq.map func contents

let lowerRanges (maxContentSize:int) (ranges:(int option*int option) list) =
    ranges
        |> List.map (fun (sOpt, eOpt) -> (
            (sOpt |> Option.defaultValue 0),
            (eOpt |> Option.defaultValue maxContentSize)
        ))
        |> List.map (fun (ss, ee) -> (
            (if ss < 0 then 0 else ss),
            (if ee > maxContentSize then maxContentSize else ee)
        ))

let stringSplice (ranges:(int option*int option) list) (contents:string) =
    let maxContentSize = contents.Length - 1
    seq {
        for (ss,ee) in (ranges |> lowerRanges maxContentSize) do
            yield contents[ss..ee]
    }

let rangeSplice (ranges:(int option*int option) list) (contents:string[]) =
    let maxContentSize = contents.Length - 1
    [
        for (ss,ee) in (ranges |> lowerRanges maxContentSize) do
            yield! Array.sub contents ss (ee - ss + 1)
    ]
