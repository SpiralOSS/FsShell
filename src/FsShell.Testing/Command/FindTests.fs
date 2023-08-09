module SpiralOSS.FsShell.Testing.Command.FindTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open System.Linq
open SpiralOSS.FsShell.Command.Find

let createTempFolderTree () =
    let tempPath = Path.Combine (Path.GetTempPath (), Path.GetFileNameWithoutExtension (Utility.GetTempFileName ()))
    [ "dat";"txt";"xlsx" ]
    |> Seq.iteri (fun indx ext ->
        let subfolder = Path.Combine (tempPath, indx.ToString())
        Directory.CreateDirectory (subfolder) |> ignore
        File.WriteAllText (Path.Combine (subfolder, $"{indx}.{ext}"), ext)
    )
    tempPath

[<Fact>]
let ``Test Find *`` () =
    let tempTreePath = createTempFolderTree ()
    let expect = [
        Path.Combine (tempTreePath, "0", "0.dat")
        Path.Combine (tempTreePath, "1", "1.txt")
        Path.Combine (tempTreePath, "2", "2.xlsx")
    ]
    let actual = find tempTreePath "*"
    Assert.True(expect.SequenceEqual(actual));
    Directory.Delete (tempTreePath, recursive=true)

[<Fact>]
let ``Test Find *.txt`` () =
    let tempTreePath = createTempFolderTree ()
    let expect = [
        Path.Combine (tempTreePath, "1", "1.txt")
    ]
    let actual = find tempTreePath "*.txt"
    Assert.True(expect.SequenceEqual(actual));
    Directory.Delete (tempTreePath, recursive=true)

[<Fact>]
let ``Test FindPaths`` () =
    let tempTreePath = createTempFolderTree ()
    let expect = [
        Path.Combine (tempTreePath, "0")
        Path.Combine (tempTreePath, "1")
        Path.Combine (tempTreePath, "2")
    ]
    let actual = findPaths tempTreePath "*"
    Assert.True(expect.SequenceEqual(actual));
    Directory.Delete (tempTreePath, recursive=true)