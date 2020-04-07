namespace SchnauzEngineFSharp

module Karten =
    
    type Zahlwert = 
        | Ass 
        | Ober 
        | Unter 
        | Value of int

        static member Values() = 
            [ yield Ass
              for i in 6 .. 10 do yield Value i
              yield Ober
              yield Unter ]

    let zahlenwert a =
        match a with
        | Ass -> 11
        | Ober | Unter -> 10
        | Value n -> n


    type Farbwert = 
        | Eichel
        | Gras
        | Herz
        | Schellen

    type Karte = { Zahlwert: Zahlwert; Farbwert: Farbwert }
    let kartenwert a = 
        zahlenwert a.Zahlwert


    type Hand = list<Karte>

    type Stapel = list<Karte>

    let fullDeck = 
        [ for farbwert in [ Eichel; Gras; Herz; Schellen] do
              for zahlwert in Zahlwert.Values() do 
                  yield { Farbwert=farbwert; Zahlwert=zahlwert } ]

module Mischer =
    let rand = new System.Random()

    let mischen a =
        List.sortBy (fun elem -> rand.Next()) a

module HandAuswertung = 

    let wert b =
        let a = List.map Karten.kartenwert b
        List.sum a
