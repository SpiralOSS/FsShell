module SpiralOSS.FsShell.Infrastructure.DataFile

open System.Collections.Generic
open System.Text

let ReadLine (separatorAndQuantifier:char*char) (row:string) =
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

let ReadFile (separatorAndQuantifier:char*char) (path:string) =
    Utility.ReadFile Encoding.UTF8 path
    |> Seq.map (fun line -> ReadLine separatorAndQuantifier line)

let DetectSeparatorAndQuantifier (path:string) =
    (' ', ' ')
