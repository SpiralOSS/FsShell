module SpiralOSS.FsShell.Infrastructure.Utility

open System.IO
open System.Text
open System

let readFile (encoding:Encoding) (path:string) =
    seq {
        use fs = new FileStream (path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        use sr = new StreamReader (fs, detectEncodingFromByteOrderMarks=true, encoding=encoding)
        while not sr.EndOfStream do
            yield sr.ReadLine ()
    }

type CursorPosition =
    | Before
    | After
    | Within of int
    static member MaxOf c1 c2 =
        match (c1,c2) with
        | (After,Before) -> c1
        | (Within x,Within y) when x > y -> c1
        | _ -> c2

let rangesToCursors (lastPosition:int) (ranges:(int*int) list) =
    ranges
    |> List.map (fun (ss, ee) -> (
        let toCursor num =
            match num with
            | num when num < 0 && (lastPosition + num) < -1 -> Before
            | num when num < 0 -> Within (lastPosition + num + 1)
            | num when num > lastPosition -> After
            | num -> Within num
        let newSS = toCursor ss
        let newEE = toCursor ee
        (newSS, (CursorPosition.MaxOf newSS newEE))
    ))

let stringRangeSplice (cursors:(CursorPosition*CursorPosition) list) (contents:string) =
    let lastPosition = contents.Length - 1
    cursors
    |> Seq.map (function
        | (Within x,Within y) -> contents[x..y]
        | (Within x,After)    -> contents[x..lastPosition]
        | (Before,Within y)   -> contents[0..y]
        | (Before,After)      -> contents
        | _                   -> ""
        )

let stringRangeSplice' (ranges:(int*int) list) (contents:string) =
    let lastIndex = contents.Length - 1
    stringRangeSplice (ranges |> rangesToCursors lastIndex) contents

let arrayRangeSplice (cursors:(CursorPosition*CursorPosition) list) (contents:string[]) =
    let lastPosition = contents.Length - 1
    cursors
    |> List.map (function
        | (Within x,Within y) -> contents[x..y]
        | (Within x,After)    -> contents[x..lastPosition]
        | (Before,Within y)   -> contents[0..y]
        | (Before,After)      -> contents
        | _                   -> [||]
    )
let arrayRangeSplice' (ranges:(int*int) list) (contents:string[]) =
    let lastIndex = contents.Length - 1
    arrayRangeSplice (ranges |> rangesToCursors lastIndex) contents
