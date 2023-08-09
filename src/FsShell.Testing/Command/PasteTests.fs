module SpiralOSS.FsShell.Testing.Command.PasteTests

open Xunit
open System.Linq
open System.Text
open SpiralOSS.FsShell.Testing.Infrastructure
open SpiralOSS.FsShell.Command.Paste

[<Fact>]
let ``Test Paste`` () =
    use tempFile1 = new Utility.AutoDeletingTempFile ()
    use tempFile2 = new Utility.AutoDeletingTempFile ()
    tempFile1.write Utility.sampleTextSeq
    tempFile2.write Utility.sampleTextSeq
    let expect = seq { yield! Utility.sampleTextSeq; yield! Utility.sampleTextSeq }
    let actual = Paste Encoding.UTF8 "\t" [ tempFile1.filename; tempFile2.filename ]
    Assert.True(expect.SequenceEqual(actual));