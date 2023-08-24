module SpiralOSS.FsShell.Infrastructure.DataReader

open System.Collections.Generic
open System.Text

let readDataLine (separatorAndQuantifier:char*char) (row:string) =
    let (separator, quantifier) = separatorAndQuantifier
    let columns = List<string>()
    let column = StringBuilder()
    let mutable isQuantifiedColumn = false
    let mutable wasOnQuantifier = false

    for chr in row.ToCharArray() do
        if chr = quantifier then
            match (isQuantifiedColumn, wasOnQuantifier) with
            | (false, false) ->
                isQuantifiedColumn <- true
            | (false, true) ->
                failwith "Unexpected quantifier"
            | (true, false) ->
                wasOnQuantifier <- true
            | (true, true) ->
                wasOnQuantifier <- false
                column.Append(quantifier) |> ignore

        elif chr = separator then
            match (isQuantifiedColumn, wasOnQuantifier) with
            | (false, false) ->  // abc,
                columns.Add(column.ToString())
                column.Clear() |> ignore
            | (true, true) ->  // "abc",
                isQuantifiedColumn <- false
                wasOnQuantifier <- false
                columns.Add(column.ToString())
                column.Clear() |> ignore
            | (false, true) ->  // abc",
                failwith "Unexpected quantifier after separator"
            | (true, false) ->  // "abc"",
                failwith "Unmatched quantifier"
        else
            column.Append(chr) |> ignore

    columns.Add(column.ToString())
    columns.ToArray()

let readDataFile (separatorAndQuantifier:char*char) (path:string) =
    Utility.readFile Encoding.UTF8 path
    |> Seq.map (fun line -> readDataLine separatorAndQuantifier line)

let determineSeparatorAndQuantifier isValidSp isValidQt (getSpFromQt:char->char option) (getQtFromSp:char->char option) (line:string) : (char*char) option =

    if line.Length < 1 then
        None
    else
        let isValidQtOrSp (chr:char) = isValidQt chr || isValidSp chr

        // IF STARTS WITH A VALID Q, EXPECT IT
        let expectQt = if isValidQt line[0] then Some line[0] else None
        let mutable firstSp : char option = None

        // FIND A VALID SEPARATOR AND QUANTIFIER IN LINE
        //  or None
        let validSpQt =
            line.ToCharArray()
                |> Seq.skip 1                 // skip first character
                |> Seq.filter isValidQtOrSp   // only possible quote or separator
                |> Seq.iterTee (fun chr ->    // capture the first valid separator
                    firstSp <- if (firstSp.IsNone && isValidSp chr) then Some chr else firstSp
                )
                |> Seq.windowed 3             // three characters in a group
                |> Seq.filter (fun window ->  // find a {Quote}{Separator}{Quote} pattern
                    let isQSQ =
                        (window[0] = (expectQt |> Option.orElse (Some window[2]) |> Option.get)) &&
                        (isValidSp window[1]) &&
                        (window[2] = (expectQt |> Option.orElse (Some window[0]) |> Option.get))
                    //printfn "%A -> %b" window isQSQ
                    isQSQ && window[0] <> window[1]
                )
                |> Seq.tryHead
                |> Option.map (fun qsq -> (qsq[1], qsq[0]))

        if validSpQt.IsSome then
            validSpQt
        else
            printfn "%A or %A | %A" firstSp (getSpFromQt line[0]) expectQt
            match (firstSp |> Option.orElse (getSpFromQt line[0]), expectQt) with
            | (Some sp, Some qt) -> Some (sp, qt)
            | (Some sp, None) ->
                match getQtFromSp sp with
                | Some qt -> Some (sp, qt)
                | _ -> None
            | _ -> None

let commonSpFromQt (chr:char) =
    match chr with
    | '"' -> Some ','
    | '|' -> Some '^'
    | '^' -> Some '|'
    | '\uc3be' -> (Some ((char)20uy))
    | it when it = (char)254uy -> (Some ((char)20uy))
    | _ -> None

let commonQtFromSp (chr:char) =
    match chr with
    | ',' -> Some '"'
    | '|' -> Some '^'
    | '^' -> Some '|'
    | it when it = (char)20uy -> (Some '\uc3be')
    | _ -> None

let determineSpAndQt =
    determineSeparatorAndQuantifier
        (fun chr -> List.contains chr [ ','; '^'; '|'; (char)20uy ])
        (fun chr -> List.contains chr [ '"'; '^'; '|'; '\uc3be'; '\u00fe'; (char)254uy ])
        commonSpFromQt
        commonQtFromSp