namespace SpiralOSS.FsShell.Infrastructure

open System
open System.Reflection
open Microsoft.FSharp.Reflection

type ManualEntryAttribute (tags:string[],group:string,shortDescription:string,longDescription:string) =
    inherit Attribute()
    member val Tags = tags |> Seq.toList with get
    member val ShortDescription = shortDescription with get
    member val LongDescription = longDescription with get
    member val Group = group with get
    member val MethodName = "" with get,set
    member val MethodSignature = "" with get,set
    member val Method = null with get,set

    static member private getMethodSignature (method:MethodInfo) =
        let mutable signature = $"{method.Name}: "
        let parms = method.GetParameters()
        if parms.Length = 0 then
            signature <- "unit -> "
        else
            for parm in parms do
                signature <- signature + $"{parm.Name}:{parm.ParameterType.Name} -> "
        let returnParam = method.ReturnParameter.ToString()
        let returnParam = if returnParam = "Void" then "unit" else returnParam
        signature <- signature + returnParam
        signature

    static member getManualEntriesFromType (moduleType:System.Type) =
        [
            for method in (moduleType.DeclaringType.GetMethods ()) do
            for attribute in System.Attribute.GetCustomAttributes (method) do
                match attribute with
                | :? ManualEntryAttribute as manualEntry ->
                    manualEntry.MethodName <- method.Name
                    manualEntry.MethodSignature <- (ManualEntryAttribute.getMethodSignature method)
                    manualEntry.Method <- method
                    yield (manualEntry)
                | it -> ignore it
        ]

    static member getFuncs (fsiAssembly:System.Reflection.Assembly) =
        // Code taken from ionide to help understand parameters and make a prettier signature
        fsiAssembly.GetTypes()
        //|> Seq.filter (fun ty -> ty.FullName.StartsWith("FSI"))
        |> Seq.filter (FSharpType.IsModule)
        |> Seq.choose (fun ty ->
            let methods =
                ty.GetMethods()
                |> Seq.filter (fun m ->
                    m.IsStatic
                    && not (Seq.isEmpty (m.GetParameters()))
                    && m.Name <> "set_it")
                |> Seq.map (fun m ->
                    let parms =
                        m.GetParameters()
                        |> Seq.map (fun p ->
                            p.Name,
                            if p.ParameterType.IsGenericParameter then
                                "'" + p.ParameterType.Name
                            else
                                p.ParameterType.Name)

                    m.Name, parms, m.ReturnType.Name)

            if Seq.isEmpty methods then
                None
            else
                Some(methods))
        |> Seq.collect id
