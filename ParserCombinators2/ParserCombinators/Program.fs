open System

open Streams
open Combinators
open Lexer
open Semantic
open Interpreter

let source = "var x = 0
var y = 2
if x = 1
then y + 3
else 5 - y"

let sourceStream = stream source
let tokens = lex sourceStream
let tokenStream = stream tokens
let ast = parseProgram tokenStream

interpret ast
|> Console.WriteLine