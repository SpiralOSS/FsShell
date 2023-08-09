module SpiralOSS.FsShell.Testing.Command.GrepTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open SpiralOSS.FsShell.Command.Grep

[<Fact>]
let ``Test Grep`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 1
    let actual = grep false false "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Grep Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = grep true false "Lo" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep false false "ve.*e" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test Egrep Ignore Case`` () =
    use srcFile = new Utility.AutoDeletingTempFile ()
    srcFile.write Utility.sampleTextSeq
    let expect = 2
    let actual = egrep true false "VE.*E" Utility.sampleTextSeq |> Seq.length
    Assert.Equal(expect, actual)

