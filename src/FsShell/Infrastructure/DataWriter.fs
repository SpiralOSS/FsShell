module SpiralOSS.FsShell.Infrastructure.DataWriter

let dataToRow (separatorAndQuantifier:char*char) (row:string seq) : string =
    let (separator, quantifier) = separatorAndQuantifier
    row
    |> Seq.map (fun item -> item.Replace (quantifier.ToString(), $"{quantifier}{quantifier}"))
    |> Seq.map (fun item -> $"{quantifier}{item}{quantifier}")
    |> String.concat (separator.ToString())