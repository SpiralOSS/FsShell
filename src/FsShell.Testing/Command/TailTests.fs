module SpiralOSS.FsShell.Testing.Command.TailTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.Linq
open SpiralOSS.FsShell.Command.Tail

[<Fact>]
let ``Test Tail 0`` () =
    let expect = Seq.empty
    let actual = tail 0 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Tail 1`` () =
    let expect = seq { Utility.sampleTextSeq |> Seq.last }
    let actual = tail 1 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Tail 3`` () =
    let expect = Utility.sampleTextSeq |> Seq.rev |> Seq.take 3 |> Seq.rev
    let actual = tail 3 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Tail Too Many`` () =
    let expect = Utility.sampleTextSeq
    let actual = tail 100 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Tail Negative`` () =
    let expect = Utility.sampleTextSeq |> Seq.skip 2
    let actual = tail -2 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test Tail Skip Too Many`` () =
    let expect = Seq.empty
    let actual = tail -10 Utility.sampleTextSeq
    Assert.True(expect.SequenceEqual(actual))

