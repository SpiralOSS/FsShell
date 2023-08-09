module SpiralOSS.FsShell.Testing.Command.CatTests

open Xunit
open System.Linq
open SpiralOSS.FsShell.Testing.Infrastructure
open SpiralOSS.FsShell.Command.Cat

[<Fact>]
let ``Test Cat 1 File`` () =
    use tempFile = new Utility.AutoDeletingTempFile ()
    tempFile.write Utility.sampleTextSeq
    let expect = Utility.sampleTextSeq
    let actual = cat System.Text.Encoding.UTF8 [ tempFile.filename ]
    Assert.True(expect.SequenceEqual(actual));

[<Fact>]
let ``Test Cat 2 Files`` () =
    use tempFile1 = new Utility.AutoDeletingTempFile ()
    use tempFile2 = new Utility.AutoDeletingTempFile ()
    tempFile1.write Utility.sampleTextSeq
    tempFile2.write Utility.sampleTextSeq
    let expect = seq { yield! Utility.sampleTextSeq; yield! Utility.sampleTextSeq }
    let actual = cat System.Text.Encoding.UTF8 [ tempFile1.filename; tempFile2.filename ]
    Assert.True(expect.SequenceEqual(actual));