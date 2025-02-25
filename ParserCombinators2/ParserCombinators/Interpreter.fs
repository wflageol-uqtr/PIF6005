module Interpreter

open Lexer
open Semantic
open System.Collections.Generic

type ExecutingEnvironment = { Variables: Dictionary<string, int> }

let interpretDeclVar declVar env =
    env.Variables.Add (declVar.Name, declVar.Value)
    declVar.Value

let interpretOperand operand env =
    match operand with
    | Variable name -> env.Variables.[name]
    | Value value -> value

let interpretCondition (cond: Condition) env =
    let left = interpretOperand cond.Left env
    let right = interpretOperand cond.Right env
    left = right

let interpretOperation (op: Operation) env =
    let left = interpretOperand op.Left env
    let right = interpretOperand op.Right env

    match op.Operator with
    | Plus -> left + right
    | Minus -> left - right

let rec interpretIf declIf env =
    if interpretCondition declIf.Condition env
    then interpretOperation declIf.Then env
    else interpretOperation declIf.Else env

and interpretInstruction instr env =
    match instr with
    | Scalar operand -> interpretOperand operand env
    | If declIf -> interpretIf declIf env
    | Operation op -> interpretOperation op env
    | Condition cond -> if interpretCondition cond env
                        then 1 else 0
    | DeclVar declVar -> interpretDeclVar declVar env


let interpret program =
    let env = { Variables = Dictionary<string, int>() }

    let mutable result = 0
    for instr in program do
        result <- interpretInstruction instr env

    result
