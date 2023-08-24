module SpiralOSS.FsShell.Command.Grep

open System.Text.RegularExpressions
open SpiralOSS.FsShell.Infrastructure.Utility

let grep (ignoreCase:bool) (inverse:bool) (pattern:string) (contents:string seq) =
    let filterMethod =
        match (ignoreCase, inverse) with
        | (false, false) -> fun (content:string) -> content.Contains(pattern)
        | (false, true)  -> fun (content:string) -> not (content.Contains(pattern))
        | (true, false)  -> fun (content:string) -> content.Contains(pattern, System.StringComparison.CurrentCultureIgnoreCase)
        | (true, true)   -> fun (content:string) -> not (content.Contains(pattern, System.StringComparison.CurrentCultureIgnoreCase))
    Seq.filter filterMethod contents

let egrep (ignoreCase:bool) (inverse:bool) (pattern:string) (contents:string seq) =
    let compiledRegex =
        match ignoreCase with
        | false -> Regex(pattern, RegexOptions.Compiled)
        | true  -> Regex(pattern, RegexOptions.Compiled + RegexOptions.IgnoreCase)
    let filterMethod =
        match inverse with
        | false -> fun (content:string) -> compiledRegex.IsMatch(content)
        | true  -> fun (content:string) -> not (compiledRegex.IsMatch(content))
    Seq.filter filterMethod contents

let xgrep (ignoreCase:bool) (inverse:bool) (pattern:string) (ranges:(int option*int option) list) (contents:string[] seq) =
    let filterMethod =
        let containsPattern (ignoreCase:bool) (value:string) = value.Contains(pattern, (if ignoreCase then System.StringComparison.CurrentCultureIgnoreCase else System.StringComparison.CurrentCulture))
        match (ignoreCase, inverse) with
        | (false, false) -> fun (content:string[]) -> content |> rangeSplice ranges |> List.exists (containsPattern false)
        | (false, true)  -> fun (content:string[]) -> content |> rangeSplice ranges |> List.filter (containsPattern false) |> List.isEmpty
        | (true, false)  -> fun (content:string[]) -> content |> rangeSplice ranges |> List.exists (containsPattern true)
        | (true, true)   -> fun (content:string[]) -> content |> rangeSplice ranges |> List.filter (containsPattern true) |> List.isEmpty
    Seq.filter filterMethod contents
let xgrep' (ignoreCase:bool) (inverse:bool) (pattern:string) (ranges:(int*int) list) (contents:string[] seq) =
    xgrep ignoreCase inverse pattern (ranges |> List.map (fun (ss,ee) -> (Some ss, Some ee))) contents

let xegrep (ignoreCase:bool) (inverse:bool) (pattern:string) (ranges:(int option*int option) list) (contents:string[] seq) =
    let compiledRegex =
        match ignoreCase with
        | false -> Regex(pattern, RegexOptions.Compiled)
        | true  -> Regex(pattern, RegexOptions.Compiled + RegexOptions.IgnoreCase)
    let filterMethod =
        let isMatch (value:string) = compiledRegex.IsMatch(value)
        match inverse with
        | false -> fun (content:string[]) -> content |> rangeSplice ranges |> List.exists isMatch
        | true  -> fun (content:string[]) -> content |> rangeSplice ranges |> List.filter isMatch |> List.isEmpty
    Seq.filter filterMethod contents
let xegrep' (ignoreCase:bool) (inverse:bool) (pattern:string) (ranges:(int*int) list) (contents:string[] seq) =
    xegrep ignoreCase inverse pattern (ranges |> List.map (fun (ss,ee) -> (Some ss, Some ee))) contents
