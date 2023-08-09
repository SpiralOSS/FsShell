module SpiralOSS.FsShell.Testing.Command.CdTests

open Xunit
open System.IO
open SpiralOSS.FsShell.Command.Cd

[<Fact>]
let ``Test Cd`` () =
    let expected = Path.GetDirectoryName (Directory.GetCurrentDirectory ())
    let actual = cd ".."
    Assert.Equal(expected, actual)
