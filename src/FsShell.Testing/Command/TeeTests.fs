module SpiralOSS.FsShell.Testing.Command.TeeTests

open Xunit
open System.Text
open System.IO
open System.Linq
open SpiralOSS.FsShell.Testing.Infrastructure
open SpiralOSS.FsShell.Command.Tee

[<Fact>]
let ``Test Tee`` () =
    let expect = Utility.sampleTextSeq
    let filename = Utility.GetTempFileName ()
    let actual = Tee Encoding.UTF8 false filename Utility.sampleTextSeq
    // INPUT = OUTPUT
    Assert.True (actual.SequenceEqual(expect))

    // INPUT = TEXT OUTPUT
    //  need an extra newline on the expect
    let expect = seq { yield! expect; yield "" } |> String.concat System.Environment.NewLine
    let actual = File.ReadAllText(filename)
    Assert.True (actual.SequenceEqual(expect))
