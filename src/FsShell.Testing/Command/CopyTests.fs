module SpiralOSS.FsShell.Testing.Command.CopyTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open SpiralOSS.FsShell.Command.Copy

[<Fact>]
let ``Test Copy`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let tgtFileName = Utility.GetTempFileName ()
    copy srcFile.filename tgtFileName
    Assert.True(File.Exists (srcFile.filename))
    Assert.True(File.Exists (tgtFileName))

    // CHECK SIZE
    let srcFileInfo = FileInfo (srcFile.filename)
    let tgtFileInfo = FileInfo (tgtFileName)
    Assert.Equal(srcFileInfo.Length, tgtFileInfo.Length)
    tgtFileInfo.Delete ()
