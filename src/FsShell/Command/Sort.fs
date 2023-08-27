module SpiralOSS.FsShell.Command.Sort

open SpiralOSS.FsShell.Infrastructure.Utility
open System.Linq

let sort (contents:string seq) = seq { yield! contents.OrderBy(fun it -> it) }

let sort_k (ranges:(int*int)) (contents:string seq) =
    seq { yield! contents.OrderBy (fun content -> (stringRangeSplice' [ ranges ] content) |> String.concat "") }

let sort_kn (ranges:(int*int)) (contents:string seq) =
    contents |> Seq.sortBy (fun content ->
        let sortNum = stringRangeSplice' [ ranges ] content |> String.concat ""
        let (success, number) = System.Int64.TryParse(sortNum)
        if success then number else 0L
        )

type SortBy =
    | String
    | Numeric

module private Utility =
    let rec xSortThenBy (thenBy:(int*SortBy)->string array->'a when 'a : comparison) (retSeq:IOrderedEnumerable<string array>) (columnIndices:(int*SortBy) list) (contents:string[] seq) =
        match columnIndices with
        | [] -> retSeq
        | sortBy::sortBys -> xSortThenBy thenBy (retSeq.ThenBy(thenBy sortBy)) sortBys contents
    let inline stringSort (index,_) (content:string array) =
        content[index] :> System.IComparable
    let inline numericSort (index,_) (content:string array) =
        let (success, number) = System.Int64.TryParse(content[index])
        (if success then number else 0L) :> System.IComparable
    let inline eitherSort (index, sortType) (content:string array) =
        match sortType with
        | String -> stringSort (index, sortType) content
        | Numeric -> numericSort (index, sortType) content

    let orderBy (contents:string[] seq) = (contents.OrderBy(fun _ -> 0))

let xsort (columnIndices:int list) (contents:string[] seq) =
    seq { yield! Utility.xSortThenBy Utility.stringSort (contents.OrderBy(fun _ -> 0)) (columnIndices |> List.map (fun it -> (it, String))) contents }

let xsort_n (columnIndices:int list) (contents:string[] seq) =
    seq { yield! Utility.xSortThenBy Utility.numericSort (contents.OrderBy(fun _ -> 0)) (columnIndices |> List.map (fun it -> (it, Numeric))) contents }

let xsort' (columnIndices:(int*SortBy) list) (contents:string[] seq) =
    seq { yield! Utility.xSortThenBy Utility.eitherSort (contents.OrderBy(fun _ -> 0)) columnIndices contents }