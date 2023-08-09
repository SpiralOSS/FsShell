module SpiralOSS.FsShell.Testing.Command.MoveTests

open Xunit
open System.IO
open SpiralOSS.FsShell.Testing.Infrastructure
open SpiralOSS.FsShell.Command.Move

[<Fact>]
let ``Test Move`` () =
    let srcFileName = Utility.GetTempFileName ()
    let tgtFileName = Utility.GetTempFileName ()
    File.WriteAllText (srcFileName, "A file to move")
    let srcFileSize = (FileInfo (srcFileName)).Length
    move srcFileName tgtFileName

    // CHECK MOVED
    Assert.False(File.Exists (srcFileName))
    Assert.True(File.Exists (tgtFileName))

    // CHECK SIZE
    let tgtFileInfo = FileInfo (tgtFileName)
    Assert.Equal(srcFileSize, tgtFileInfo.Length)
