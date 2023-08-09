module SpiralOSS.FsShell.Testing.Command.OutputTests

open Xunit
open System.IO
open System.Text
open SpiralOSS.FsShell.Testing.Infrastructure
open SpiralOSS.FsShell.Command.Output

[<Fact>]
let ``Test Write`` () =
    let filename = Utility.GetTempFileName ()
    let expect = seq {yield! Utility.sampleTextSeq; yield "" } |> String.concat System.Environment.NewLine
    write Encoding.UTF8 false filename Utility.sampleTextSeq
    let actual = File.ReadAllText (filename)
    Assert.Equal(actual, expect)
