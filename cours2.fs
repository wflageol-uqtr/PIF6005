open System

type Scalar = Value of int
            | Var of char
type Cond = GreaterThan of Scalar * Scalar
          | LesserThan of Scalar * Scalar
          | GreaterEq of Scalar * Scalar
          | LesserEq of Scalar * Scalar
          | Equal of Scalar * Scalar
          | NonZero of Scalar
type If = { Cond: Cond
            Then: Expression
            Else: Expression }
and Expression = Scalar of Scalar
               | Cond of Cond
               | Return of Scalar
               | Assign of Scalar * Scalar
               | If of If
                
let interpretScalar s = 
    match s with
    | Value v -> v
    | Var v -> 10

let interpretCond c =
    match c with 
    | GreaterThan (a, b) -> (interpretScalar a) > (interpretScalar b)
    | GreaterEq (a, b) -> (interpretScalar a) >= (interpretScalar b)
    | LesserThan (a, b) -> (interpretScalar a) < (interpretScalar b)
    | LesserEq (a, b) -> (interpretScalar a) <= (interpretScalar b)
    | Equal (a, b) -> (interpretScalar a) = (interpretScalar b)
    | NonZero a -> (interpretScalar a) <> 0

let rec interpretIf i =
    if interpretCond i.Cond
    then interpret i.Then
    else interpret i.Else
and interpret expression = 
    match expression with
    | Scalar s -> failwith "Invalid scalar."
    | Cond c -> failwith "Invalid cond."
    | Return r -> printfn "Return: %A" r
    | Assign (a, b) -> printfn "Assignation: %A vers %A" b a
    | If i -> interpretIf i

[<EntryPoint>]
let main args =
    let code = If { Cond = GreaterThan (Var 'a', Value 5)
                    Then = Assign (Var 'b', Value 10)
                    Else = Return (Value 0) }

    interpret code
    0