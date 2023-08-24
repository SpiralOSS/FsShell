module SpiralOSS.FsShell.Testing.Command.JoinTests

open Xunit
open SpiralOSS.FsShell.Command.Join

[<Fact>]
let ``Test XJoin`` () =
    let input  = seq {
        [| "A"; "B" |]
        }
    let expect = seq { @"'A','B'" }
    let actual = xjoin (',','\'') input
    Assert.Equal(expect |> Seq.item 0, actual |> Seq.item 0)