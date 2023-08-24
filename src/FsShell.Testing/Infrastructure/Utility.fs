module SpiralOSS.FsShell.Testing.Infrastructure.Utility

open System.IO

let sampleTextSeq =
    seq {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
        "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
        "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
        "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
    }

// AS SINGLETON
//  allows us to call a function that takes a string seq and
//  run it with a single item
let inline AsSingleton func (input:string) =
    func (Seq.singleton input)
    |> Seq.exactlyOne

let GetTempFileName () =
    let randomNumber = System.Random.Shared.Next (1_000_000)
    let tempFileName = Path.GetFileNameWithoutExtension (Path.GetTempFileName ())
    Path.Combine (Path.GetTempPath(), $"{tempFileName}{randomNumber}.tmp")

type AutoDeletingTempFile() =
    let tempfilename = GetTempFileName ()
    member this.filename = tempfilename
    member this.write (contents:string seq) =
        File.WriteAllText(this.filename, contents |> String.concat System.Environment.NewLine)
    interface System.IDisposable with
        member this.Dispose() =
            try
                File.Delete(this.filename)
            with
                | it -> ignore it
