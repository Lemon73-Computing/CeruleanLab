let myName = "apple"
// ↓ Error
// myName <- "fish"
printfn $"{myName}"

let mutable myAge = 200
myAge <- 300
printfn $"{myAge}"

let pi: float = 3.14

[<EntryPoint>]
let main argv =
    let number =
        if isNull argv[0] then $"{pi}"
        else argv[0]

    printfn $"{number}"
    0
