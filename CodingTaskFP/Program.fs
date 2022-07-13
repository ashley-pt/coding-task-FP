// Learn more about F# at http://fsharp.org
module Program

open System
open System.IO
open FSharp.Collections
open System.Text.RegularExpressions

let readFile file = 
    let contents = File.ReadAllText(file)
    if contents.Length = 0
    then List.Empty
    else let cleanContents = Regex.Replace(contents, @"[^\w\s]", "").ToLower()
         let listOfLines = List.ofArray (cleanContents.Split('\n'))
         List.reduce(fun x y -> x @ y) <| List.map (fun (x:String) -> List.ofArray (x.Split(' '))) listOfLines

let cleanText (list:String List) =
    List.map (fun (s:String) -> s.Trim('\r')) list

let addWord (wordMap:Map<String, int>) (word:String) = 
    if wordMap.ContainsKey(word) 
    then let number = wordMap.[word]
         wordMap.Add(word, number + 1)
    else wordMap.Add(word, 1)

let countWords words = 
    let rec count xs acc = 
        match xs with
        | [] -> acc
        | h::t -> count t (addWord acc h)
    (count words Map.empty)

[<EntryPoint>]
let main argv =
    printfn "Enter file name: "
    let file = Console.ReadLine()
    let wordCount = countWords (cleanText(readFile file))
    if wordCount.IsEmpty
    then printfn "File is empty"
    else Console.WriteLine(Map.fold (fun acc word count -> acc + word + ": " + count.ToString() + "\n") "" wordCount)
    0 // return an integer exit code
