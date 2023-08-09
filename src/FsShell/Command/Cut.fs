module SpiralOSS.FsShell.Command.Cut

let cut_d (delimiter:char seq) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        content.Split ((Seq.toArray delimiter), System.StringSplitOptions.RemoveEmptyEntries) |> Seq.toList
    )

let cut_c (ranges:(int option*int option) list) (contents:string seq) =
    contents
    |> Seq.map (fun content ->
        ranges |>
        Seq.map (fun range ->
            let str = fst range |> Option.defaultValue 0
            let fin = snd range |> Option.defaultValue content.Length
            content[(str)..(fin)]
        )
        |> String.concat ""
    )

let cutx (contents:string seq) =
    contents
    |> Seq.map (SpiralOSS.FsShell.Infrastructure.DataFile.ReadLine (',','"'))