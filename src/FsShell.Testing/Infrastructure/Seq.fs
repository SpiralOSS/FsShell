module SpiralOSS.FsShell.Testing.Infrastructure.Seq

open Xunit
open System.Linq

[<Fact>]
let ``Test iterTee`` () =
    let input = seq { 1; 2; 3 }
    let expect = seq { 1; 2; 3 }
    let actual = Seq.iterTee (fun _ -> ()) input
    Assert.True(expect.SequenceEqual(actual))

[<Fact>]
let ``Test iterTee2`` () =
    let input = seq { 1; 2; 3 }
    let expect = Some 2
    let mutable actual = None
    input
    |> Seq.iterTee (fun item -> actual <- (if (actual.IsNone && item = 2) then Some item else actual) )
    |> Seq.iter (fun _ -> ())
    Assert.Equal(expect, actual)
