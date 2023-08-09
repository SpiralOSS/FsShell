module Seq

let skipSafe (count:int) (source:seq<'a>) : seq<'a> =
    if count < 1 || source = null then
        Seq.empty
    else
        seq {
            use enum = source.GetEnumerator()
            let mutable counter = 0
            while counter < count && enum.MoveNext() do
                counter <- counter + 1

            while enum.MoveNext() do
                yield enum.Current
        }

let takeSafe (count:int) (source:seq<'a>) : seq<'a> =
    if count < 1 || source = null then
        Seq.empty
    else
        seq {
            use enum = source.GetEnumerator ()
            let mutable outputted = 0
            while outputted < count && enum.MoveNext () do
                yield enum.Current
                outputted <- outputted + 1
        }
