open System.IO
open System

let checkIfContainsTodo (line : string, fileInfo : FileInfo) =
    if (line.ToLower().Contains("todo")) then printfn "In %s: %s" fileInfo.Name line

let printFileIntfo (fileInfo : FileInfo) =
    use file = File.OpenText(fileInfo.FullName)
    let mutable valid = true
    while (valid) do
        let line = file.ReadLine()
        if (line = null) then valid <- false
        else checkIfContainsTodo (line, fileInfo)


let main() =
    let footballPracticePath = @"/Users/lukaszlawicki/Documents/Coding/FootballPractice"
    for folder in Directory.GetDirectories(footballPracticePath, "*", SearchOption.AllDirectories) do

        let currentDirectory = new DirectoryInfo(folder)

        for csharpFile in currentDirectory.GetFiles("*.cs") do
            printFileIntfo (csharpFile)

main()
