﻿module Common


module Debug =

    let logFile = !Options.debugDir + "debug.log"

    /// run a function if in debug mode
    let debug n f =
        if !Options.debug >= n then
            f ()

    let debug0 f = debug 0 f
    let debug1 f = debug 1 f
    let debug2 f = debug 2 f
    let debug3 f = debug 3 f

    /// Log information to a file
    let logInfo n str = 
        let indent = String.replicate n "\t"
        if !Options.debug >= n then
            System.IO.File.AppendAllText(logFile, indent + str + "\n")

    let logInfo0 f = logInfo 0 f
    let logInfo1 f = logInfo 1 f
    let logInfo2 f = logInfo 2 f
    let logInfo3 f = logInfo 3 f

    /// Generate a string header for a debugging section
    let logHeader n x = 
        let indent = String.replicate n "\t"
        let len = String.length x
        let y = String.replicate (len + 2) "="
        let x = indent + x
        let y = indent + y
        if !Options.debug >= n then
            System.IO.File.AppendAllText(logFile, y + "\n" + x + "\n" + y + "\n")

    let logHeader0 x = logHeader 0 x
    let logHeader1 x = logHeader 1 x 
    let logHeader2 x = logHeader 2 x 
    let logHeader3 x = logHeader 3 x 


module List = 

    /// A faster, inline version of fold
    let inline fold f b ls = 
        let mutable acc = b 
        for i in ls do 
            acc <- f acc i 
        acc

    /// List fold without a required base element
    let inline fold1 f ls = 
        match ls with 
        | [] -> failwith "empty list in fold1"
        | hd::tl -> 
            let mutable acc = hd 
            for i in tl do 
                acc <- f acc i 
            acc

    /// Easier custom printing by joining a list of strings with a separator
    let inline joinBy sep ss = 
        fold1 (fun a b -> a + sep + b) ss

    /// Enumerate all length-n combinations of a list
    let combinations n ls = 
        let rec aux acc size set = seq {
            match size, set with 
            | n, x::xs -> 
                if n > 0 then yield! aux (x::acc) (n - 1) xs
                if n >= 0 then yield! aux acc n xs 
            | 0, [] -> yield acc 
            | _, [] -> () }
        aux [] n ls


module Error =

    type Result<'a, 'b> = 
        | Ok of 'a
        | Err of 'b

    let isOk res = 
        match res with 
        | Ok _ -> true 
        | Err _ -> false 

    let isErr res =
        match res with 
        | Ok _ -> false 
        | Err _ -> true

    let unwrap res = 
        match res with 
        | Ok v -> v 
        | Err _ -> 
            failwith "unwrapping error result"

    let map f res = 
        match res with 
        | Ok v -> Ok (f v)
        | Err e -> Err e

    let andThen f res = 
        match res with 
        | Ok v -> f v 
        | Err e -> Err e
