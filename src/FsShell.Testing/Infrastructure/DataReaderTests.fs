module SpiralOSS.FsShell.Testing.Infrastructure.DataFileTests

open Xunit
open SpiralOSS.FsShell.Infrastructure

[<Fact>]
let ``Test ReadDataLine with single column and quotes`` () =
    let input = @"""Column Value"""
    let expect = ["Column Value"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with single column without quotes`` () =
    let input = @"Column Value"
    let expect = ["Column Value"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns without quotes`` () =
    let input = @"Column Value 1,Column Value 2,Column Value 3"
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns with quotes`` () =
    let input = @"""Column Value 1"",""Column Value 2"",""Column Value 3"""
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns with mixed quotes 1`` () =
    let input = @"""Column Value 1"",Column Value 2,""Column Value 3"""
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns with mixed quotes 2`` () =
    let input = @"Column Value 1,""Column Value 2"",""Column Value 3"""
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns with mixed quotes 3`` () =
    let input = @"Column Value 1,Column Value 2,""Column Value 3"""
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)

[<Fact>]
let ``Test ReadDataLine with multiple columns with mixed quotes 4`` () =
    let input = @"""Column Value 1"",Column Value 2,Column Value 3"
    let expect = ["Column Value 1";"Column Value 2";"Column Value 3"]
    let actual = DataReader.readDataLine (',', '"') input
    Assert.Equal(expect, actual)


let samplesDetermineSeparatorAndQuantifier : obj[] list =
    [
        [| Some (',','"'); @"""column1"",""column2""" |]
        [| Some (',','"'); @"column1,," |]
        [| Some (',','"'); @"column1,column2,column3,," |]
        [| Some (',','"'); @",,,," |]
        [| Some (',','"'); @",,,,""column5""" |]
        [| Some (',','"'); @"""""" |]
        [| Some ('|','^'); @"^column1^|^column2^" |]
        [| Some ('|','^'); @"column1|column2" |]
        [| None; @"" |]
        [| Some ('','þ'); @"þcolumn1þþcolumn2þþcolumn3þ" |]
    ]

[<Theory>]
[<MemberData(nameof(samplesDetermineSeparatorAndQuantifier))>]
let ``Test determineSeparatorAndQuantifier`` expect input =
    let actual = DataReader.determineSpAndQt input
    Assert.Equal(expect, actual)
