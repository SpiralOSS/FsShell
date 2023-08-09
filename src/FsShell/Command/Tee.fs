module SpiralOSS.FsShell.Command.Tee

open System.IO
open System.Text

let Tee (encoding:Encoding) (append:bool) (path:string) (contents:string seq) =
    seq {
        use sw = new StreamWriter (path, append, encoding)
        for content in contents do
            sw.WriteLine content
            yield content
    }