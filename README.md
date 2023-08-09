# FsShell
List of Shell-like commands for use in F#.

## Caveat
These commands are NOT intended for projects with a long life. It is recommended that this only be used in the FSI or for short fire-and-forget utilities. As I eat my own dog food and become more proficient in idiomatic F#, some things might change.

## Usage
```F#
#r "nuget:FsShell"
open FsShell
man "";;

System
  cd        Change directory
  pwd       Print working directory
  mkdir     Make directory
  mv        Move file
  cp        Copy file
  ls        List
  ll        List with details (fullname * size * FileInfo)
  find      Find files
  find_p    Find folders
Output
  stdout    Output to STDOUT
  stderr    Output to STDERR
  write     Output to a file
  append    Append to a file
  tee       Output to a file and passthrough
  tee_a     Append to a file and passthrough
  cat       Read a list of files consecutively
Data Manipulation
  NOTE: these will create seq<string list> that aren't friendly with other commands
  cut       Split lines at tabs
  cut_d     Split lines at delimeter
  cut_c     Cut character range options
  cut_c2    Cut character ranges
  cutx      Splits CSV into columns
Data Flow
  grep      Filter lines to include
  grep_i    Filter lines to include, case insensitive
  egrep     Filter lines to include with regex
  egrep_i   Filter lines to include with regex, case insensitive
  grep_n    Filter lines to exclude
  grep_in   Filter lines to exclude, case insensitive
  egrep_n   Filter lines to exclude with regex
  egrep_in  Filter lines to exclude with regex, case insensitive
  head_n    First count lines
  head      First 10 lines
  tail_n    Last count lines, or skip first count of lines
  tail      Last 10 lines
Miscellaneous
  man       This
```

[All functions are in FsShell.fs](src/FsShell/FsShell.fs)

## Examples

### Manipulating Folders
```F#
> mkdir "test_fsshell";;
val it: string = "/Users/gregh/Projects/test_fsshell"

> cd "test_fsshell";;
val it: string = "/Users/gregh/Projects/test_fsshell"

> pwd ();;
val it: string = "/Users/gregh/Projects/test_fsshell"
```

### Manipulating Files
```F#
> // write to output lines to files
- // newlines are added and stripped automatically
- seq {
-     "My First Line"
-     "My Second Line"
- }
- |> write "file1.txt"
- ;;
val it: unit = ()

> // cat and grep
- cat [ "file1.txt" ]
- |> grep_i "second"
- |> write "file2.txt"
- ;;
val it: unit = ()

> cat [ "file1.txt"; "file2.txt" ];;
val it: seq<string> =
  seq ["My First Line"; "My Second Line"; "My Second Line"]
```

### Manipulating Data
```F#
> seq { "A\tB"; "C\tD" } |> write "file1.txt";;
val it: unit = ()

> cat [ "file1.txt" ]
- |> cut
- ;;
val it: seq<string list> = seq [["A"; "B"]; ["C"; "D"]]
> // also available is cut_d to split on a provided delimiter
- // cut_c and cut_c2 to get specific character ranges

> // cutx for handling CSV data files
- cat [ "./datafile.csv" ]
- |> cutx
- ;;
val it: seq<string array> =
  seq
    [[|"Column 1"; "Column 2"; "Column 3"|];
     [|"Val 1-1"; "Val 2-1"; "Val 3-1"|];
     [|"Val 1-2"; "Val 2-2"; "Val 3-2"|];
     [|"Val 1-3"; "Val 2-3"; "Val 3-3"|]]
```

## Miscellaneous

### "Why no rm and rmdir?"

I could see this going badly, quickly. Maybe I'll change my mind later, but after implementing it, I felt it was too dangerous.