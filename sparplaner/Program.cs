  using System.Globalization;
using sparplaner;

var manager = new SparzielManager();
manager.Laden();
var kultur = new CultureInfo("de-DE");

bool laeuft = true;
while (laeuft)
{
    Console.Clear();
    Console.WriteLine("=== TASCHENGELD- & SPARZIEL-PLANER ===");
    Console.WriteLine("1) Sparziel anlegen");
    Console.WriteLine("2) Ziele auflisten");
    Console.WriteLine("3) Einzahlung erfassen");
    Console.WriteLine("4) Beenden");
    Console.Write("\nAuswahl: ");

    switch (Console.ReadLine())
    {
        case "1": ZielAnlegen(); break;
        case "2": ZieleAnzeigen(); break;
        case "3": EinzahlungErfassen(); break;
        case "4":
            manager.Speichern();
            laeuft = false;
            break;
        default: break;
    }
}

void ZielAnlegen()
{
    var ziel = new Sparziel();
    ziel.name = LiesText("Wofür sparst du? ");
    ziel.Zielbetrag = LiesBetrag("Zielbetrag (€): ", 0.01m);
    ziel.Gespart = LiesBetrag("Bereits gespart (€): ", 0);
    ziel.MonatlicheRate = LiesBetrag("Monatliche Sparrate (€): ", 0.01m);

    manager.Hinzufuegen(ziel);
    manager.Speichern();
    Console.WriteLine("\nZiel erfolgreich angelegt! Weiter mit Enter...");
    Console.ReadLine();
}

void ZieleAnzeigen()
{
    if (manager.Ziele.Count == 0)
    {
        Console.WriteLine("\nNoch keine Ziele vorhanden.");
    }
    else
    {
        foreach (var z in manager.Ziele)
        {
            ZeigeEinzelnesZiel(z);
        }
    }
    Console.WriteLine("\nWeiter mit Enter...");
    Console.ReadLine();
}

void ZeigeEinzelnesZiel(Sparziel z)
{
    Console.WriteLine("-------------------------------------------");
    Console.WriteLine($"Ziel: {z.name} ({z.Zielbetrag.ToString("C", kultur)})");
    Console.Write($"Gespart: {z.Gespart.ToString("C", kultur)} ");
    Console.Write(Fortschrittsbalken(z.Prozent()) + " ");
    Console.WriteLine($"{z.Prozent():0} %");

    if (z.IstErreicht())
    {
        Console.WriteLine("  -> Ziel erreicht! 🎉");
    }
    else if (z.MonateBisZiel() < 0)
    {
        Console.WriteLine("  -> Sparrate zu niedrig, Ziel aktuell nicht erreichbar.");
    }
    else
    {
        var monat = z.Zielmonat()?.ToString("MMMM yyyy", kultur);
        Console.WriteLine($"  -> Noch offen: {z.NochOffen().ToString("C", kultur)} (ca. {z.MonateBisZiel()} Monate bis {monat})");
    }
}

void EinzahlungErfassen()
{
    if (manager.Ziele.Count == 0) return;

    for (int i = 0; i < manager.Ziele.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {manager.Ziele[i].name}");
    }

    int index = (int)LiesBetrag("Wähle die Nummer des Ziels: ", 1) - 1;
    if (index < manager.Ziele.Count)
    {
        decimal betrag = LiesBetrag("Einzahlungsbetrag (€): ", 0.01m);
        manager.Ziele[index].Gespart += betrag;
        manager.Speichern();
        Console.WriteLine("Betrag gutgeschrieben!");
    }
    Console.ReadLine();
}

// Hilfsmethoden für Validierung & Grafik
string LiesText(string frage)
{
    string? eingabe;
    do
    {
        Console.Write(frage);
        eingabe = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(eingabe));
    return eingabe;
}

decimal LiesBetrag(string frage, decimal min)
{
    decimal wert;
    while (true)
    {
        Console.Write(frage);
        if (decimal.TryParse(Console.ReadLine(), out wert) && wert >= min)
            return wert;
        Console.WriteLine($"Bitte eine Zahl >= {min} eingeben.");
    }
}

string Fortschrittsbalken(double prozent, int breite = 20)
{
    int gefuellt = (int)(prozent / 100 * breite);
    gefuellt = Math.Clamp(gefuellt, 0, breite);
    return "[" + new string('#', gefuellt) + new string('-', breite - gefuellt) + "]";
}
