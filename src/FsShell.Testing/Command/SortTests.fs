module SpiralOSS.FsShell.Testing.Command.SortTests

open Xunit
open System.Linq
open SpiralOSS.FsShell.Command.Sort

[<Fact>]
let ``Test Sort`` () =
    let input  = seq { "C"; "B"; "A" }
    let expect = seq { "A"; "B"; "C" }
    let actual = sort input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Sort Ignore Case`` () =
    let input  = seq { "C"; "b"; "A" }
    let expect = seq { "A"; "b"; "C" }
    let actual = sort input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Sort Range`` () =
    let input  = seq { "AC"; "BB"; "CA" }
    let expect = seq { "CA"; "BB"; "AC" }
    let actual = sort_k (1, 1) input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Sort Range Ignore Case`` () =
    let input  = seq { "AC"; "Bb"; "Ca" }
    let expect = seq { "Ca"; "Bb"; "AC" }
    let actual = sort_k (1, 1) input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Sort Number Range`` () =
    let input  = seq { "A31"; "B21"; "C11" }
    let expect = seq { "C11"; "B21"; "A31" }
    let actual = sort_kn (1, 2) input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test XSort`` () =
    let input = seq {
        [| "Val 1-3" ; "Val 2-3"; "Val 3-3" |]
        [| "Col   1" ; "Col   2"; "Col   3" |]
        [| "Val 1-2" ; "Val 2-2"; "Val 3-2" |]
        [| "Val 1-1" ; "Val 2-1"; "Val 3-1" |]
    }
    let expect = seq {
        [| "Col   1" ; "Col   2"; "Col   3" |]
        [| "Val 1-1" ; "Val 2-1"; "Val 3-1" |]
        [| "Val 1-2" ; "Val 2-2"; "Val 3-2" |]
        [| "Val 1-3" ; "Val 2-3"; "Val 3-3" |]
    }
    let actual = xsort [ 1 ] input
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))

[<Fact>]
let ``Test XSort Multiple`` () =
    let input = seq {
        [| "Val 1-3" ; "Val B"  ; "Val   3" |]
        [| "Val 1-2" ; "Val A"  ; "Val   2" |]
        [| "Val 1-1" ; "Val A"  ; "Val   1" |]
        [| "Col   1" ; "Col   2"; "Col   3" |]
    }
    let expect = seq {
        [| "Col   1" ; "Col   2"; "Col   3" |]
        [| "Val 1-1" ; "Val A"  ; "Val   1" |]
        [| "Val 1-2" ; "Val A"  ; "Val   2" |]
        [| "Val 1-3" ; "Val B"  ; "Val   3" |]
    }
    let actual = xsort [ 1; 2 ] input
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))

[<Fact>]
let ``Test XSort Ignore Case`` () =
    let input = [
        [| "Col   1" ; "Col   2"; "Col   3" |]
        [| "Val 1-1" ; "val 1-1"; "Val 3-1" |]
        [| "Val 1-3" ; "val 2-3"; "Val 3-3" |]
        [| "Val 1-2" ; "Val 2-2"; "Val 3-2" |]
    ]
    let expect = [
        [| "Col   1" ; "Col   2"; "Col   3" |]
        [| "Val 1-1" ; "val 1-1"; "Val 3-1" |]
        [| "Val 1-2" ; "Val 2-2"; "Val 3-2" |]
        [| "Val 1-3" ; "val 2-3"; "Val 3-3" |]
    ]
    let actual = xsort [ 1 ] input |> Seq.toList
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))

[<Fact>]
let ``Test XSort Numeric`` () =
    let input = seq {
        [| "3"; "3" |]
        [| "4"; "4" |]
        [| "1"; "1" |]
        [| "2"; "2" |]
    }
    let expect = seq {
        [| "1"; "1" |]
        [| "2"; "2" |]
        [| "3"; "3" |]
        [| "4"; "4" |]
    }
    let actual = xsort_n [ 1 ] input
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))

[<Fact>]
let ``Test XSort Numeric Multiple`` () =
    let input = [
        [| "4"; "3"; "10" |]
        [| "4"; "4"; "10" |]
        [| "1"; "1"; "00" |]
        [| "4"; "4"; "20" |]
        [| "2"; "1"; "10" |]
        [| "3"; "1"; "20" |]
        [| "4"; "2"; "00" |]
    ]
    let expect = [
        [| "1"; "1"; "00" |]
        [| "2"; "1"; "10" |]
        [| "3"; "1"; "20" |]
        [| "4"; "2"; "00" |]
        [| "4"; "3"; "10" |]
        [| "4"; "4"; "10" |]
        [| "4"; "4"; "20" |]
    ]
    let actual = xsort_n [ 1; 2 ] input |> Seq.toArray
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))

[<Fact>]
let ``Test XSort Mixed`` () =
    let input = [
        [| "4"; "C"; "10" |]
        [| "4"; "D"; "10" |]
        [| "1"; "A"; "00" |]
        [| "4"; "D"; "20" |]
        [| "2"; "A"; "10" |]
        [| "3"; "A"; "20" |]
        [| "4"; "B"; "00" |]
    ]
    let expect = [
        [| "1"; "A"; "00" |]
        [| "2"; "A"; "10" |]
        [| "3"; "A"; "20" |]
        [| "4"; "B"; "00" |]
        [| "4"; "C"; "10" |]
        [| "4"; "D"; "10" |]
        [| "4"; "D"; "20" |]
    ]
    let actual = xsort' [ (1, String); (2, Numeric) ] input |> Seq.toArray
    for (expectRow, actualRow) in (expect |> Seq.zip actual) do
        Assert.True(expectRow.SequenceEqual(actualRow))