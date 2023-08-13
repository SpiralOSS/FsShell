module FsShell
type IFsShell = interface end  // used for getting the manual

open System.Text
open SpiralOSS.FsShell
open SpiralOSS.FsShell.Infrastructure

let mutable defaultEncoding = Encoding.UTF8

[<ManualEntry([|"cd";"chdir"|],"System","Change directory","")>]
let inline cd (path:string) = Command.Cd.cd path

[<ManualEntry([|"pwd"|],"System","Print working directory","")>]
let inline pwd () = Command.Pwd.pwd ()

[<ManualEntry([|"mkdir";"md"|],"System","Make directory","")>]
let inline mkdir (path:string) = Command.Mkdir.mkdir path

[<ManualEntry([|"mv";"move"|],"System","Move file","")>]
let inline mv (sourcePath:string) (targetPath:string) = Command.Move.move sourcePath targetPath

[<ManualEntry([|"cp";"copy"|],"System","Copy file","")>]
let inline cp (sourcePath:string) (targetPath:string) = Command.Copy.copy sourcePath targetPath


[<ManualEntry([|"ls";"ll"|],"System","List","")>]
let inline ls (path:string) (searchPattern:string) = Command.List.list path searchPattern

[<ManualEntry([|"ls";"ll"|],"System","List with details","")>]
let inline ll (path:string) (searchPattern:string) = Command.List.listLong path searchPattern


[<ManualEntry([|"find"|],"System","Find files","")>]
let inline find (startPath:string) (searchPattern:string) = Command.Find.find startPath searchPattern

[<ManualEntry([|"find"|],"System","Find folders","")>]
let inline find_p (startPath:string) (searchPattern:string) = Command.Find.findPaths startPath searchPattern


[<ManualEntry([|"stdout";"std";"out"|],"Output","Output to STDOUT","")>]
let inline stdout (contents:string seq) = Command.Output.stdout contents

[<ManualEntry([|"stderr";"std";"err"|],"Output","Output to STDERR","")>]
let inline stderr (contents:string seq) = Command.Output.stderr contents

[<ManualEntry([|"write";"append"|],"Output","Output to a file","")>]
let inline write (path:string) (contents:string seq) = Command.Output.write defaultEncoding false path contents

[<ManualEntry([|"write";"append"|],"Output","Append to a file","")>]
let inline append (path:string) (contents:string seq) = Command.Output.write defaultEncoding true path contents

[<ManualEntry([|"tee"|],"Output","Output to a file and passthrough","")>]
let inline tee (path:string) (contents:string seq) = Command.Tee.Tee defaultEncoding false path contents

[<ManualEntry([|"tee"|],"Output","Append to a file and passthrough","")>]
let inline tee_a (path:string) (contents:string seq) = Command.Tee.Tee defaultEncoding true path contents

[<ManualEntry([|"cat"|],"Output","Read a list of files consecutively","")>]
let inline cat (paths:string seq) = Command.Cat.cat defaultEncoding paths


[<ManualEntry([|"cut"|],"Data Manipulation","Split lines at tabs","")>]
let inline cut (contents:string seq) = Command.Cut.cut_d (Seq.singleton '\t') contents

[<ManualEntry([|"cut"|],"Data Manipulation","Split lines at delimeter","")>]
let inline cut_d (delimiter:char) (contents:string seq) = Command.Cut.cut_d (Seq.singleton delimiter) contents

[<ManualEntry([|"cut"|],"Data Manipulation","Cut character range options","")>]
let inline cut_c (ranges:(int option*int option) list) (contents:string seq) = Command.Cut.cut_c ranges contents

[<ManualEntry([|"cut"|],"Data Manipulation","Cut character ranges","")>]
let inline cut_c2 (ranges:(int*int) list) (contents:string seq) = cut_c (ranges |> List.map (fun (aa, bb) -> (Some aa, Some bb))) contents

[<ManualEntry([|"cut"|],"Data Manipulation","Splits data file into columns","Will autodetect CSV, PCARET, CPIPE, and CONCORDANCE")>]
let inline cutx (contents:string seq) = Command.Cut.cutx contents


[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to include","")>]
let inline grep (pattern:string) (contents:string seq) = Command.Grep.grep false false pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to include, case insensitive","")>]
let inline grep_i (pattern:string) (contents:string seq) = Command.Grep.grep true false pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to include with regex","")>]
let inline egrep (pattern:string) (contents:string seq) = Command.Grep.egrep false false pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to include with regex, case insensitive","")>]
let inline egrep_i (pattern:string) (contents:string seq) = Command.Grep.egrep true false pattern contents


[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to exclude","")>]
let inline grep_n (pattern:string) (contents:string seq) = Command.Grep.grep false true pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to exclude, case insensitive","")>]
let inline grep_in (pattern:string) (contents:string seq) = Command.Grep.grep true true pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to exclude with regex","")>]
let inline egrep_n (pattern:string) (contents:string seq) = Command.Grep.egrep false true pattern contents

[<ManualEntry([|"grep";"egrep"|],"Data Flow","Filter lines to exclude with regex, case insensitive","")>]
let inline egrep_in (pattern:string) (contents:string seq) = Command.Grep.egrep true false pattern contents


[<ManualEntry([|"head"|],"Data Flow","First count lines","")>]
let inline head_n (count:int) (contents:'a seq) = Command.Head.head count contents

[<ManualEntry([|"head"|],"Data Flow","First 10 lines","")>]
let inline head (contents:'a seq) = head_n 10 contents


[<ManualEntry([|"tail"|],"Data Flow","Last count lines, or skip first count of lines","")>]
let inline tail_n (count:int) (contents:'a seq) = Command.Tail.tail count contents

[<ManualEntry([|"tail"|],"Data Flow","Last 10 lines","")>]
let inline tail (contents:'a seq) = tail_n 10 contents


let private manualEntries = ManualEntryAttribute.getManualEntriesFromType typeof<IFsShell>
[<ManualEntry([|"man";"manual"|],"Miscellaneous","This","")>]
let man (funcName:string) =
    if funcName = "" then
        manualEntries
        |> Seq.groupBy (fun manPage -> manPage.Group)
        |> Seq.iter (fun (manPageGroupName, manPageGroup) ->
            printfn "%s" manPageGroupName
            manPageGroup
            |> Seq.iter (fun manPage ->
                printfn $"  %-8s{manPage.MethodName}  %s{manPage.ShortDescription}"
            )
        )
    else
        let displayEntries =
            manualEntries
            |> List.filter (fun manEntry -> manEntry.MethodName = funcName || (List.contains funcName manEntry.Tags))
        if displayEntries.Length = 0 then
            printfn @"Command not found. Use `man "";;` to see all commands."
        else
            displayEntries
            |> Seq.sortBy (fun manPage -> manPage.MethodName)
            |> Seq.iter (fun manPage ->
                printfn $"{manPage.MethodName}"
                printfn $"  {manPage.MethodSignature}"
                printfn $"  {manPage.ShortDescription}"
                printfn $"  {manPage.LongDescription}"
                printfn ""
            )