open System

let rec somme nombres =
    match nombres with
    | [] -> failwith "Impossible de faire la somme d'une liste vide."
    | [x] -> x 
    | (x :: rest) -> x + somme rest

let rec reduire fonction nombres =
    match nombres with
    | [] -> failwith "Impossible de réduire une liste vide."
    | [x] -> x
    | (x :: rest) -> fonction x (reduire fonction rest)

let main args =
    let nombres = [1;2;3;4;5;6]
    let inc = (+) 1
    let incList = List.map inc
    let produit = List.reduce (*)

    incList nombres
    |> produit
    |> Console.WriteLine
    0

type Fraction = { Num: int; Den: int }
let fraction num den = { Num = num; Den = den }

let addFraction x y =
    if x.Den = y.Den
    then { x with Num = (x.Num + y.Num) }
    else failwith "Not supported."

[<EntryPoint>]
let main2 args =
    let x = fraction 1 3
    let y = fraction 2 3

    // Exemple d'utilisation de match avec balises.
    let vitesse = 3
    match vitesse with
    | 1 -> Console.WriteLine "1"
    | 2 -> Console.WriteLine "2"
    | 3 -> Console.WriteLine "3"
    | x when x < 10 -> Console.WriteLine "< 10"
    | _ -> Console.WriteLine "Default"

    0