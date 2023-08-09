module SpiralOSS.FsShell.Testing.Command.MkdirTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open SpiralOSS.FsShell.Command.Mkdir

[<Fact>]
let ``Test Mkdir`` () =
    let tempPath = Path.Combine (Path.GetTempPath (), Path.GetFileNameWithoutExtension (Utility.GetTempFileName ()))
    let actual = mkdir tempPath
    Assert.Equal(tempPath, actual)
    Assert.True(Directory.Exists (tempPath))
    Directory.Delete (tempPath, true)
