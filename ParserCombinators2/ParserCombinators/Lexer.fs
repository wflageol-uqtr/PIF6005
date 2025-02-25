module Lexer

open System
open Combinators

type Token = TokenInt of int
           | TokenKeyword of string
           | TokenVariable of string
           | TokenOperator of char

let convertToInt (seq: char seq) : Token seq =
    let int = Seq.toArray seq |> String |> Int32.Parse
    [TokenInt int]

let convertToKeyword (seq: char seq) : Token seq =
    let string = Seq.toArray seq |> String
    [TokenKeyword string]

let convertToVariable (seq: char seq) : Token seq =
    let string = Seq.toArray seq |> String
    [TokenVariable string]

let convertToOperator (seq: char seq) : Token seq =
    [TokenOperator <| Seq.head seq]

// Parser qui valide que le prochain caractère fait partie de la liste fournie.
let chars (list: char list) : Parser<char, char> =
    one (fun c -> List.contains c list)

// Parser qui valide que le prochain mot correspond au mot fourni.
let word (string: String) : Parser<char, char> =
    Seq.map (fun e -> chars [e]) string
    |> Seq.reduce bind
    
let alpha = chars ['a'..'z']
let digit = chars ['0'..'9']
let alphanum = either [alpha; digit]
let spaces = many (chars [' '; '\n'; '\r'])
let op = chars ['-'; '+'; '='] |> convert convertToOperator
let integer = digit >>= many digit |> convert convertToInt
let keyword = either [word "var"
                      word "if"
                      word "then"
                      word "else"]
              |> convert convertToKeyword
let variable = alpha >>= many alphanum |> convert convertToVariable

let tokens = many <| (either [op; integer; keyword; variable] >>= skip spaces)

let lex stream =
    tokens stream |> unwrap