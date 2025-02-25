module Semantic

open Streams
open Combinators
open Lexer

type DeclVar = { Name: string; Value: int }
and Operand = Variable of string
            | Value of int
and Condition = { Left: Operand; Right: Operand }
and Operator = Plus
             | Minus
and Operation = { Left: Operand; Right: Operand; Operator: Operator }
and If = { Condition: Condition; Then: Operation; Else: Operation }
and Instruction = Scalar of Operand
                | If of If
                | Operation of Operation
                | Condition of Condition
                | DeclVar of DeclVar

let tokenKeyword name = one ((=) <| TokenKeyword name)
let tokenOperator name = one ((=) <| TokenOperator name)
let tokenVar = one (fun token -> match token with 
                                 | TokenVariable _ -> true
                                 | _ -> false)
let tokenInt = one (fun token -> match token with
                                 | TokenInt _ -> true
                                 | _ -> false)
let operand = either [tokenVar; tokenInt]
              |> convert (fun tokenSeq ->
                             match Seq.head tokenSeq with
                             | TokenVariable var -> [Variable var]
                             | TokenInt value -> [Value value]
                             | _ -> failwith "Should not happen.")

let condition = operand >>= skip (tokenOperator '=') >>= operand
                |> convert (fun tokenSeq ->
                            let tokens = Seq.toList tokenSeq
                            [{Condition.Left = tokens.[0]; Right = tokens.[1]}])

let addition = operand >>= skip (tokenOperator '+') >>= operand
               |> convert (fun tokenSeq ->
                           let tokens = Seq.toList tokenSeq
                           [{Operator = Plus; Left = tokens.[0]; Right = tokens.[1]}])

let subtraction = operand >>= skip (tokenOperator '-') >>= operand
                  |> convert (fun tokenSeq ->
                                let tokens = Seq.toList tokenSeq
                                [{Operator = Minus; Left = tokens.[0]; Right = tokens.[1]}])

let operation = either [addition; subtraction]

let declVar = skip (tokenKeyword "var") 
              >>= tokenVar 
              >>= skip (tokenOperator '=') 
              >>= tokenInt
              |> convert (fun tokenSeq ->
                          match Seq.toList tokenSeq with
                          | [TokenVariable var; TokenInt value] ->
                            [DeclVar { Name = var; Value = value }]
                          | _ -> failwith "Should not happen.")

// Trop complexe pour la librairie de parser combinators actuelle.
// Nécéssiterait une expression computationnelle, qui est un concept fonctionnel que nous n'avons pas vu dans ce cours.
// Implémentation "manuelle" à la place.
let declIf = fun stream ->
    match tokenKeyword "if" stream with
    | Ok (_, stream) ->
        match condition stream with
        | Ok (condition, stream) ->
            match tokenKeyword "then" stream with
            | Ok (_, stream) ->
                match operation stream with
                | Ok (opThen, stream) ->
                    match tokenKeyword "else" stream with
                    | Ok (_, stream) ->
                        match operation stream with
                        | Ok (opElse, stream) ->
                            Ok([If { Condition = Seq.head condition
                                     Then = Seq.head opThen
                                     Else = Seq.head opElse }], stream)
                        | Error -> Error
                    | Error -> Error
                | Error -> Error
            | Error -> Error
        | Error -> Error
    | Error -> Error

let parseProgram = many (either [declVar; declIf]) >> unwrap