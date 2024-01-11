module SpiralOSS.FsShell.Testing.Command.CutTests

open Xunit
open System.Linq
open SpiralOSS.FsShell.Command.Cut
open SpiralOSS.FsShell.Testing.Infrastructure

[<Fact>]
let ``Test Cut_d - whitespace`` () =
    let input = "Quick brown,fox,jumps\tover the lazy    dog  "
    let expect = [ "Quick";"brown,fox,jumps";"over";"the";"lazy";"dog" ]
    let actual = Utility.AsSingleton (cut_d [ ' ';'\t' ]) input
    Assert.True(expect.SequenceEqual(actual));

[<Fact>]
let ``Test Cut_d - spaces`` () =
    let input = "Quick brown fox jumps over the   lazy dog    "
    let expect = ["Quick";"brown";"fox";"jumps";"over";"the";"lazy";"dog"]
    let actual = Utility.AsSingleton (cut_d [' ']) input
    Assert.True(expect.SequenceEqual(actual));

[<Fact>]
let ``Test Cut_c - None to 4`` () =
    let input = "Quick brown,fox,jumps\tover the lazy    dog  "
    let expect = "Quick"
    let actual = Utility.AsSingleton (cut_c [(0, 4)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - 0 to 4, 11 to 14`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = "Quick fox"
    let actual = Utility.AsSingleton (cut_c [(0, 4); (11, 14)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - 31 to None`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = "lazy dog"
    let actual = Utility.AsSingleton (cut_c [(31, -1)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - None to None`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = input
    let actual = Utility.AsSingleton (cut_c [(0, -1)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - 0 to 500`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = input
    let actual = Utility.AsSingleton (cut_c [(0, 500)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - -5 to -1`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = "y dog"
    let actual = Utility.AsSingleton (cut_c [(-5, -1)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_c - 500 to 505`` () =
    let input = "Quick brown fox jumps over the lazy dog"
    let expect = ""
    let actual = Utility.AsSingleton (cut_c [(500, 505)]) input
    Assert.Equal(expect, actual);

[<Fact>]
let ``Test Cut_x`` () =
    let expect = [
        [ "Col   1"; "Col   2"; "Col   3" ]
        [ "Val 1-1"; "Val 2-1"; "Val 3-1" ]
        [ "Val 1-2"; "Val 2-2"; "Val 3-2" ]
        [ "Val 1-3"; "Val 2-3"; "Val 3-3" ]
    ]
    let actual =
        expect
        |> Seq.map (fun row -> row |> String.concat ",")
        |> cut_x

    Assert.True(
        Seq.zip expect actual
        |> Seq.forall (fun (expect_list, actual_list) -> expect_list.SequenceEqual(actual_list))
    )

[<Fact>]
let ``Test Cut_x with quotes`` () =
    let expect = [
        [ "Col   1"; "Col   2"; "Col   3" ]
        [ "Val 1-1"; "Val 2-1"; "Val 3-1" ]
        [ "Val 1-2"; "Val 2-2"; "Val 3-2" ]
        [ "Val 1-3"; "Val 2-3"; "Val 3-3" ]
    ]
    let actual =
        expect
        |> Seq.map (fun row -> row |> Seq.map (fun col -> $@"""{col}""") |> String.concat ",")
        |> cut_x

    Assert.True(
        Seq.zip expect actual
        |> Seq.forall (fun (expect_list, actual_list) -> expect_list.SequenceEqual(actual_list))
    )

[<Fact>]
let ``Test Cut_x with quotes and commas`` () =
    let expect = [
        [ "Col   1"; "Col   2"; "Col   3" ]
        [ "Val,1-1"; "Val 2-1"; "Val 3-1" ]
        [ ",Val 1-2"; "Val 2-2"; "Val 3-2" ]
        [ "Val 1-3,"; "Val 2-3"; "Val 3-3" ]
    ]
    let actual =
        expect
        |> Seq.map (fun row -> row |> Seq.map (fun col -> $@"""{col}""") |> String.concat ",")
        |> cut_x

    Assert.True(
        Seq.zip expect actual
        |> Seq.forall (fun (expect_list, actual_list) -> expect_list.SequenceEqual(actual_list))
    )

[<Fact>]
let ``Test Cut_x pcaret with inline pipe`` () =
    let expect = [ @"Test 1|"; "Test 2"; "Test 3" ]
    let actual = [ @"^Test 1|^|Test 2|Test 3" ] |> cut_x |> Seq.head |> Seq.toList

    Assert.True(
        Seq.zip expect actual |> Seq.forall (fun (expect, actual) -> expect = actual)
    )