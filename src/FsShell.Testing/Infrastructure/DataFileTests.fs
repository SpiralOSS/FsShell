module SpiralOSS.FsShell.Testing.Infrastructure.DataFileTests

open Xunit
open SpiralOSS.FsShell.Infrastructure

[<Fact>]
let ``Test ReadLine with single column and quotes`` () =
    let input = @"""Column Value"""
    let expected = ["Column Value"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with single column without quotes`` () =
    let input = @"Column Value"
    let expected = ["Column Value"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns without quotes`` () =
    let input = @"Column Value 1,Column Value 2,Column Value 3"
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns with quotes`` () =
    let input = @"""Column Value 1"",""Column Value 2"",""Column Value 3"""
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns with mixed quotes 1`` () =
    let input = @"""Column Value 1"",Column Value 2,""Column Value 3"""
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns with mixed quotes 2`` () =
    let input = @"Column Value 1,""Column Value 2"",""Column Value 3"""
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns with mixed quotes 3`` () =
    let input = @"Column Value 1,Column Value 2,""Column Value 3"""
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)

[<Fact>]
let ``Test ReadLine with multiple columns with mixed quotes 4`` () =
    let input = @"""Column Value 1"",Column Value 2,Column Value 3"
    let expected = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataFile.ReadLine (',', '"') input
    Assert.Equal(expected, actual)