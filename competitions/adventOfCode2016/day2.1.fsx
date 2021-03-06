﻿open System

type Direction = Up | Down | Left | Right 
type SingleNumberInstructions = { Directions : Direction list }
type MultipleNumberInstructions = { Instructions : SingleNumberInstructions list }
type MultipleNumberInstructionsAccumulator = { CurrentPosition : int * int; CalculatedNumbers : int list } 

let parseDirection directionAsChar = 
    match directionAsChar with 
    | 'U' -> Up
    | 'D' -> Down
    | 'L' -> Left
    | 'R' -> Right
    | _ -> failwith "unknown direction"

let parseSingleNumberInstructions (singleNumberInstructionsAsString : string) = { Directions = singleNumberInstructionsAsString.ToCharArray() |> Array.map parseDirection |> List.ofArray }
let parseMultipleNumberInstructions (input : string) = { Instructions = input.Split('\n', '\r') |> Array.map parseSingleNumberInstructions |> List.ofArray }

let mutable currentNumber = 0

let seedFunc i j = 
    currentNumber <- currentNumber + 1
    currentNumber

let keypad = Array2D.init 3 3 seedFunc
let startKeyPosition = 1,1

let isInKeypad x y = x >= 0 && y >= 0 && x <= 2 && y <= 2
    
let moveTo direction currentPosition =
    let nextPosition = 
        match direction with 
        | Up   ->  fst currentPosition, snd currentPosition - 1
        | Down -> fst currentPosition, snd currentPosition + 1
        | Right -> fst currentPosition + 1,  snd currentPosition
        | Left -> fst currentPosition - 1, snd currentPosition

    match (isInKeypad (fst nextPosition) (snd nextPosition)) with 
    | true  -> nextPosition
    | false -> currentPosition

let computeSingleNumber singleNumberInstructions currentPosition  =
    let numberPosition = singleNumberInstructions.Directions |> List.fold (fun previousPosition instruction -> moveTo instruction previousPosition) currentPosition
    numberPosition, keypad.[snd numberPosition, fst numberPosition]

let computeBathroomCode (multipleNumberInstructions : MultipleNumberInstructions) =
    let sd accumulator instructions =
        let computedNumber = computeSingleNumber instructions accumulator.CurrentPosition
        { CurrentPosition = fst computedNumber; CalculatedNumbers = (snd computedNumber) :: accumulator.CalculatedNumbers }

    multipleNumberInstructions.Instructions |> List.fold sd { CurrentPosition = startKeyPosition; CalculatedNumbers = []}

let solve (input : string) =
    let formNumberFromDigits computationResult = 
        let digits = List.rev computationResult.CalculatedNumbers 
        digits |> List.fold (fun number digit -> number + string digit) "" |> Int32.Parse
        
    parseMultipleNumberInstructions input |> computeBathroomCode |> formNumberFromDigits