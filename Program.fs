module Program

open AzureDevOpsCLI
open System.IO

let addTaskToDevOps (line : string, fileInfo : FileInfo) =
    let response =
        postDocRaw
            ("https://dev.azure.com/niobo/AzureDevOpsCITutorial/_apis/wit/workitems/$user%20story?api-version=5.1",
             "[{\r\n    \"op\": \"add\",\r\n    \"path\": \"\/fields\/System.Title\",\r\n    \"from\": null,\r\n    \"value\": \"Sample task\"\r\n  }]")
    printfn "Sialala"

let checkIfContainsTodo (line : string, fileInfo : FileInfo) =
    let trimedLine = line.Trim()
    if (trimedLine.ToLower().Contains("//todo") || trimedLine.ToLower().Contains("// todo")) then
        addTaskToDevOps (trimedLine, fileInfo)

let printFileInfo (fileInfo : FileInfo) =
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
            printFileInfo (csharpFile)

main()