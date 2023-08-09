module SpiralOSS.FsShell.Command.Grep

open System.Text.RegularExpressions

let grep (ignoreCase:bool) (inverse:bool) (pattern:string) (contents:string seq) =
    let filterMethod =
        match (ignoreCase, inverse) with
        | (false, false) -> fun (content:string) -> content.Contains(pattern)
        | (false, true)  -> fun (content:string) -> not (content.Contains(pattern))
        | (true, false)  -> fun (content:string) -> content.Contains(pattern, System.StringComparison.CurrentCultureIgnoreCase)
        | (true, true)   -> fun (content:string) -> not (content.Contains(pattern, System.StringComparison.CurrentCultureIgnoreCase))
    Seq.filter filterMethod contents

let egrep (ignoreCase:bool) (inverse:bool)  (pattern:string) (contents:string seq) =
    let compiledRegex =
        match ignoreCase with
        | false -> Regex(pattern, RegexOptions.Compiled)
        | true -> Regex(pattern, RegexOptions.Compiled + RegexOptions.IgnoreCase)
    let filterMethod =
        match inverse with
        | false -> fun (content:string) -> compiledRegex.IsMatch(content)
        | true  -> fun (content:string) -> not (compiledRegex.IsMatch(content))
    Seq.filter filterMethod contents
