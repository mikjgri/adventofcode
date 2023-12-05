open System
open System.IO

let lines = File.ReadAllLines(__SOURCE_DIRECTORY__ + "\\input.txt")

let list = Seq.toList lines

let mutable result: int = 0;

for item in list do
    let charSeq = item |> Seq.toList
    let firstNumberOpt = Seq.find Char.IsDigit charSeq
    let lastNumberOpt = Seq.findBack Char.IsDigit charSeq

    let number = int(string firstNumberOpt + string lastNumberOpt)
    result <- result + number

printfn "Sum %d" result