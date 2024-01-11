module SpiralOSS.FsShell.Command.Cut

open SpiralOSS.FsShell.Infrastructure.Utility
open SpiralOSS.FsShell.Infrastructure.DataReader

let cut_d (delimiter:char seq) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        content.Split ((Seq.toArray delimiter), System.StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
        )

let cut_c (ranges:(int*int) list) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        stringRangeSplice' ranges content
        |> String.concat ""
        )

let cut_x (contents:string seq) =
    let spAndQt =
        match Seq.tryHead contents with
        | Some content -> determineSpAndQt content
        | _ -> None
    contents
    |> Seq.map (readDataLine (Option.defaultValue (',','"') spAndQt))