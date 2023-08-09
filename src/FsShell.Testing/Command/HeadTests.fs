module SpiralOSS.FsShell.Testing.Command.HeadTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.Linq
open SpiralOSS.FsShell.Command.Head

[<Fact>]
let ``Test Head -1`` () =
    let expect = Seq.empty
    let actual = head -1 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Head 0`` () =
    let expect = Seq.empty
    let actual = head 0 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Head 1`` () =
    let expect = Utility.sampleTextSeq |> Seq.take 1
    let actual = head 1 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Head 3`` () =
    let expect = Utility.sampleTextSeq |> Seq.take 3
    let actual = head 3 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Head Too Many`` () =
    let expect = Utility.sampleTextSeq
    let actual = head 100 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

