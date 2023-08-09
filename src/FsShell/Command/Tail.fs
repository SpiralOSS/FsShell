module SpiralOSS.FsShell.Command.Tail

open System.Collections.Generic

let tail (count:int) (contents:'a seq) =
    match count with
    | _ when count < 0 ->
        contents |> Seq.skipSafe (abs count)
    | _ when count > 0 ->
        let queue = Queue<'a> (count)
        use enum = contents.GetEnumerator ()
        while enum.MoveNext () do
            if queue.Count = count then
                queue.Dequeue () |> ignore
            queue.Enqueue (enum.Current)
        seq queue
    | _ ->
        Seq.empty
