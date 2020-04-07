module Tests

open System
open Xunit
open SchnauzEngineFSharp
open SchnauzEngineFSharp.Karten

[<Fact>]
let ``Deck hat 32 Karten`` () =
    Assert.Equal (Karten.fullDeck.Length, 32)

let rec checkUnique values =
    match values with
    | [] -> true
    | head :: tail -> (if List.contains head tail then false else checkUnique tail)

[<Fact>]
let ``Deck hat keine Karte doppelt`` () =
    Assert.True (checkUnique Karten.fullDeck)

let isEqualElement list1 list2 = List.exists2 (fun elem1 elem2 -> elem1 = elem2) list1 list2

[<Fact>]
let ``Deck ist gemischt`` () =
    let a = Mischer.mischen Karten.fullDeck
    Assert.NotEqual<Karte>(a, Karten.fullDeck)

[<Fact>]
let ``Schnauz hat 31`` () =
    let a = [{ Farbwert=Eichel; Zahlwert=Zahlwert.Value 10 }; {Farbwert=Eichel; Zahlwert=Ass}; {Farbwert=Eichel; Zahlwert=Ober}]
    Assert.Equal (HandAuswertung.wert a, 31)
