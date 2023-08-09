module SpiralOSS.FsShell.Testing.Command.ListTests

open Xunit
open SpiralOSS.FsShell.Testing.Infrastructure
open System.IO
open System.Linq
open SpiralOSS.FsShell.Command.List

let createTempFolder () =
    let tempPath = Path.Combine (Path.GetTempPath (), Path.GetFileNameWithoutExtension (Utility.GetTempFileName ()))
    Directory.CreateDirectory (tempPath) |> ignore
    [ "dat";"txt";"xlsx" ]
    |> Seq.iteri (fun indx ext ->
        File.WriteAllText (Path.Combine (tempPath, $"{indx}.{ext}"), ext)
    )
    tempPath

[<Fact>]
let ``Test List *`` () =
    let tempFolder = createTempFolder ()
    let expect = Seq.sort [
        Path.Combine (tempFolder, "0.dat")
        Path.Combine (tempFolder, "1.txt")
        Path.Combine (tempFolder, "2.xlsx")
    ]
    let actual = list tempFolder "*" |> Seq.sort
    Assert.True(expect.SequenceEqual(actual));
    Directory.Delete (tempFolder, recursive=true)

[<Fact>]
let ``Test List *.txt`` () =
    let tempFolder = createTempFolder ()
    let expect = [
        Path.Combine (tempFolder, "1.txt")
    ]
    let actual = list tempFolder "*.txt"
    Assert.True(expect.SequenceEqual(actual));
    Directory.Delete (tempFolder, recursive=true)

[<Fact>]
let ``Test ListLong *.txt`` () =
    let tempFolder = createTempFolder ()
    let expect_filename = Path.Combine (tempFolder, "1.txt")
    let expect_filesize:int64 = 3
    let listlong = listLong tempFolder "*.txt"
    Assert.Equal((Seq.length listlong), 1)

    let (actual_filename, actual_filesize, _) = Seq.head listlong
    Assert.Equal(expect_filename, actual_filename);
    Assert.Equal(expect_filesize, actual_filesize);
    Directory.Delete (tempFolder, recursive=true)