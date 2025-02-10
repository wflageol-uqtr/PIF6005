open System

type IndexedString = { string: string; mutable index : int }

let peekChar { string = string; index = index } = 
    if index < string.Length
    then Some string.[index]
    else None

let incIndex istring =
    istring.index <- istring.index + 1
        
let popChar istring =
    let c = peekChar istring
    incIndex istring
    c

let skipWhitespace istring =
    while peekChar istring = Some ' ' do incIndex istring


let nextChar charTable istring =
    match peekChar istring with
    | None -> false
    | Some c -> if List.contains c charTable
                then incIndex istring
                     true
                else false

let chiffre = nextChar ['0'..'9']
let nonzero = nextChar ['1'..'9']

let entier istring =
    let decimalPart () =
        incIndex istring
        match peekChar istring with
        | None -> true
        | Some c -> while chiffre istring do ()
                    match peekChar istring with
                    | None -> true
                    | _ -> false

    match peekChar istring with
    | None -> false
    | Some c -> if c = '0'
                then incIndex istring
                     true
                elif c = '-'
                then decimalPart ()
                elif nonzero istring
                then decimalPart ()
                else false

[<EntryPoint>]
let main args =
    0
