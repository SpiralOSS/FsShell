module SpiralOSS.FsShell.Command.Cut

open SpiralOSS.FsShell.Infrastructure.Utility

let cut_d (delimiter:char seq) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        content.Split ((Seq.toArray delimiter), System.StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
        )

let cut_c (ranges:(int option*int option) list) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        stringSplice ranges content
        |> String.concat ""
        )

open SpiralOSS.FsShell.Infrastructure.DataReader
let cut_x (contents:string seq) =
    let spAndQt =
        match Seq.tryHead contents with
        | Some content -> determineSpAndQt content
        | _ -> None
    contents
    |> Seq.map (readDataLine (Option.defaultValue (',','"') spAndQt))