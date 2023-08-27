module SpiralOSS.FsShell.Testing.Infrastructure.UtilityTests

open Xunit
open System.Linq
open SpiralOSS.FsShell.Infrastructure.Utility

let rangesToCursorsTests = seq {
    [| 5 :> obj; [ ( 0,  5) ] :> obj; [ (Within 0, Within 5) ] :> obj |]
    [| 5 :> obj; [ ( 0,  6) ] :> obj; [ (Within 0, After   ) ] :> obj |]
    [| 5 :> obj; [ ( 0,  3) ] :> obj; [ (Within 0, Within 3) ] :> obj |]
    [| 5 :> obj; [ ( 0, -1) ] :> obj; [ (Within 0, Within 5) ] :> obj |]
    [| 5 :> obj; [ ( 0, -5) ] :> obj; [ (Within 0, Within 1) ] :> obj |]
    [| 5 :> obj; [ ( 0, -6) ] :> obj; [ (Within 0, Within 0) ] :> obj |]
    [| 5 :> obj; [ ( 0, -6) ] :> obj; [ (Within 0, Within 0) ] :> obj |]
    [| 5 :> obj; [ (-1, -6) ] :> obj; [ (Within 5, Within 5) ] :> obj |]
    [| 5 :> obj; [ (-3, -1) ] :> obj; [ (Within 3, Within 5) ] :> obj |]
    [| 5 :> obj; [ (-7, -1) ] :> obj; [ (Before,   Within 5) ] :> obj |]
    [| 5 :> obj; [ (-7,  6) ] :> obj; [ (Before,   After   ) ] :> obj |]
    }

[<Theory>]
[<MemberData(nameof(rangesToCursorsTests))>]
let ``Test rangesToCursors`` inputLastPosition inputRanges (expect:((CursorPosition*CursorPosition) list)) =
    let actual = rangesToCursors inputLastPosition inputRanges
    Assert.True(expect.SequenceEqual(actual))

let rangeSpliceTests = seq {
    [| [ ( 0,  6) ] :> obj; "0123456789" :> obj; "0123456" :> obj  |]
    [| [ ( 0, -1) ] :> obj; "0123456789" :> obj; "0123456789" :> obj  |]
    [| [ ( 0, -2) ] :> obj; "0123456789" :> obj; "012345678" :> obj  |]
    [| [ ( 5,  0) ] :> obj; "0123456789" :> obj; "5" :> obj  |]
    [| [ ( 5,100) ] :> obj; "0123456789" :> obj; "56789" :> obj  |]
    [| [ ( 1,  3); ( 5, 6); (-1, -1) ] :> obj; "0123456789" :> obj; "123569" :> obj  |]
    }

[<Theory>]
[<MemberData(nameof(rangeSpliceTests))>]
let ``Test stringRangeSplice`` (ranges:(int*int) list) (input:string) (expect:string) =
    let actual =
        stringRangeSplice' ranges input
        |> String.concat ""
    Assert.Equal(expect, actual)

[<Theory>]
[<MemberData(nameof(rangeSpliceTests))>]
let ``Test arrayRangeSpliceTests`` (ranges:(int*int) list) (input:string) (expect:string) =
    let inputArr = input.ToCharArray() |> Array.map string
    let actual =
        arrayRangeSplice' ranges inputArr
        |> Array.concat
        |> String.concat ""
    Assert.Equal(expect, actual)
