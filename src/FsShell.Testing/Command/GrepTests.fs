module SpiralOSS.FsShell.Testing.Command.GrepTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open SpiralOSS.FsShell.Command.Grep

[<Fact>]
let ``Test Grep`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 1
    let actual = grep false false "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Grep Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = grep true false "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Grep Inverse Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 3
    let actual = grep false true "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Grep Inverse and Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = grep true true "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep false false "ve.*e" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep true false "VE.*E" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep Inverse`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep false true "ve.*e" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep Inverse and Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep true true "VE.*E" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XGrep`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "Find Me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "Find Me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
    }
    let expect = 2
    let actual = xgrep' false false "Find Me" ([ (1, 2) ]) input |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XGrep 2`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "Find Me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "Find Me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
    }
    let expect = 2
    let actual = xgrep false false "Find Me" ([ (Some 1, None) ]) input |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XGrep Inverse`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "Find Me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "Find Me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
        [| "Do Not Find Me"; "Val 2-4"; "Val 3-4" |]
    }
    let expect = 3
    let actual = xgrep false true "Find Me" ([ (Some 1, None) ]) input |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XGrep Ignore Case`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "find me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "find me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
    }
    let expect = 2
    let actual = xgrep true false "Find Me" ([ (Some 1, None) ]) input |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XEGrep`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "Find Me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "Find Me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
    }
    let expect = 2
    let actual = xegrep' false false "^Find Me$" ([ (0, 1); (2, 2) ]) input |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test XEGrep Inverse`` () =
    let input = seq {
        [| "Col   1"       ; "Col   2"; "Col   3" |]
        [| "Val 1-1"       ; "Find Me"; "Val 3-1" |]
        [| "Val 1-2"       ; "Val 2-2"; "Find Me" |]
        [| "Do Not Find Me"; "Val 2-3"; "Val 3-3" |]
    }
    let expect = 2
    let actual = xegrep' false false "^Find Me$" ([ (0, 1); (2, 2) ]) input |> Seq.length
    Assert.Equal(expect, actual)