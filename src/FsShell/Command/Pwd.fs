module SpiralOSS.FsShell.Command.Pwd

let inline pwd () = System.IO.Directory.GetCurrentDirectory ()